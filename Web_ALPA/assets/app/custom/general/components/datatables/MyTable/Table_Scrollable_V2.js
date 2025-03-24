
"use strict";

var KTDatatablesBasicScrollable = function () {

    var initTable2 = function () {
        $("#kt_table_1x").dataTable().fnDestroy();
        var table = $('#kt_table_Scroller2');

        // begin second table
        table.DataTable({
            "searching": false,
            "info": false,
            scrollY: '60vh',
            scrollCollapse: true,
            scrollX: true,
            paging: true,
            responsive: false,
            sortable: true,
            filterable: false,
            pagination: true,
            "language": {
                "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Spanish.json"
            },
        });
    };
    return {
        //main function to initiate the module
        init: function () {
            initTable2();
        }
    };
}();

jQuery(document).ready(function () {
    KTDatatablesBasicScrollable.init();
});