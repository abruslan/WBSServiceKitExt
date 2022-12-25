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
    public class ExcelProjectRequestProcessor : ExcelProcessor, IExcelProcessor
    {

        public ExcelProjectRequestProcessor(AppDbContext context) : base(context)
        {
        }

        public WBS_Request Eval(IFormFile Excel)
        {
            ErrorMessage.Clear();

            var request = CreateFromFile(Excel, WBS_RequestType.Project);

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
                    request.ProjectCode = worksheet.Cell("D12").GetString().Trim();
                    request.ProjectName = worksheet.Cell("A3").GetString().Trim();
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
                ErrorMessage.Add($"Не удалось определить код проекта. Проверьте атрибут в ячейке D12");
                return false;
            }
            if (string.IsNullOrEmpty(request.ProjectName))
            {
                ErrorCode = ERROR_INVALID_TITLE_ATTRIBUTE;
                ErrorMessage.Add($"Не удалось определить код проекта. Проверьте атрибут в ячейке A3");
                return false;
            }
            return true;
        }


        private bool ParseContent(WBS_Request request)
        {
            int row = 0;
            try
            {
                using (var workbook = new XLWorkbook(new MemoryStream(request.Data)))
                {
                    var worksheet = workbook.Worksheets.FirstOrDefault();

                    int firstrow = worksheet.FindRowWith("Направление");
                    if (firstrow <= 0)
                    {
                        ErrorCode = ERROR_TABLE_NOT_FOUND;
                        ErrorMessage.Add("Не найдена таблица. Таблица должна начинаться с колонки 'Направление'");
                        return false;
                    }

                    int orderNum = 1;

                    for (row = firstrow + 1; row < 10_000; row++)
                    {
                        // Находим не более 5 ошибок. Затем останавливаемся.
                        if (ErrorMessage.Count >= 30)
                            break;

                        if (string.IsNullOrEmpty(getAttrStringRange(1,6)))
                            break;

                        if (ErrorWhen(string.IsNullOrEmpty(getAttrStringRange(1, 5)), $"Не заполнен ни один код в строке {row}."))
                            continue;

                        // создадим элемент, который будем заполнять
                        var item = new WBS_RequestProjectItem();

                        if (!ParseValues())
                            continue;

                        if (ErrorWhen(item.FullCode.Length != item.Level * 2, $"Ошибка в строке {row}: количество символов элемента {item.FullCode} уровня {item.Level} должно быть {item.Level * 2}. "))
                            continue;

                        if (ErrorWhen(request.ProjectItems.Any(r => r.FullCode == item.FullCode), $"Ошибка в строке {row}: дубликат полного кода элемента {item.FullCode} уровня {item.Level}. "))
                            continue;

                        if (ErrorWhen(!item.FullCode.EndsWith(item.ShortCode), $"Ошибка в строке {row}: полный код элемента {item.FullCode} должен заканчиваться на краткий код {item.ShortCode}. "))
                            continue;

                        request.ProjectItems.Add(item);
                        orderNum++;

                        #region вспомогательные функции
                        bool ParseValues()
                        {
                            item.OrderNum = orderNum;
                            item.ShortName = getAttrString(6);
                            item.Comment = getAttrString(8);

                            // LEVEL 3
                            if (!string.IsNullOrEmpty(getAttrString(5)))
                            {
                                item.Level = 3;
                                item.FullCode = getAttrString(4);
                                item.ShortCode = getAttrString(5);
                                if (ErrorWhen(string.IsNullOrEmpty(item.FullCode), $"Не заполнен полный код в строке {row} в столбце {4}. "))
                                    return false;
                                return true;
                            }

                            // LEVEL 2
                            if (!string.IsNullOrEmpty(getAttrString(3)))
                            {
                                item.Level = 2;
                                item.FullCode = getAttrString(2);
                                item.ShortCode = getAttrString(3);
                                if (ErrorWhen(string.IsNullOrEmpty(item.FullCode), $"Не заполнен полный код в строке {row} в столбце {2}. "))
                                    return false;
                                return true;
                            }

                            // LEVEL 1
                            item.Level = 1;
                            item.FullCode = getAttrString(1);
                            item.ShortCode = getAttrString(1);
                            if (ErrorWhen(string.IsNullOrEmpty(item.FullCode), $"Не заполнен полный код в строке {row} в столбце {1}. "))
                                return false;
                            return true;
                        }

                        bool ErrorWhen(bool predicate, string message)
                        {
                            if (predicate)
                                ErrorMessage.Add(message);
                            return predicate;
                        }

                        string getAttrString(int col)
                            => _getAttrString(worksheet, row, col).Trim();
                        string getAttrStringRange(int col1, int col2)
                        {
                            var ret = "";
                            for (int col = col1; col <= col2; col++)
                            {
                                var s = _getAttrString(worksheet, row, col).Trim();
                                ret += s ?? "";
                            }
                            return ret;
                        }
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
            return ErrorMessage.Count == 0;
        }
    }
}
