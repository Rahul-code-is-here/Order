$(document).ready(function(){
    let pageSize = $("#taxPageSize").val();
    let pageNumber = 1;
    let searchFilter = $('#taxSearch').val();

    // pagination
    loadTaxes(pageSize, pageNumber, searchFilter)
    $(document).on('keyup', '#taxSearch', function () {
        searchFilter = $(this).val();
        pageNumber = 1;
        loadTaxes(pageSize, pageNumber, searchFilter)
    })
    $(document).on('change', '#taxPageSize', function () {
        pageSize = $(this).val();
        pageNumber = 1;
        console.log(pageSize);
        loadTaxes(pageSize, pageNumber, searchFilter)
    })
    $(document).on('click', '#taxPrevious', function () {
        pageNumber = pageNumber - 1;
        loadTaxes(pageSize, pageNumber, searchFilter)
    })
    $(document).on('click', '#taxNext', function () {
        pageNumber = pageNumber + 1;
        loadTaxes(pageSize, pageNumber, searchFilter)
    })

    $(document).on('click','#AddTaxBtn',function(e){
        e.preventDefault();
        console.log("Ready for get add text form")
        let AddTaxForm = $('#AddTaxForm');
        let AddTaxFormData = new FormData(AddTaxForm[0]);
        
        $.validator.unobtrusive.parse(AddTaxFormData);
        if (AddTaxForm.valid()) {
        
            AddTax(AddTaxFormData);
        }
    })
    //fetch tax for edit
    $(document).on("click",".editTaxes",function(){
        let taxId = $(this).attr("data-taxId")
        console.log(taxId)
        GetTaxForEdit(taxId)
    })
    // edit tax
    $(document).on('click','#EditTaxBtn',function(e){
        e.preventDefault();
        let editTaxForm = $('#EditTaxForm');
        let editTaxFormData = new FormData(editTaxForm[0]);
        $.validator.unobtrusive.parse(editTaxFormData);
        for (let pair of editTaxFormData.entries()) {
            console.log(pair[0] + ': ' + pair[1]);  
        }
        if (editTaxForm.valid()) {
           
            EditTax(editTaxFormData);
        }
    })
    // delete tax 
    let deleteTaxId ;
    $(document).on('click','.deleteTax',function(){
        deleteTaxId = $(this).attr("data-Delete-taxId");
        console.log(deleteTaxId)
    })
    $(document).on('click','#deleteTaxConfirmBtn',function(){
        console.log(deleteTaxId)
        DeleteTax(deleteTaxId);

    })

   
})

function loadTaxes(pageSize, pageNumber, searchFilter) {
    $.ajax({
        url: '/TaxAndFee/TaxList',
        type: 'GET',
        data: {
            PageSize: pageSize,
            CurrentPage: pageNumber,
            SearchFilter: searchFilter,
        },
        success: function (data) {
            $('#taxContainer').html(data)
        },
        error: function (xhr, status, error) {
            console.log("error occured", error)
            alert("something went wrong");
        }
    })
}
function AddTax(AddTaxFormData){
    $.ajax({
        url: '/TaxAndFee/AddTax',
        type: 'POST',
        data: AddTaxFormData,
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

function GetTaxForEdit(taxId){
    $.ajax({
        url:'/TaxAndFee/GetTaxForEdit',
        type:'GET',
        data:{
            taxId:taxId
        },
        success:function(data){
            $('#taxId').val(data.taxId)
            $('#EditTaxName').val(data.taxName)
            $('#EditTaxType').val(data.taxtype)
            $('#EditTaxValue').val(data.taxValue)
            $('#EditIsEnabled').prop("checked",data.isEnabled);
            $('#EditDefaultTax').prop("checked",data.defaultTax);
        },
        error: function (xhr, error) {
            let errResponse = JSON.parse(xhr.responseText);
            window.location.href = errResponse.redirectUrl;
            console.log("error:", error);
        }
    })
}
// for edit tax
function EditTax(editTaxFormData){
    $.ajax({
        url:'/TaxAndFee/EditTax',
        type:'POST',
        data:editTaxFormData,
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

// delete tax and fee
function DeleteTax(deleteTaxId){
    $.ajax({
        url:'/TaxAndFee/DeleteTax',
        type:'GET',
        data:{
            taxId:deleteTaxId
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