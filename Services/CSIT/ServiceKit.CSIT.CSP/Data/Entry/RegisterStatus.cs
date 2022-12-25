using System.ComponentModel.DataAnnotations;

namespace ServiceKit.CSIT.CSP.Data.Entry
{
    public enum RegisterStatus
    {
        [Display(Name = "Не определено")]
        None,
        [Display(Name="Загружен")]
        Imported = 1,
        [Display(Name = "На проверке")]
        Checking = 2,
        [Display(Name = "Проверен")]
        Checked = 3,
        [Display(Name = "Закрыт")]
        Closed = 4,
        [Display(Name = "В архиве")]
        Archived = 5
    }
}