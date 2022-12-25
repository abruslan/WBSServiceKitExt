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
    public class WBS_SyncLog : BaseEditedEntity
    {
        [Display(Name = "Проект")]
        public WBS_Project Project { get; set; }

        [ForeignKey(nameof(Project))]
        public Guid? ProjectId { get; set; }

        [Display(Name = "Результат публикации")]
        public bool Success { get; set; }
        [Display(Name = "Описание")]
        public string Message { get; set; }

        [Display(Name = "ID запроса")]
        public Guid? RequestId { get; set; }

        public List<WBS_SyncLogItem> Items { get; set; }

    }
}
