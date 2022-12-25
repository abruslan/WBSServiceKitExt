using Microsoft.EntityFrameworkCore;
using ServiceKit.Model.WBS;
using ServiceKit.Model.WBS.Entry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceKit.WBS.Processors.Request
{
    public class ProjectRequestProcessor : RequestProcessor, IRequestProcessor
    {

        public ProjectRequestProcessor(AppDbContext context) : base(context)
        {
        }

        public WBS_Project Eval(WBS_Request request)
        {
            //Check(request);
            var project = RecreateProject(request);

            // Сохранили в лог текущую версию
            //SaveLog(project);

            if (project != null)
                _context.SaveChanges();
            return project;
        }

        private void SaveLog(WBS_Project project)
        {
            throw new NotImplementedException();
        }

        public bool Check(WBS_Request request)
        {
            ErrorMessage.Clear();
            var project = _context.WBS_Projects.FirstOrDefault(r => r.ProjectCode == request.ProjectCode && !r.IsDeleted);
            if (project == null)
            {
                ErrorMessage.Add($"Не найден проект с кодом {request.ProjectCode}");
                return false;
            }
            if (string.IsNullOrEmpty(project.ProjectShortName))
            {
                ErrorMessage.Add($"не заполнено которкое название проекта, необходимое для проверки длины наименования в 1С.");
                return false;
            }

            foreach (var item in request.ProjectItems.Where(r => r.Level == 1 && !r.IsDeleted))
            {
                item.Full1CName = $"{project.ProjectShortName}_{item.ShortName}";
                _CheckItem(request, item);
            }
            return ErrorMessage.Count == 0;
        }

        private void _CheckItem(WBS_Request request, WBS_RequestProjectItem parent)
        {
            if (string.IsNullOrWhiteSpace(parent.ShortName))
            {
                ErrorMessage.Add(parent.ErrorMessage = $"Не заполнено наименование для {parent.FullCode}");
            }

            foreach (var item in request.ProjectItems.Where(r => r.Level == parent.Level + 1 && r.FullCode.Contains(parent.FullCode) && !r.IsDeleted))
            {
                item.ErrorMessage = "";

                // проверим длину для каждого ребенка
                item.Full1CName = $"{parent.Full1CName}_{item.ShortName}";
                if (item.Full1CName.Length > FULL_NAME_MAX_LENGTH)
                {
                    // если у элемента превышена длина наименования, глубже уже нет смысла идти
                    item.ErrorMessage = $"Итоговая длина наименования для {item.FullCode} превышает допустимую длину в {FULL_NAME_MAX_LENGTH} символов на {item.Full1CName.Length - FULL_NAME_MAX_LENGTH}: '{item.Full1CName}'";
                    ErrorMessage.Add(item.ErrorMessage);
                }

                // проверим длину наименования для всех его детей
                _CheckItem(request, item);
            }
        }

        private WBS_Project RecreateProject(WBS_Request request)
        {
            var project = _context.WBS_Projects.Include(r => r.ProjectItems).Where(r => r.ProjectCode == request.ProjectCode && !r.IsDeleted).FirstOrDefault();
            if (project == null)
            {
                ErrorMessage.Add("Отсутствует проект для загрузки заявки");
                return null;
                //project = new WBS_Project() { ProjectItems = new List<WBS_ProjectItem>() };
                //_context.WBS_Projects.Add(project);
            }
            //project.ProjectCode = request.ProjectCode;
            //project.ProjectName = request.ProjectName;

            // все содержимое удаляем
            _context.RemoveRange(project.ProjectItems);

            var cache = new List<WBS_ProjectItem>();
            foreach(var item in request.ProjectItems.OrderBy(r => r.OrderNum))
            {
                var projectItem = new WBS_ProjectItem()
                {
                    Id = Guid.NewGuid(),
                    Level = item.Level,
                    OrderNum = item.OrderNum,
                    ShortCode = item.ShortCode,
                    FullCode = $"{request.ProjectCode}_{item.FullCode}",
                    ShortName = item.ShortName,
                    Comment = item.Comment,
                };
                cache.Add(projectItem);

                // родитель - предыдущий в упорядоченном по "№ п/п" списке, у которого уровень на 1 меньше
                if (projectItem.Level > 1)
                    projectItem.ParentId = 
                        //request.ProjectItems
                        cache
                            .Where(r => r.Level == (projectItem.Level - 1) && r.OrderNum < projectItem.OrderNum)
                            .OrderBy(r => r.OrderNum).LastOrDefault()?.Id;

                project.ProjectItems.Add(projectItem);

                // Starting with EF Core 3.0, if an entity is using generated key values and some key value is set, then the entity will be tracked in the Modified state.
                _context.Entry(projectItem).State = EntityState.Added;
            }
            return project;
        }
    }
}
