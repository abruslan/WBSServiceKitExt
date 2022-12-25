using ServiceKit.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceKit.Model.WBS.Entry
{
    public class UserRole : BaseEditedEntity
    {
        [Display(Name = "Пользователь")]
        [StringLength(100)]
        public string UserName { get; set; }

        [Display(Name = "Роль")]
        [StringLength(100)]
        public string Role { get; set; }

        [Display(Name = "Система")]
        [StringLength(100)]
        public string System { get; set; }
    }
}
