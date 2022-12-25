using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// todo: выделить в независимую библиотеку
namespace ServiceKit.WBS.Processors
{
    public static class ExcelProcessorHelper
    {
        private const string HeaderArea = "A1:K20";

        public static string GetAttribute(this IXLWorksheet worksheet, string value, int number = 1, int col_offset = 1)
        {
            int count = 0;
            foreach (var cell in worksheet.Cells(HeaderArea))
            {
                var cellValue = cell.Value.ToString();
                if (cellValue.Equals(value, StringComparison.InvariantCultureIgnoreCase))
                {
                    count++;
                    if (number == count)
                        return worksheet.Cell(cell.WorksheetRow().RowNumber(), cell.WorksheetColumn().ColumnNumber() + col_offset).Value.ToString().Trim();
                }
            }
            return "";
        }

        public static string GetAttributeStartWith(this IXLWorksheet worksheet, string value, int number = 1)
        {
            int count = 0;
            foreach (var cell in worksheet.Cells(HeaderArea))
            {
                var cellValue = cell.Value.ToString();
                if (cellValue.StartsWith(value, StringComparison.InvariantCultureIgnoreCase))
                {
                    count++;
                    if (number == count)
                        return worksheet.Cell(cell.WorksheetRow().RowNumber(), cell.WorksheetColumn().ColumnNumber() + 1).Value.ToString().Trim();
                }
            }
            return "";
        }

        public static int FindRowWith(this IXLWorksheet worksheet, string value)
        {
            foreach (var cell in worksheet.Cells(HeaderArea))
            {
                var cellValue = cell.Value.ToString();
                if (cellValue.Equals(value, StringComparison.InvariantCultureIgnoreCase))
                    return cell.WorksheetRow().RowNumber();
            }
            return 0;
        }

    }
}
