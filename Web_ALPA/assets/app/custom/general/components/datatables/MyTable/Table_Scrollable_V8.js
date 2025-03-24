"use strict";
var KTDatatablesBasicScrollable = function () {

	var initTable1 = function () {
		var table = $('#kt_table_1x');
		table.DataTable({
			scrollY: '60vh',
			scrollX: '120vh',
			scrollCollapse: true,
			scrollX: true,
			paging: false,
			responsive: false,
			// Pagination settings
			dom: `<'row'<'col-sm-6 text-left'f><'col-sm-6 text-right'B>>
			<'row'<'col-sm-12'tr>>
			<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7 dataTables_pager'lp>>`,

			buttons: [
				//'print',
				//'copyHtml5',
				//'excelHtml5',
				//'csvHtml5',
				//'pdfHtml5',
			],
			sortable: true,
			"language": {
				"url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Spanish.json"
			},
			filterable: true,

			pagination: false,
		});
	};

	var initTable2 = function () {
		var table = $('#kt_table_2x');
		table.DataTable({
			scrollY: '60vh',
			scrollCollapse: true,
			scrollX: true,
			paging: false,
			responsive: false,
			// Pagination settings
			dom: `<'row'<'col-sm-6 text-left'f><'col-sm-6 text-right'B>>
			<'row'<'col-sm-12'tr>>
			<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7 dataTables_pager'lp>>`,

			buttons: [
				//'print',
				'copyHtml5',
				'excelHtml5',
				//'csvHtml5',
				//'pdfHtml5',
			],
			sortable: true,

			filterable: false,

			pagination: false,
		});
	};

	return {

		//main function to initiate the module
		init: function () {
			initTable1();
			initTable2();
		}
	};
}();

jQuery(document).ready(function () {
	KTDatatablesBasicScrollable.init();
});