
var dataTable;
$(document).ready(function () {

    var url = window.location.search;

    if (url.includes("pending"))
    {
        loadDataTable("pending");
    }

    else {
        if (url.includes("inprocess"))
        {
            loadDataTable("inprocess");
        }

        else {
            if (url.includes("completed")) {
                loadDataTable("completed");
            }

            else {

                if (url.includes("approved")) {
                    loadDataTable("approved");
                }
                else {
                    loadDataTable("all");
                }
            }
        }
    }
});

function loadDataTable(status) {
    if ($.fn.DataTable.isDataTable('#tblData')) {
        $('#tblData').DataTable().destroy();
    }
    dataTable = $('#tblData').DataTable({
        "ajax": {
            url: '/admin/order/getall?status=' + status,
        },
        "columns": [
            { data: 'id', "width": "5%", "className": "text-center" },
            { data: 'name', "width": "20%", "className": "text-center" },
            { data: 'phoneNumber', "width": "15%", "className": "text-center" },
            { data: 'applicationUser.email', "width": "15%", "className": "text-center" },
            { data: 'orderStatus', "width": "10%", "className": "text-center" },
            { data: 'orderTotal', "width": "10%", "className": "text-center" },
            {
                data: 'id',
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <a href="/admin/order/details?orderId=${data}" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square"></i>Details
                            </a>
                        </div>
                    `;
                },
                "width": "10%", "className": "text-center"
            }
        ]
    });
}


