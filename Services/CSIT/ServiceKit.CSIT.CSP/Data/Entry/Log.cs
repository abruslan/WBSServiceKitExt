using Microsoft.Extensions.Logging;
using ServiceKit.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceKit.CSIT.CSP.Data.Entry
{
    public class Log : BaseEntity
    {
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public Guid? Source { get; set; }
        public LogLevel LogLevel { get; set; }
        public string Message { get; set; }
        public string Area { get; set; }
        public string Data { get; set; }
    }
}
