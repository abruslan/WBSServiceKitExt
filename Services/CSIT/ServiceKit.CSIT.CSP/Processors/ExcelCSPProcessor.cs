using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using ServiceKit.CSIT.CSP.Common;
using ServiceKit.CSIT.CSP.Data;
using ServiceKit.CSIT.CSP.Data.Entry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceKit.CSIT.CSP.Processors
{
    public class ExcelCSPProcessor
    {
        #region error codes
        public const int ERROR_INVALID_TITLE_ATTRIBUTE  = 1;
        public const int ERROR_TABLE_NOT_FOUND          = 2;
        public const int ERROR_INVALID_TABLE_ATTRIBUTE  = 3;
        #endregion
        //private Register register;
        public int ErrorCode = 0;
        public string ErrorMessage = "";
        public string ErrorContent = "";

        private readonly ApplicationDbContext _context;
        private RegisterLogger _logger;

        public ExcelCSPProcessor(ApplicationDbContext context)
        {
            _context = context;
            _logger = new RegisterLogger(_context);
        }

        public Register Eval(IFormFile Excel)
        {
            var register = CreateRegisterFromFile(Excel);
            if (ParseTitle(register) && ParseContent(register))
                return register;
            return null;
        }

        private Register CreateRegisterFromFile(IFormFile Excel)
        {
            var register = new Register()
            {
                Status = RegisterStatus.Imported,
                FileName = Excel.FileName,
                FileLength = Excel.Length,
                ContentType = Excel.ContentType,
                Items = new List<RegisterItem>()
            };
            if (Excel.Length > 0)
            {
                using (var target = new MemoryStream())
                {
                    Excel.CopyTo(target);
                    register.Data = target.ToArray();
                }
            }
            return register;
        }
        private bool ParseTitle(Register register)
        {
            string attr = "";
            try
            {
                using (var workbook = new XLWorkbook(new MemoryStream(register.Data)))
                {
                    var worksheet = workbook.Worksheets.FirstOrDefault();
                    register.Contractor = worksheet.GetAttribute(attr = "Исполнитель:");
                    register.ContractorINN = worksheet.GetAttribute(attr = "ИНН:");
                    register.CustomerName = worksheet.GetAttribute(attr = "Заказчик:");
                    register.CustomerINN = worksheet.GetAttribute(attr = "ИНН:", 2);
                    register.Account = worksheet.GetAttribute(attr = "Счет №");
                    register.AccountDate = DateTime.Parse(worksheet.GetAttribute(attr = "от"));
                    register.Invoice = worksheet.GetAttribute(attr = "Накладная/Акт №");
                    register.Contract = worksheet.GetAttribute(attr = "Договор");
                    var period = worksheet.GetAttribute(attr = "Период продажи:");
                    if (period.Split('-').Length > 1)
                    {
                        register.SaleFrom = DateTime.Parse(period.Split('-')[0].Trim());
                        register.SaleTo = DateTime.Parse(period.Split('-')[1].Trim());
                    }
                    register.Currency = worksheet.GetAttributeStartWith(attr = "валюта:");
                }
            }
            catch (Exception ex)
            {
                ErrorCode = ERROR_INVALID_TITLE_ATTRIBUTE;
                ErrorMessage = $"Ошибка разбора заголовка файла. Анализ атрибута {attr}";
                ErrorContent = ex.ToString();
                return false;
            }
            return true;
        }

        private bool ParseContent(Register register)
        {
            int row = 0;
            int errorCount = 0;
            try
            {
                using (var workbook = new XLWorkbook(new MemoryStream(register.Data)))
                {
                    var worksheet = workbook.Worksheets.FirstOrDefault();

                    int firstrow = worksheet.FindRowWith("Артикул");
                    if (firstrow <= 0)
                    {
                        ErrorCode = ERROR_TABLE_NOT_FOUND;
                        ErrorMessage = "Не найдена таблица. Таблица должна начинаться с колонки Атикул";
                        return false;
                    }

                    for (row = firstrow + 1; row < 10_000; row++)
                    {
                        if (string.IsNullOrEmpty(worksheet.Cell(row, 1)?.Value?.ToString().Trim()))
                            break;
                        if (errorCount >= 5)
                            break;

                        var item = new RegisterItem(); // { Register = register };
                        register.Items.Add(item);

                        ParseValues();

                        #region вспомогательные функции
                        void ParseValues()
                        {
                            var col = 1;
                            item.Number = getAttrString(col++);
                            item.ServiceName = getAttrString(col++).Trim();
                            item.ClientName = getAttrString(col++).Trim();
                            item.ClientINN = getAttrString(col++).Trim();
                            item.WorkPlaceCount = getAttrInt(col++);
                            item.PeriodFrom = getAttrDate(col++);
                            item.PeriodTo = getAttrDate(col++);
                            item.DayCount = getAttrInt(col++);
                            item.RegisterRateText = getAttrString(col++).Trim();
                            item.RegisterRate = getAttrDecimal(col++);
                        }

                        int getAttrInt(int col)
                        {
                            if (string.IsNullOrEmpty(worksheet.Cell(row, col).Value.ToString()))
                                return 0;
                            try
                            {
                                return Convert.ToInt32(worksheet.Cell(row, col).Value);
                            }
                            catch { }
                            return 0;
                        }
                        decimal getAttrDecimal(int col)
                        {
                            if (string.IsNullOrEmpty(worksheet.Cell(row, col).Value.ToString()))
                                return 0;
                            try
                            {
                                return Convert.ToDecimal(worksheet.Cell(row, col).Value);
                            }
                            catch { }
                            return 0;
                        }
                        DateTime getAttrDate(int col)
                        {
                            if (string.IsNullOrEmpty(worksheet.Cell(row, col).Value.ToString()))
                                return DateTime.MinValue;
                            try
                            {
                                return Convert.ToDateTime(worksheet.Cell(row, col).Value);
                            }
                            catch { }
                            return DateTime.MinValue;
                        }
                        string getAttrString(int col)
                            => worksheet.Cell(row, col).Value.ToString();
                        #endregion
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorCode = ERROR_INVALID_TABLE_ATTRIBUTE;
                ErrorMessage = $"Ошибка разбора содержимого файла. Анализ строки {row}";
                ErrorContent = ex.ToString();
                return false;
            }
            return true;
        }
    }
}
