
var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    if ($.fn.DataTable.isDataTable('#tblData')) {
        $('#tblData').DataTable().destroy();
    }
    dataTable = $('#tblData').DataTable({
        "ajax": {
            url: '/admin/order/getall',
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


