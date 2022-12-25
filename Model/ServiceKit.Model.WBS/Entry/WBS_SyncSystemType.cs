using System.ComponentModel.DataAnnotations;

namespace ServiceKit.Model.WBS.Entry
{
    public enum WBS_SyncSystemType
    {
        [Display(Name = "Не определено")]
        None = 0,

        [Display(Name = "Первичная загрузка из 1С")]
        Import1C = 1,

        [Display(Name = "Экспорт в 1С")]
        Export1C = 2,
        [Display(Name = "Экспорт в PayDox")]
        ExportPayDox = 3
    }
}