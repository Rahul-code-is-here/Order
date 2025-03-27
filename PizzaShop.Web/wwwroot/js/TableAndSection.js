  
$(document).ready(function () {
    let pageSize = $("#tablePageSize").val();
    let pageNumber = 1;
    let sectionId = getCurrentSectionId()
    let searchFilter = $('#tableSearch').val();

    // pagination
    loadTables(pageSize, pageNumber, searchFilter, sectionId)
    $(document).on('click', '.currentSectionId', function () {
        sectionId = $(this).attr('data-id');
        loadTables(pageSize, pageNumber, searchFilter, sectionId)
    })
    $(document).on('keyup', '#tableSearch', function () {
        searchFilter = $(this).val();
        pageNumber = 1;
        loadTables(pageSize, pageNumber, searchFilter, sectionId)
    })
    $(document).on('change', '#tablePageSize', function () {
        pageSize = $(this).val();
        pageNumber = 1;
        console.log(pageSize);
        loadTables(pageSize, pageNumber, searchFilter, sectionId)
    })
    $(document).on('click', '#tablePrevious', function () {
        pageNumber = pageNumber - 1;
        loadTables(pageSize, pageNumber, searchFilter, sectionId)
    })
    $(document).on('click', '#tableNext', function () {
        pageNumber = pageNumber + 1;
        loadTables(pageSize, pageNumber, searchFilter, sectionId)
    })

    $('#AddSection').on('hidden.bs.modal', function () {
        // Clear all input fields
        $(this).find('input[type="text"], textarea').val('');
        // Clear hidden input fields
        $(this).find('input[type="hidden"]').val('');
    });

    $('#EditSection').on('hidden.bs.modal', function () {
        // Clear all input fields
        $(this).find('input[type="text"], textarea').val('');
        // Clear hidden input fields
        $(this).find('input[type="hidden"]').val('');
    });

    $('#AddTable').on('hidden.bs.modal', function () {
        // Clear all input fields
        $(this).find('input[type="text"], textarea').val('');
        // Clear hidden input fields
        $(this).find('input[type="hidden"]').val('');
    });

    $('#EditTable').on('hidden.bs.modal', function () {
        // Clear all input fields
        $(this).find('input[type="text"], textarea').val('');
        // Clear hidden input fields
        $(this).find('input[type="hidden"]').val('');
    });

    // for Add Section
    $(document).on('click', '#AddSectionBtn', function (e) {
        e.preventDefault();
        let AddSectionForm = $('#AddSectionForm');
        let AddSectionFormData = new FormData(AddSectionForm[0]);
        
        $.validator.unobtrusive.parse(AddSectionFormData);
        if (AddSectionForm.valid()) {
            AddSection(AddSectionFormData);
        }
    })

    // For fetching the section for edit
    $(document).on("click",".edit-btn-section",function(){
        let sectionId = $(this).attr("data-id")
        FetchSectionForEdit(sectionId)
    })

    

    // for edit section 
    $(document).on("click",'#EditSectionBtn',function(e){
        e.preventDefault();
        let editSectionForm = $('#EditSectionForm')
        let editSectionFormData = new FormData(editSectionForm[0]);
        for (let pair of editSectionFormData.entries()) {
            console.log(pair[0] + ': ' + pair[1]);
        }
        $.validator.unobtrusive.parse(editSectionFormData);
        if (editSectionForm.valid()) {
            EditSection(editSectionFormData);
        }

    })
    //delete Section
    let currentSectionIdForDelete;
    $(document).on('click','.delete-Section',function(){
        currentSectionIdForDelete = $(this).attr('data-id');
        console.log(currentSectionIdForDelete)
    })
    $(document).on('click','#deleteSectionConfirmBtn',function(){
        Delete(currentSectionIdForDelete);
        console.log(currentSectionIdForDelete)

    })

    // Add Table
    $(document).on('click','#AddTableBtn',function(e){
        e.preventDefault();
        let AddTableForm = $('#AddTableForm');
        let AddTableFormData = new FormData(AddTableForm[0]);
        $.validator.unobtrusive.parse(AddTableFormData);
        if (AddTableForm.valid()) {
            AddTable(AddTableFormData);
        }

    })

    // fetch table for edit
    $(document).on('click','.editTables',function(){
        let tableId = $(this).attr("data-tableId");
        let sectionId = $(this).attr("data-tableId");
        GetTableForEdit(tableId);
    })
    //edit Table
    $(document).on('click','#EditTableBtn',function(e){
        e.preventDefault();
        let editTableForm = $('#EditTableForm');
        let editTableFormData = new FormData(editTableForm[0]);
        $.validator.unobtrusive.parse(editTableFormData);
        if (editTableForm.valid()) {
          
            EditTable(editTableFormData);
        }
    })
    // mass  delete and delete
    $(document).on("change", "#selectAllTables", function () {
        if ($(this).is(":checked")) {
          $(".selectTable").prop("checked", true);
        } else {
          $(".selectTable").prop("checked", false);
        }
      });
      $(document).on("change", ".selectTable", function () {
        if ($(".selectTable:checked").length === $(".selectTable").length) {
          $("#selectAllTables").prop("checked", true);
        } else {
          $("#selectAllTables").prop("checked", false);
        }
      });
      let tableIds = [];
      $(document).on("click", "#deleteSelectedTables", function () {
        tableIds.length = 0; //emptying the array each time
        $(".selectTable:checked").each(function () {
            tableIds.push($(this).attr("data-tableId"));
        });
        console.log(tableIds);
        if (tableIds.length > 0) {
          $("#TableDelete").modal("show");
        } else {
          toastr.error("No items selected");
        }
      });
      $(document).on("click", ".deleteTable", function () {
        tableIds.length = 0; //emptying the array each time
        let itemId = $(this).attr("data-Delete-tableId");
        console.log(itemId)
        tableIds.push(itemId);
      });
      // for confirm delete
      $(document).on("click", "#deleteTableConfirmBtn", function () {
        if (tableIds.length != 0) {
            DeleteTable(tableIds);
        }
      })
})
function loadTables(pageSize, pageNumber, searchFilter, sectionId) {
    $.ajax({
        url: '/TableAndSection/GetTablesListForSection',
        type: 'GET',
        data: {
            PageSize: pageSize,
            CurrentPage: pageNumber,
            SearchFilter: searchFilter,
            sectionId: sectionId
        },
        success: function (data) {
            $('#tableContainer').html(data)
        },
        error: function (xhr, status, error) {
            console.log("error occured", error)
            alert("something went wrong");
        }
    })
}
function getCurrentSectionId() {
    let sectionId = $('.currentSectionId').first().attr('data-id');
    console.log(sectionId)
    return sectionId;
}

// ajax call for add section
function AddSection(AddSectionFormData) {
    $.ajax({
        url: '/TableAndSection/AddSection',
        type: 'POST',
        data: AddSectionFormData,
        processData: false,
        contentType: false,
        success: function (data) {
            
            window.location.href = data.redirectUrl;

        },
        error: function (xhr, error) {
            let errResponse = JSON.parse(xhr.responseText);
            window.location.href = errResponse.redirectUrl;
            console.log("error:", error);
        }
    })
}

// ajax call for Fetching Section For Edit
function FetchSectionForEdit(sectionId){
    $.ajax({
        url:'/TableAndSection/GetSectionForEdit',
        type:'GET',
        data:{
            sectionId:sectionId
        },
        success:function(data){
            $('#SectionName').val(data.sectionName)
            $('#SectionDescription').val(data.description)
            $('#sectionId').val(data.sectionId)
        },
        error: function (xhr, error) {
            let errResponse = JSON.parse(xhr.responseText);
            window.location.href = errResponse.redirectUrl;
            console.log("error:", error);
        }
    })
}
//ajax call for edit section
function EditSection(editSectionFormData){
    $.ajax({
        url:'/TableAndSection/EditSection',
        type:'Post',
        data:editSectionFormData,
        processData:false,
        contentType:false,
        success:function(data){
          
            window.location.href = data.redirectUrl;
        },
        error:function(xhr,error){
            let errResponse = JSON.parse(xhr.responseText);
            window.location.href = errResponse.redirectUrl;
            console.log("error:", error);
        }
    })
}
// ajax call for delete Section
function Delete(currentSectionId){
    console.log(currentSectionId)
    $.ajax({
        url:'/TableAndSection/DeleteSection',
        type:'GET',
        data:{
            sectionId:currentSectionId
        },
        success:function(data){
            window.location.href = data.redirectUrl;
        },
        error:function(xhr,error){
            let errResponse = JSON.parse(xhr.responseText);
            window.location.href = errResponse.redirectUrl;
            console.log("error:", error);
        }
    })
}
// ajax call for Add Table
function AddTable(AddTableFormData){
    $.ajax({
        url:'/TableAndSection/AddTable',
        type:'Post',
        data:AddTableFormData,
        processData:false,
        contentType: false,
        success:function(data){
           
            window.location.href = data.redirectUrl;
        },
        error:function(xhr,error){
            let errResponse = JSON.parse(xhr.responseText);
            window.location.href = errResponse.redirectUrl;
            console.log("error:", error);
        }
    })
}

// ajax call for Fetching Table For Edit
function GetTableForEdit(tableId){
debugger
    var statusMap= {
        "Available":0,
        "Assigned":1,
        "Reserved":2,
        "Selected":3
    }
    $.ajax({
        url:'/TableAndSection/GetTableForEdit',
        type:'GET',
        data:{
            tableId:tableId
        },
        success:function(data){
            $('#editTableId').val(data.tableId)
            $('#editTableName').val(data.tableName)
            $('#editsectionId').val(data.sectionId)
            $('#editTableCapacity').val(data.capacity)
            // $('#editStatus').val(statusMap[data.newtatus])
            $('#editStatus option:contains('+data.newstatus+')').prop('selected',true);
        },
        error: function (xhr, error) {
            let errResponse = JSON.parse(xhr.responseText);
            window.location.href = errResponse.redirectUrl;
            console.log("error:", error);
        }
    })
}

// ajax call forn Edit Table
function EditTable(editTableFormData){
    $.ajax({
        url:'/TableAndSection/EditTable',
        type:'POST',
        data:editTableFormData,
        processData:false,
        contentType: false,
        success:function(data){
           
            window.location.href = data.redirectUrl;
        },
        error:function(xhr,error){
            let errResponse = JSON.parse(xhr.responseText);
            window.location.href = errResponse.redirectUrl;
            console.log("error:", error);
        }
    })
}

function DeleteTable(tableIds){
    $.ajax({
        url: "/TableAndSection/DeleteTable",
        type: "POST",
        data: {
            tableIds: tableIds,
        },
        success: function (data) {
          window.location.href = data.redirectUrl;
        },
        error: function (xhr, error) {
          var errResponse = JSON.parse(xhr.responseText);
          window.location.href = errResponse.redirectUrl;
          console.log("error:", error);
        },
      });
}