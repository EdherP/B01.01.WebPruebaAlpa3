﻿"use strict";
var KTDatatablesBasicBasic = function () {

    var initTable1 = function () {
        var table = $('#kt_tableBase');

        // begin first table
        table.DataTable({
            responsive: false,

            // DOM Layout settings
            dom: `<'row'<'col-sm-12'tr>>
			<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7 dataTables_pager'lp>>`,

            lengthMenu: [5, 10, 25, 50],

            pageLength: 10,

            language: {
                'lengthMenu': 'Display _MENU_',
            },

            // Order settings
            order: [[1, 'asc']],

            columnDefs: [

                {
                    targets: -1,
                    title: 'Acción',
                    orderable: false,
                    render: function (data, type, full, meta) {
                        return `
                        <span class="dropdown">
                            <a href="#" class="btn btn-sm btn-clean btn-icon btn-icon-md" data-toggle="dropdown" aria-expanded="true">
                              <i class="la la-ellipsis-h"></i>
                            </a>
                            <div class="dropdown-menu dropdown-menu-right">
                                <a class="dropdown-item" href="#"><i class="la la-edit"></i> Edit Details</a>
                                <a class="dropdown-item" href="#"><i class="la la-leaf"></i> Update Status</a>
                                <a class="dropdown-item" href="#"><i class="la la-print"></i> Generate Report</a>
                            </div>
                        </span>
                        <a href="#" class="btn btn-sm btn-clean btn-icon btn-icon-md" title="View">
                          <i class="la la-edit"></i>
                        </a>`;
                    },
                },
               
              
            ],
        });

        table.on('change', '.kt-group-checkable', function () {
            var set = $(this).closest('table').find('td:first-child .kt-checkable');
            var checked = $(this).is(':checked');

            $(set).each(function () {
                if (checked) {
                    $(this).prop('checked', true);
                    $(this).closest('tr').addClass('active');
                }
                else {
                    $(this).prop('checked', false);
                    $(this).closest('tr').removeClass('active');
                }
            });
        });

        table.on('change', 'tbody tr .kt-checkbox', function () {
            $(this).parents('tr').toggleClass('active');
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
    KTDatatablesBasicBasic.init();
});