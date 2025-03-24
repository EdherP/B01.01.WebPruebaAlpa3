"use strict";
var KTDatatablesBasicScrollable = function () {

    var initTable2 = function () {
        var table = $('#kt_table_BGral');
        // begin second table
        table.DataTable({
            scrollY: '60vh',
            scrollCollapse: true,
            scrollX: true,
            paging: false,
            responsive: true,
         
            sortable: false,

            filterable: false,

            pagination: false,
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