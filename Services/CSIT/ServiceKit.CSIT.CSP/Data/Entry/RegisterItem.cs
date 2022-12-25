using ServiceKit.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceKit.CSIT.CSP.Data.Entry
{
    /// <summary>
    /// Строки файла-реестра
    /// </summary>
    public class RegisterItem : BaseEntity
    {
        /* Данные из файла*/
        public Register Register { get; set; }
        [Display(Name = "Артикул")]
        public string Number { get; set; }
        [Display(Name = "Сервис")]
        public string ServiceName { get; set; }
        [Display(Name = "Сервис")]
        public ClientService ClientService { get; set; }
        [Display(Name = "Пользователь")]
        public Client Client { get; set; }
        [Display(Name = "Пользователь")]
        public string ClientName { get; set; }
        [Display(Name = "ИНН")]
        public string ClientINN { get; set; }
        [Display(Name = "Кол-во раб. мест")]
        public int WorkPlaceCount { get; set; }
        [Display(Name = "Начало периода")]
        public DateTime PeriodFrom { get; set; }
        [Display(Name = "Конец периода")]
        public DateTime PeriodTo { get; set; }
        [Display(Name = "Кол-во дней использования")]
        public int DayCount { get; set; }
        [Display(Name = "Тариф за отч. период")]
        public string RegisterRateText { get; set; }
        [Display(Name = "Тариф за отч. период")]
        public decimal RegisterRate { get; set; }
        /* /Данные из файла */

        // Отчетный период на основании введенных данных по услугам
        [Display(Name = "Отчетный период")]
        public PeriodType RegisterRatePeriod { get; set; }

        // расчетный пеиод
        [Display(Name = "Тариф за день")]
        public decimal ReportRate { get; set; }

        public decimal Total()
        {
            switch (RegisterRatePeriod)
            {
                case PeriodType.Month:
                    // если услуга за полный месяц, то берем сумму услуги
                    if (DayCount == DateTime.DaysInMonth(PeriodFrom.Year, PeriodFrom.Month))
                        return (ClientService?.Price??0) * WorkPlaceCount;
                    // иначе берем сумму услуги, рассчитанную на день, и умножаем на количество дней
                    else
                        return ReportRate * WorkPlaceCount * DayCount;

                case PeriodType.Year:
                    if (DayCount >= 365) // дополнительные дни в году не считаем
                        return (ClientService?.YearPrice??0) * WorkPlaceCount;
                    else
                        return ReportRate * WorkPlaceCount * DayCount;
            }
            return 0;
        }
    }
}
