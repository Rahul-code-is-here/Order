$(document).ready(function () {
    let pageSize = $("#orders").val();
    let pageNumber = 1;
    let searchFilter = $('#OrdersearchFun').val();
    let sortColumn = "Orderid"
    let sortOrder = "asc";
    loadOrdersList(pageSize, pageNumber, searchFilter)
    $(document).on('keyup', '#OrdersearchFun', function () {
        searchFilter = $(this).val();
        pageNumber = 1;
        loadOrdersList(pageSize, pageNumber, searchFilter, sortColumn, sortOrder)
    })
    $(document).on('click', '.Orderid', function () {
        sortColumn = "Orderid"
        sortOrder = (sortOrder === "asc") ? "dsc" : "asc";
        console.log(sortColumn, sortOrder);
        loadOrdersList(pageSize, pageNumber, searchFilter, sortColumn, sortOrder)
    });
    $(document).on('click', '.Orderdate', function () {
        sortColumn = "Orderdate"
        sortOrder = (sortOrder === "asc") ? "dsc" : "asc";
        console.log(sortColumn, sortOrder);
        loadOrdersList(pageSize, pageNumber, searchFilter, sortColumn, sortOrder)
    });
    $(document).on('click', '.customerName', function () {
        sortColumn = "customerName"
        sortOrder = (sortOrder === "asc") ? "dsc" : "asc";
        console.log(sortColumn, sortOrder);
        loadOrdersList(pageSize, pageNumber, searchFilter, sortColumn, sortOrder)
    });
    $(document).on('click', '.totalAmount', function () {
        sortColumn = "totalAmount"
        sortOrder = (sortOrder === "asc") ? "dsc" : "asc";
        console.log(sortColumn, sortOrder);
        loadOrdersList(pageSize, pageNumber, searchFilter, sortColumn, sortOrder)
    });
    $(document).on('change', '#PageSize', function () {
        pageSize = $(this).val();
        pageNumber = 1;
        console.log(pageSize);
        loadOrdersList(pageSize, pageNumber, searchFilter, sortColumn, sortOrder)
    })
    $(document).on('click', '#OrdersNext', function () {
        pageNumber = pageNumber + 1;
        loadOrdersList(pageSize, pageNumber, searchFilter, sortColumn, sortOrder)
    })
    $(document).on('click', '#OrdersPrevious', function () {
        pageNumber = pageNumber - 1;
        loadOrdersList(pageSize, pageNumber, searchFilter, sortColumn, sortOrder)
    })
    // SearchButton
    $(document).on('click', '#searchOrderBtn', function (e) {
        e.preventDefault();
        let searchFilter = $('#OrdersearchFun').val();
        let orderStatusId = $('#orderStatusId').val();
        let allTimeSearchFilter = $('#allTimeSearchFilter').val();
        let fromDate = $('#fromdateOrders').val();
        let toDate = $('#toDateOrders').val();

        console.log(fromDate);
        console.log(allTimeSearchFilter)
        if (!fromDate && !toDate && allTimeSearchFilter != "0") {

            toDate = new Date().toISOString().slice(0, 10);

            fromDate = new Date();

            if (allTimeSearchFilter == "1") {
                fromDate.setDate(fromDate.getDate() - 7);
                fromDate = fromDate.toISOString().slice(0, 10);
            }
            else if (allTimeSearchFilter == "2") {
                fromDate.setDate(fromDate.getDate() - 30);
                fromDate = fromDate.toISOString().slice(0, 10);
            }
            else if (allTimeSearchFilter == "3") {
                let today = new Date();
                fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
                fromDate = fromDate.getFullYear() + "-" +
                    String(fromDate.getMonth() + 1).padStart(2, '0') + "-" +
                    String(fromDate.getDate()).padStart(2, '0');
            }
        }
        loadOrdersList(pageSize, pageNumber, searchFilter, sortColumn, sortOrder, fromDate, toDate, orderStatusId)
    })

    // exportbutton
    $(document).on('click', '#exportToExcel', function () {
        let searchValue = $('#OrdersearchFun').val();
        let allStatus = $('#orderStatusId').val();
        let allTimeSearchFilter = $('#allTimeSearchFilter').val();
        let allTimeSearchFilterText = $('#allTimeSearchFilter option:selected').text();
        let fromDate;
        if (allTimeSearchFilter != "0") {

            fromDate = new Date();

            if (allTimeSearchFilter == "1") {
                fromDate.setDate(fromDate.getDate() - 7);
                fromDate = fromDate.toISOString().slice(0, 10);
            }
            else if (allTimeSearchFilter == "2") {
                fromDate.setDate(fromDate.getDate() - 30);
                fromDate = fromDate.toISOString().slice(0, 10);
            }
            else if (allTimeSearchFilter == "3") {
                let today = new Date();
                fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
                fromDate = fromDate.getFullYear() + "-" +
                    String(fromDate.getMonth() + 1).padStart(2, '0') + "-" +
                    String(fromDate.getDate()).padStart(2, '0');
            }
        }
        exportToExcel(searchValue, allStatus, fromDate, allTimeSearchFilterText)
    })
})
function loadOrdersList(pageSize, pageNumber, searchFilter, sortColumn, sortOrder, fromDate, toDate, orderStatusId) {
    $.ajax({
        url: '/Orders/OrdersList',
        type: 'GET',
        data: {
            PageSize: pageSize,
            CurrentPage: pageNumber,
            SearchFilter: searchFilter,
            sortOrder: sortOrder,
            sortColumn: sortColumn,
            FromDate: fromDate,
            ToDate: toDate,
            OrderStatusId: orderStatusId
        },
        success: function (data) {
            $('#OrdersList').html(data)
        },
        error: function (xhr, status, error) {
            console.log("error occured", error)
            alert("something went wrong");
        }
    })
}
function exportToExcel(searchValue, allStatus, fromDate, allTimeSearchFilterText) {
    $.ajax({
        url: '/Orders/ExportToExcel',
        type: 'POST',
        xhrFields: {
            responseType: "blob" // Expect a binary file (blob) as the response
        },
        data: {
            searchFilter: searchValue,
            fromDate: fromDate,
            statusFilter: allStatus,
            dateFilter: allTimeSearchFilterText,
        },
        success: function (data) {

            console.log(data);
            var blob = new Blob([data], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
            var link = document.createElement("a");
            link.href = window.URL.createObjectURL(blob);
            link.download = "Orders.xlsx";
            document.body.appendChild(link)
            link.click();
            document.body.removeChild(link)
            toastr.success("excelsheet Downloaded successfully");
        },
        error: function (xhr, error) {

            console.log("error:", error);
        }
    });

}