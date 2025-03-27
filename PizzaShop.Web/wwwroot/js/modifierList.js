$(document).ready(function () {
    let pageSize = $("#items").val();
    let pageNumber = 1;
    let modifierGroupId;
    let searchFilter = $('#searchFun').val();

    getCurrrentModifierId()
    function getCurrrentModifierId() {
        modifierGroupId = $('.currentModifierId').first().attr('data-id');
    }
    $(document).on('click', '#nav-modifiers-tab', function () {
        loadModifiers();
    })
    $(document).on('click', '.currentModifierId', function () {
        modifierGroupId = $(this).attr('data-id');
        loadModifiers();
    })
    $(document).on('keyup', '#searchFun', function () {
        searchFilter = $(this).val();
        pageNumber = 1;
        loadModifiers()
    })
    $(document).on('change', '#PageSize', function () {
        pageSize = $(this).val();
        pageNumber = 1;
        console.log(pageSize);
        loadModifiers();
    })
    $(document).on('click', '#ModifierItemNext', function () {
        pageNumber = pageNumber + 1;
        loadModifiers()
    })
    $(document).on('click', '#ModifierItemPrevious', function () {
        pageNumber = pageNumber - 1;
        loadModifiers()
    })
    function loadModifiers() {
        $.ajax({
            url: '/SuperAdmin/GetModifierItemsList',
            type: 'GET',
            data: {
                PageSize: pageSize,
                CurrentPage: pageNumber,
                SearchFilter: searchFilter,
                ModifierGroupId: modifierGroupId
            },
            success: function (data) {
                $('#modifiersItemsContainer').html(data)
            },
            error: function (xhr, status, error) {
                console.log("error occured", error)
                alert("something went wrong");
            }
        })
    }
    // for selecting existing modifier
    // $('#SelectExistingModal').on('shown.bs.modal', function () {
    //     getAllModifiersItemList()

    //     $(document).on('keyup', '#SelectExistingModifierSearch', function () {
    //         searchFilter = $(this).val();
    //         pageNumber = 1;
    //         getAllModifiersItemList();
    //     });
    //     $(document).on('change','#ExistingModifierItemPageSize', function () {
    //         pageSize = $(this).val();
    //         pageNumber = 1;
    //         getAllModifiersItemList();
    //     });
    //     $(document).on('click','#ExistingModifierItemNext', function () {
    //         pageNumber = pageNumber + 1;
    //         getAllModifiersItemList();
    //     });
    //     $(document).on('click','#ExistingModifierItemPrevious', function () {
    //         pageNumber = pageNumber - 1;
    //         getAllModifiersItemList();
    //     });
    //     function getAllModifiersItemList() {
    //         $.ajax({
    //             url: '/SuperAdmin/GetAllModifierItemList',
    //             type: 'GET',
    //             data: {
    //                 PageSize: pageSize,
    //                 CurrentPage: pageNumber,
    //                 SearchFilter: searchFilter,
    //             },
    //             success: function (data) {
    //                 $('#SelectExistingModifiers').html(data)
    //             },
    //             error: function (xhr, status, error) {
    //                 console.log("error occured", error)
    //                 alert("something went wrong");
    //             }
    //         })
    //     }
    // });
    // for adding existing modifiers at the time of adding modifier group
   
})
