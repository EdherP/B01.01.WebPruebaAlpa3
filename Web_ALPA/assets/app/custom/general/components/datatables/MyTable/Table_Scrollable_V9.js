
"use strict";
var KTDatatablesBasicScrollable = function () {

	var initTable1 = function () {
		$("#kt_table_Scroller1").dataTable().fnDestroy();
		var table = $('#kt_table_Scroller1');
		// begin second table
		table.DataTable({			 
			"info": false,
			scrollY: '60vh',
			scrollCollapse: true,
			scrollX: true,
			paging: true,
			responsive: false,
			sortable: true,
			filterable: false,
			pagination: true,
			searching:true,
			"language": {
				"url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Spanish.json"
			},
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
		});
	};

	return {

		//main function to initiate the module
		init: function () {
			initTable1();
		}
	};
}();

jQuery(document).ready(function () {
	KTDatatablesBasicScrollable.init();
});