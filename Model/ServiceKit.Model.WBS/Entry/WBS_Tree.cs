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
    //[Table(nameof(WBS_Tree), Schema = Common.Schema)]
    public class WBS_Item : BaseEditedEntity
    {

        public Guid? ParentId { get; set; }

        [Display(Name = "Код")]
        public string Code { get; set; }

        [Display(Name = "Наименование")]
        public string Name { get; set; }

        [Display(Name = "Сокращенное наименование")]
        public string ShortName { get; set; }

        [Display(Name = "Полное наименование")]
        public string FullName { get; set; }

        [Display(Name = "Уровень")]
        public int Level { get; set; }

        [Display(Name = "Ид. внешней системы")]
        [StringLength(100)]
        public string ExtId { get; set; }

        public ICollection<WBS_TreeSyncSystem> SyncSystems { get; set; }
    }
}
