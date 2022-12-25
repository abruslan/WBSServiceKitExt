using ServiceKit.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceKit.Model.WBS.Entry
{
    public class WBS_SyncSystem : BaseEditedEntity
    {
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Тип")]
        public WBS_SyncSystemType SystemType { get; set; }
        [Display(Name = "Строка соединения")]
        public string ConnectionString { get; set; }
        [Display(Name = "Логин")]
        public bool TrustedConnection { get; set; }
        [Display(Name = "Логин")]
        public string Login { get; set; }
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}
