/**
 * 
 * Работа с таблицами js DataTables
 * https://datatables.net/
 *
 */


/* DATA TABLES */
function init_DataTables() {
	var $datatableElement = $(".datatable");
	$datatableElement.DataTable({
		"stateSave": true,
		"lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "Все"]],
		"ordering": $($datatableElement).hasClass("sortable"),
		//"ordering": true,
		"language": {
			"search": "Поиск: ",
			"info": "Страница _PAGE_ из _PAGES_",
			"infoEmpty": "",
			"zeroRecords": "Нет записей для отображения",
			"infoFiltered": " (из _MAX_ записей)",
			"lengthMenu": "Показывать _MENU_ записей",
			"paginate": {
				"previous": "Назад",
				"next": "Вперед"
			}
		},
		//__dom: '<"top"f>rt<"bottom"Blp><"clear">',
		//dom: "lfrtipB",
		dom: "<'row'<'col-sm-12 col-md-6'l><'col-sm-12 col-md-6'f>><'row'<'col-sm-12'tr>><'row'<'col-sm-12 col-md-3 exportbuttons'B><'col-sm-12 col-md-3'i><'col-sm-12 col-lg-6'p>>",
		buttons: [
			//{
			//	extend: "copy",
			//	className: "btn-sm btn-outline-secondary",
			//	text: "Копировать"
			//},
			//{
			//	extend: "csv",
			//	className: "btn-sm"
			//},
			{
				extend: "excel",
				className: "mt-1",
				text: "Экспорт в Excel"
			},
			//{
			//	extend: "pdfHtml5",
			//	className: "btn-sm"
			//},
			//	{
			//		extend: "print",
			//		className: "btn-sm",
			//		text: "Печать"
			//	},
		]
	});

	// фикс "дерганья" таблицы при первичном рендеринге DataTable
	// изначально таблица обернута в скрытый div
	var $parent = $datatableElement.parent().parent();
	$parent.show();
}

$(document).ready(function () {
	init_DataTables();
	//	table.buttons().container()
	//		.appendTo($('.col-sm-6:eq(0)', table.table().container()));
});
