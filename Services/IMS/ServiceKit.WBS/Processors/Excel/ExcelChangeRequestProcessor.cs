using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using ServiceKit.Model.WBS;
using ServiceKit.Model.WBS.Entry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceKit.WBS.Processors
{
    public class ExcelChangeRequestProcessor : ExcelProcessor, IExcelProcessor
    {
        public ExcelChangeRequestProcessor(AppDbContext context) : base(context)
        {
        }

        public WBS_Request Eval(IFormFile Excel)
        {
            var request = CreateFromFile(Excel, WBS_RequestType.Changes);

            if (ParseTitle(request) && ParseContent(request))
                return request;
            return null;
        }

        private bool ParseTitle(WBS_Request request)
        {
            string attr = "";
            try
            {
                using (var workbook = new XLWorkbook(new MemoryStream(request.Data)))
                {
                    var worksheet = workbook.Worksheets.FirstOrDefault();
                    request.ProjectCode = worksheet.GetAttribute(attr = "1.3 КОД ПРОЕКТА").Trim();
                    request.ProjectName = worksheet.GetAttribute(attr = "1.4 ПРОЕКТ:").Trim();
                }
            }
            catch (Exception ex)
            {
                ErrorCode = ERROR_INVALID_TITLE_ATTRIBUTE;
                ErrorMessage.Add($"Ошибка разбора заголовка файла. Анализ атрибута {attr}");
                ErrorContent = ex.ToString();
                return false;
            }
            if (string.IsNullOrEmpty(request.ProjectCode))
            {
                ErrorCode = ERROR_INVALID_TITLE_ATTRIBUTE;
                ErrorMessage.Add($"Не удалось определить код проекта. Проверьте атрибут правее ячейки '1.3 КОД ПРОЕКТА'");
                return false;
            }
            return true;
        }


        private bool ParseContent(WBS_Request request)
        {
            int row = 0;
            int errorCount = 0;
            try
            {
                using (var workbook = new XLWorkbook(new MemoryStream(request.Data)))
                {
                    if (workbook.Worksheets.Count < 2)
                    {
                        ErrorCode = ERROR_TABLE_NOT_FOUND;
                        ErrorMessage.Add($"Ошибка разбора содержимого файла. Файл должен содержать не менее двух листов");
                        return false;
                    }

                    var worksheet = workbook.Worksheets.ElementAt(1);

                    int firstrow = worksheet.FindRowWith("Код ИСР");
                    if (firstrow <= 0)
                    {
                        ErrorCode = ERROR_TABLE_NOT_FOUND;
                        ErrorMessage.Add("Не найдена таблица. Таблица должна начинаться с колонки 'Код ИСР'");
                        return false;
                    }

                    for (row = firstrow + 1; row < 10_000; row++)
                    {
                        if (string.IsNullOrEmpty(getAttrString(1)) && string.IsNullOrEmpty(getAttrString(4)))
                            break;
                            
                        if (errorCount >= 5)
                            break;

                        var item = new WBS_RequestItem();
                        request.Changes.Add(item);

                        ParseValues();

                        #region вспомогательные функции
                        void ParseValues()
                        {
                            var col = 1;
                            item.Code = getAttrString(col++);
                            item.Name = getAttrString(col++);
                            item.Comment = getAttrString(col++);
                            item.NewCode = getAttrString(col++);
                            item.NewName = getAttrString(col++);
                        }

                        string getAttrString(int col) => _getAttrString(worksheet, row, col).Trim();
                        #endregion
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorCode = ERROR_INVALID_TABLE_ATTRIBUTE;
                ErrorMessage.Add($"Ошибка разбора содержимого файла. Анализ строки {row}");
                ErrorContent = ex.ToString();
                return false;
            }
            return true;
        }
    }
}
