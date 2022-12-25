using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS.ERP1C.WBS
{
    /*
    {
        "ИД": "138432e7-3c72-11e6-80da-0050568b0057",
        "Код": "317_1108",
        "Наименование": "Общежития СПбГУ_Генеральный план и транспорт",
        "ИДРодителя": "2910cf91-f0be-11e5-80d9-0050568b0057",
        "КодРодителя": "317",
        "НаименованиеРодителя": "Общежития CПбГУ",
        "НаименованиеПолное": "Общежития СПбГУ_Генеральный план и транспорт",
        "НаименованиеСокращенное": "",
        "КодИСР": "",
        "ПометкаУдаления": false
    },
    */

    public class ERP1C_WBSItem
    {
        [JsonProperty(PropertyName = "ИД")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "Код")]
        public string Code { get; set; }
        [JsonProperty(PropertyName = "Наименование")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "ИДРодителя")]
        public string ParentId { get; set; }
        [JsonProperty(PropertyName = "КодРодителя")]
        public string ParentCode { get; set; }
        [JsonProperty(PropertyName = "НаименованиеРодителя")]
        public string ParentName { get; set; }
        [JsonProperty(PropertyName = "НаименованиеПолное")]
        public string FullName { get; set; }
        [JsonProperty(PropertyName = "НаименованиеСокращенное")]
        public string ShortName { get; set; }
        [JsonProperty(PropertyName = "КодИСР")]
        public string WBSCode { get; set; }
        [JsonProperty(PropertyName = "ПометкаУдаления")]
        public bool IsDeleted { get; set; }
    }
}
