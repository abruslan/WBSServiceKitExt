using ServiceKit.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceKit.Model.WBS.Entry
{
    //[Table(nameof(WBS_Request), Schema = Common.Schema)]
   public class WBS_Request : BaseEditedEntity
    {
        [Display(Name = "Статус")]
        public WBS_RequestStatus Status { get; set; }

        [Display(Name = "Код проекта")]
        [StringLength(100)]
        public string ProjectCode { get; set; }

        [Display(Name = "Наименование проекта")]
        public string ProjectName { get; set; }

        [Display(Name = "Тип заявки")]
        public WBS_RequestType RequestType { get; set; }

        public virtual List<WBS_RequestItem> Changes { get; set; }
        public virtual List<WBS_RequestProjectItem> ProjectItems { get; set; }

        public string FileName { get; set; }
        public long FileLength { get; set; }
        public string ContentType { get; set; }

        public byte[] Data { get; set; }

    }
}
