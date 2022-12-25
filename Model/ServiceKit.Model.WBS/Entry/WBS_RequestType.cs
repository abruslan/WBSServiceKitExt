using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceKit.Model.WBS.Entry
{
    public enum WBS_RequestType
    {
        [Display(Name = "Не определено")]
        None = 0,
        [Display(Name = "Файл проекта")]
        Project = 1,
        [Display(Name = "Файл с изменениями")]
        Changes = 2
    }
}
