using Microsoft.AspNetCore.Http;
using ServiceKit.Model.WBS.Entry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceKit.WBS.Processors
{
    public interface IExcelProcessor
    {
        public WBS_Request Eval(IFormFile Excel);
        public List<string> ErrorMessage { get; }
    }
}
