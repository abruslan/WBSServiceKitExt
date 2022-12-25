using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceKit.ExternalSystem.Common
{
    public class ExternalSystemLogItem
    {
        public int Level { get; set; } = 0; // 0 - info, 1 - warning, 2 - error, 3 - critical
        public string Message { get; set; }
        public string Source { get; set; }
        public Guid? SystemId { get; set; }
    }
    public class ExternalSystemLogInfo
    {
        private IExternalSystemConfiguration _configuration;

        public ExternalSystemLogInfo(IExternalSystemConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool Checked { get; set; } = false;
        public bool Success { get; set; } = false;
        public List<ExternalSystemLogItem> Log { get; set; } = new List<ExternalSystemLogItem>();
        public Exception Exception { get; set; }
        public DateTime WhenChecked { get; set; }


        public void AddLog(string message) {
            Log.Add(new ExternalSystemLogItem() { Source = _configuration.Name, Message = message, SystemId = _configuration.SourceId });
        }
        public void AddLog(string message, int level) {
            Log.Add(new ExternalSystemLogItem() { Source = _configuration.Name, Message = message, Level = level, SystemId = _configuration.SourceId });
        }
    }
}
