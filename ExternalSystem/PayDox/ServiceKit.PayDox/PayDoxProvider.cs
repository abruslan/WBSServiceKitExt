using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ServiceKit.ExternalSystem.Common;
using ServiceKit.Model.WBS.Entry;
using ServiceKit.Model.WBS.PayDox;

namespace ServiceKit.PayDox
{
    public class PayDoxProvider
    {
        private readonly IExternalSystemConfiguration _configuration;
        private readonly SqlConnection con;
        public ExternalSystemLogInfo CheckInfo;
        public ExternalSystemLogInfo SyncInfo;

        private DateTime dtRequest;
        private Guid guidRequest;
        private int inserted;

        private int sql_status = 0;
        private string sql_errormessage = "";

        public PayDoxProvider(IExternalSystemConfiguration configuration)
        {
            _configuration = configuration;
            con = new SqlConnection(_configuration.GetConnectionString());
            CheckInfo = new ExternalSystemLogInfo(_configuration);
            SyncInfo = new ExternalSystemLogInfo(_configuration);
        }

        public bool CheckConnect()
        {
            CheckInfo = new ExternalSystemLogInfo(_configuration);
            CheckInfo.WhenChecked = DateTime.Now;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT count(*) FROM sysobjects";
                    cmd.ExecuteScalar();
                    CheckInfo.AddLog("Соединение установлено");

                    cmd.CommandText = "SELECT count(*) FROM WBS_SyncRequests";
                    CheckInfo.AddLog($"Таблица WBS_SyncRequests найдена ({cmd.ExecuteScalar()})");

                    cmd.CommandText = "SELECT count(*) FROM WBS_SyncRequestItems";
                    CheckInfo.AddLog($"Таблица WBS_SyncRequestItems найдена ({cmd.ExecuteScalar()})");

                    con.Close();
                    CheckInfo.Success = true;
                }
            }
            catch (Exception ex)
            {
                CheckInfo.AddLog($"Ошибка {ex.Message}", 2);
                CheckInfo.Exception = ex;
                CheckInfo.Success = false;
            }
            return CheckInfo.Success;
        }

        public void SyncProject(WBS_Project project, Guid requestId)
        {
            try
            {
                SyncInfo = new ExternalSystemLogInfo(_configuration);
                dtRequest = DateTime.Now;
                guidRequest = requestId;
                SyncInfo.AddLog($"Публикация в систему {_configuration.Name} начата");

                con.Open();

                CreateSyncRequest(project);
                SyncInfo.AddLog($"Запись запроса успешно создана id={requestId}");

                inserted = 0;
                CreateSyncRequestItems(project);
                SyncInfo.AddLog($"Загружено записей: {inserted}");

                ExecSyncSP(project);
                SyncInfo.AddLog($"Выполнена процедура синхронизации. Код возврата {sql_status} {sql_errormessage}");

                con.Close();

                SyncInfo.Success = true;
            }
            catch (Exception ex)
            {
                SyncInfo.AddLog($"Ошибка {ex.Message}", 2);
                SyncInfo.Exception = ex;
                SyncInfo.Success = false;
            }
        }

        private void CreateSyncRequest(WBS_Project project)
        {
            var q = " INSERT INTO [dbo].[WBS_SyncRequests]([RequestId],[ProjectCode],[ProjectName],[Status],[ErrorMessage],[Created]) " +
                    " VALUES(@RequestId, @ProjectCode, @ProjectName, 0, '', @Created)";
            using (SqlCommand cmd = new SqlCommand(q, con))
            {
                cmd.Parameters.Add("@RequestId", System.Data.SqlDbType.UniqueIdentifier).Value = guidRequest;
                cmd.Parameters.Add("@ProjectCode", System.Data.SqlDbType.VarChar, 100).Value = project.ProjectCode;
                cmd.Parameters.Add("@ProjectName", System.Data.SqlDbType.VarChar, 100).Value = project.ProjectName;
                cmd.Parameters.Add("@Created", System.Data.SqlDbType.DateTime).Value = dtRequest;
                cmd.ExecuteNonQuery();
            }
        }


        public void CreateSyncRequestItems(WBS_Project project)
        {
            var q = " INSERT INTO [dbo].[WBS_SyncRequestItems] ([RequestId],[Id],[ParentId],[Level],[ShortCode],[FullCode],[ShortName],[FullName],[Comment],[Status],[ErrorMessage],[Created])" +
                     " VALUES (@RequestId,@Id,@ParentId,@Level,@ShortCode,@FullCode,@ShortName,@FullName,@Comment,0,'',@Created)";
            using (SqlCommand cmd = new SqlCommand(q, con))
            {
                foreach (var item in project.ProjectItems.Where(r => r.Level == 1 && !r.IsDeleted))
                    _CreateSyncRequestItem(cmd, project, item, null, 1);
            }
        }

        public List<WBS_SyncRequestItem> ReadSyncRequestItems(WBS_Project project, Guid request)
        {
            var ret = new List<WBS_SyncRequestItem>();
            var q = $" SELECT * FROM [dbo].[WBS_SyncRequestItems] WHERE [RequestId] = '{request}'";
            using (SqlCommand cmd = new SqlCommand(q, con))
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        try
                        {
                            ret.Add(new WBS_SyncRequestItem()
                            { 
                                FullCode = reader["FullCode"].ToString(),
                                FullName = reader["FullName"].ToString(),
                                Comment = reader["Comment"].ToString(),
                                Status = Convert.ToInt32(reader["Status"]),
                                ErrorMessage = reader["ErrorMessage"].ToString()
                            });
                        }
                        catch (Exception ex)
                        {

                            //throw;
                        }
                    }
                }
                   
            }
            return ret;
        }

        public void ExecSyncSP(WBS_Project project)
        {
            // только проверка
            //var q = $"EXEC [dbo].[PDX_ISR_Synchronize] '{guidRequest}', 0, 1, 'AutoSync'  SELECT Status, ErrorMessage FROM WBS_SyncRequests WHERE RequestId = '{guidRequest}'";
            // боевая синхронизация
            var q = $"EXEC [dbo].[PDX_ISR_Synchronize] '{guidRequest}', 1, 0, 'AutoSync'";
            using (SqlCommand cmd = new SqlCommand(q, con))
            {
                cmd.ExecuteNonQuery();
            }

            q = $"SELECT Status, ErrorMessage FROM WBS_SyncRequests WHERE RequestId = '{guidRequest}'";
            sql_errormessage = $"Не найдена запись с запросом {guidRequest} в таблице WBS_SyncRequests";
            using (SqlCommand cmd = new SqlCommand(q, con))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        try
                        {
                            sql_status = Convert.ToInt32(reader["Status"]);
                            sql_errormessage = reader["ErrorMessage"].ToString();
                        }
                        catch (Exception ex)
                        {

                            //throw;
                        }
                    }
                }
            }
        }

        private void _CreateSyncRequestItem(SqlCommand cmd, WBS_Project project, WBS_ProjectItem item, Guid? parentId, int level)
        {
            // защита от зацикливания
            if (level > 10) return;

            var id = Guid.NewGuid();
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@RequestId", System.Data.SqlDbType.UniqueIdentifier).Value = guidRequest;
            cmd.Parameters.Add("@Id", System.Data.SqlDbType.UniqueIdentifier).Value = id;
            cmd.Parameters.Add("@ParentId", System.Data.SqlDbType.UniqueIdentifier).Value = parentId??Guid.Empty;
            cmd.Parameters.Add("@Level", System.Data.SqlDbType.Int).Value = item.Level;
            cmd.Parameters.Add("@ShortCode", System.Data.SqlDbType.VarChar, 100).Value = item.ShortCode;
            cmd.Parameters.Add("@FullCode", System.Data.SqlDbType.VarChar, 100).Value = item.FullCode;
            cmd.Parameters.Add("@ShortName", System.Data.SqlDbType.VarChar, 100).Value = item.ShortName;
            cmd.Parameters.Add("@FullName", System.Data.SqlDbType.VarChar, 100).Value = item.ShortName;
            cmd.Parameters.Add("@Comment", System.Data.SqlDbType.VarChar, 100).Value = item.Comment;
            cmd.Parameters.Add("@Created", System.Data.SqlDbType.DateTime).Value = dtRequest;
            cmd.ExecuteNonQuery();

            inserted++;

            foreach (var child in project.ProjectItems.Where(r => r.ParentId == item.Id && !r.IsDeleted))
                _CreateSyncRequestItem(cmd, project, child, id, level+1);
        }

        private string IfNull(params string[] strings)
        {
            foreach(var val in strings)
                if (!string.IsNullOrEmpty(val))
                    return val;
            return "";
        }
    }
}
