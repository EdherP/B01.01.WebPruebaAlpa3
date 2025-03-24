"use strict";
var KTDatatablesBasicBasic = function () {

    var initTable2 = function () {
        var table = $('#kt_table_ScrollerDet');

        // begin second table
        table.DataTable({
           
            scrollCollapse: true,
            scrollX: true,
            paging: false,
            responsive: true,   
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
    KTDatatablesBasicBasic.init();
});