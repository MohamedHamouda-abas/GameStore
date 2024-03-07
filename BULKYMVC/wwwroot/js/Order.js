var dataTable;
$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("inprocess")) {
        loadDataTable("inprocess");
    }
    else {
        if (url.includes("completed")) {
            loadDataTable("completed");
        }
        else {
            if (url.includes("pending")) {
                loadDataTable("pending");
            }
            else {
                if (url.includes("approved")) {
                    loadDataTable("approved");
                }
                else {
                    if (url.includes("approvedfordelayedpayment")) {
                        loadDataTable("approvedfordelayedpayment");
                    }
                    else {
                        loadDataTable("all");
                    }
                }
            }
        }
    }
});

function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/Admin/Order/GetAll?status=' + status },
        "columns": [
            { data: 'id', "width": "5%" },
            { data: 'name', "width": "15%" },
            { data: 'applicationUser.email', "width": "15%" },
            {
                data: 'applicationUser.streetAddress', "width": "10%",
                "render": function (data) {
                    const maxLength = 20; // Maximum number of characters for the shortened description
                    const shortDescription = data.length > maxLength ? data.substring(0, maxLength) + '...' : data;
                    return shortDescription;
                },
            },
            { data: 'phoneNumber', "width": "15%" },
            { data: 'orderStatus', "width": "10%" },
            { data: 'orderTotal', "width": "10%" },
            {
                data: 'id',

                "render": function (data) {
                    return `  <div class ="w-75 btn-group" role="group">
                    <a href="/admin/Order/details?orderId=${data}" class="btn btn-primary mx-2">
                        <i class="bi bi-pencil-square"></i> Edit
                    </div>`
                },
                "width": "20%"
            }

        ]
    });
}