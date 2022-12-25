using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceKit.Model.WBS.Entry
{
    public enum WBS_RequestStatus
    {
        [Display(Name = "Новая")]
        New = 0,
        [Display(Name = "Проверена")]
        Checked = 1,
        [Display(Name = "Согласована")]
        Approved = 2,
        [Display(Name = "Загружена")]
        Completed = 3,

        [Display(Name = "Отклонена")]
        Canceled = 10,
    }
}
