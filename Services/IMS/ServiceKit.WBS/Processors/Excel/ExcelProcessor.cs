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
    public class ExcelProcessor
    {
        #region error codes
        public const int ERROR_INVALID_TITLE_ATTRIBUTE = 1;
        public const int ERROR_TABLE_NOT_FOUND = 2;
        public const int ERROR_INVALID_TABLE_ATTRIBUTE = 3;
        #endregion

        private readonly AppDbContext _context;
        public int ErrorCode = 0;
        public List<string> ErrorMessage { get; set; } = new List<string>();
        public string ErrorContent = "";

        public ExcelProcessor(AppDbContext context)
        {
            _context = context;
        }

        protected WBS_Request CreateFromFile(IFormFile Excel, WBS_RequestType requestType)
        {
            var register = new WBS_Request()
            {
                Status = WBS_RequestStatus.New,
                FileName = Excel.FileName,
                FileLength = Excel.Length,
                ContentType = Excel.ContentType,
                RequestType = requestType,
                Changes = new List<WBS_RequestItem>(),
                ProjectItems = new List<WBS_RequestProjectItem>()
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


        protected int _getAttrInt(IXLWorksheet worksheet, int row, int col)
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
        protected decimal _getAttrDecimal(IXLWorksheet worksheet, int row, int col)
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
        protected DateTime _getAttrDate(IXLWorksheet worksheet, int row, int col)
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
        protected string _getAttrString(IXLWorksheet worksheet, int row, int col)
            => worksheet.Cell(row, col).GetString().Trim();

    }
}
