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
   public class WBS_Project : BaseEditedEntity
    {
        [Display(Name = "Код проекта")]
        [StringLength(100)]
        public string ProjectCode { get; set; }

        [Display(Name = "Наименование проекта")]
        public string ProjectName { get; set; }

        [Display(Name = "Короткое наименование для 1С")]
        public string ProjectShortName { get; set; }

        public virtual List<WBS_ProjectItem> ProjectItems { get; set; }

    }
}
