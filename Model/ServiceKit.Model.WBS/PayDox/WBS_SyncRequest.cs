using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceKit.Model.WBS.PayDox
{
    /// <summary>
    /// PayDox Table WBS_SyncRequests
    /// </summary>
    public class WBS_SyncRequest
    {
        [Key]
        public Guid RequestId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public int Status { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime Created { get; set; }
    }
}
