
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
            url: '/admin/user/getall', // URL to fetch data
        },
        "columns": [
            { data: 'name', "width": "15%", "className": "text-center" },
            { data: 'email', "width": "15%", "className": "text-center" },
            { data: 'phoneNumber', "width": "15%", "className": "text-center" },
            { data: 'companies.name', "width": "15%", "className": "text-center" },
            { data: 'role', "width": "15%", "className": "text-center" },
            {
                data: { id: "id", lockoutEnd: "lockoutEnd" },
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();
                    if (lockout > today) {
                        return `
                            <div class="text-center">
                              <a onClick=LockUnlock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer; width:100px">
                                    <i class="bi bi-lock-fill"></i> Lock
                                </a>
                               
                                <a href="/admin/user/RoleManagment?userId=${data.id}" class="btn btn-danger text-white" style="cursor:pointer; width:150px">
                                    <i class="bi bi-pencil-square"></i> Permission
                                </a>
                            </div>
                        `;
                    } else {
                        return `
                            <div class="text-center">
                               <a onClick=LockUnlock('${data.id}') class="btn btn-success text-white" style="cursor:pointer; width:100px">
                                    <i class="bi bi-unlock-fill"></i> Unlock
                                </a>
                                <a href="/admin/user/RoleManagment?userId=${data.id}" class="btn btn-danger text-white" style="cursor:pointer; width:150px">
                                    <i class="bi bi-pencil-square"></i> Permission
                                </a>
                            </div>
                        `;
                    }
                },
                "width": "25%"
            }
        ]
    });
}

function LockUnlock(id) {
    $.ajax({
        type: "POST",
        url: '/admin/user/LockUnlock',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            }

        }
    })
}

