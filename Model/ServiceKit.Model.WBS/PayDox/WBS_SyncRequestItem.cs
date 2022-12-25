using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceKit.Model.WBS.PayDox
{
	/// <summary>
	/// PayDox Table WBS_SyncRequestItems
	/// </summary>
	public class WBS_SyncRequestItem
    {
		[Key]
		public Guid Id { get; set; }
		public Guid? ParentId { get; set; }

		[Display(Name = "Уровень")]
		public int Level { get; set; }
		[Display(Name = "Код")]
		public string ShortCode { get; set; }
		[Display(Name = "Полный код")]
		public string FullCode { get; set; }

		[Display(Name = "Короткое наименование")]
		public string ShortName { get; set; }
		[Display(Name = "Полное наименование")]
		public string FullName { get; set; }

		[Display(Name = "Комментарий")]
		public string Comment { get; set; }

		[Display(Name = "Статус")]
		public int Status { get; set; }
		[Display(Name = "Сообщение об ошибке")]
		public string ErrorMessage { get; set; }
		[Display(Name = "Дата/время создания")]
		public DateTime Created { get; set; }
	}
}
