// let selectedExistingModifierItemForEdit = [];
// let selectedContainerForEditModifiergroup;
// let currentMode = "";
// $(document).ready(function () {
//   let selectedItem = [];

//   // for adding modifier group
//   $(document).on("click", "#addModifierGroupBtn", function (e) {
//     e.preventDefault();
//     let addModifierGroupForm = $("#addModifierGroupForm");
//     $.validator.unobtrusive.parse(addModifierGroupForm);
//     let addModifierGroupFormData = new FormData(addModifierGroupForm[0]);

//     selectedExistingModifierItemForEdit.forEach(function (item) {
//       addModifierGroupFormData.append(
//         "AddModifiers.ModifierItems[]",
//         item.itemId
//       );
//     });
//     for (let pair of addModifierGroupFormData.entries()) {
//       console.log(pair[0] + ": " + pair[1]);
//     }
//     if (addModifierGroupForm.valid()) {
     
//       addModifierGroup(addModifierGroupFormData);
//     }
//   });
//   // fetching modifierGroup for editing
//   $(document).on("click", ".edit-btn-modifier", function () {
//     let currentModifierGroupId = $(this).attr("data-id");
//     console.log(currentModifierGroupId);
//     GetModifierForEdit(currentModifierGroupId);
//   });
//   // for editing modifier group
//   $(document).on('click', "#editModifierGroupBtn", function (e) {
//     e.preventDefault();
//     let editModifierGroupForm = $('#editModifierGroupForm');
//     $.validator.unobtrusive.parse(editModifierGroupForm);
//     let editModifierGroupFormData = new FormData(editModifierGroupForm[0]);

//     selectedExistingModifierItemForEdit.forEach(function (item) {
//       editModifierGroupFormData.append(
//         "AddModifiers.ModifierItems[]",
//         item.itemId
//       );
//     });
//     for (let pair of editModifierGroupFormData.entries()) {
//       console.log(pair[0] + ": " + pair[1]);
//     }
//     if (editModifierGroupForm.valid()) {
     
//       editModifierGroup(editModifierGroupFormData);
//     }
//   })
//   // delete Modifier group
//   let modifierGroupIdForDelete;
//   $(document).on("click", ".delete-modifierGroup", function () {
//     modifierGroupIdForDelete = $(this).attr("data-id");
//     console.log("modifiergroupIds", modifierGroupIdForDelete);
//   });
// // delete
//   $(document).on("click", "#deleteUserBtn", function () {
//     console.log("modifiergroupIds", modifierGroupIdForDelete);
//     if (modifierGroupIdForDelete != null) {
//       DeleteModifierGroup(modifierGroupIdForDelete);
//     }
//   });
//   // for intailizing the container to show the badges of items
//   $(document).on('click','#addMGroup',function(){
//     currentMode = $(this).attr("data-mode");
//     selectedContainerForEditModifiergroup =
//     currentMode == "edit"
//       ? $("#existingModifiersForEdit")
//       : $("#existingModifiersForAdd");
//       console.log(selectedContainerForEditModifiergroup);
//       console.log(currentMode);
//   })
//   $(document).on('click','.edit-btn-modifier',function(){
//     currentMode = $(this).attr("data-mode");
//     selectedContainerForEditModifiergroup =
//     currentMode == "edit"
//       ? $("#existingModifiersForEdit")
//       : $("#existingModifiersForAdd");
//       console.log(selectedContainerForEditModifiergroup);
//       console.log(currentMode);
//   })
//   // for edit modifiergroup existing modifiers
//   $(document).on("click", "#edit-existingModifiers", function () {
//     currentMode = $(this).attr("data-mode");
//     selectedContainerForEditModifiergroup =
//     currentMode == "edit"
//       ? $("#existingModifiersForEdit")
//       : $("#existingModifiersForAdd");
//       console.log(selectedContainerForEditModifiergroup);
//       console.log(currentMode);
//   });
//   $(document).on("change", ".checkedIt", function () {
//     // selectedContainerForEditModifiergroup =
//     //   currentMode == "edit"
//     //     ? $("#existingModifiersForEdit")
//     //     : $("#existingModifiersForAdd");
//     let ModifierItemId = $(this).attr("data-mid");
//     let ModifierItemName = $(this).attr("data-modifier");

//     if ($(this).is(":checked")) {
//       selectedExistingModifierItemForEdit.push({
//         itemId: ModifierItemId,
//         itemName: ModifierItemName,
//       });
//       console.log(selectedExistingModifierItemForEdit);
//     } else {
//       selectedExistingModifierItemForEdit =
//         selectedExistingModifierItemForEdit.filter(
//           (item) => item.itemId != ModifierItemId
//         );

//       console.log(selectedExistingModifierItemForEdit);
//     }
//     console.log(selectedExistingModifierItemForEdit);
//   });
//   //   adding the badges
//   $(document).on("click", "#addExistingModifier", function () {
//     showModifierItems();
//   });
//   //removing the badges
//   $(document).on("click", ".removeSelected-modifierItem", function () {
//     console.log("removed called");
//     let itemvalue = $(this).attr("data-ModifierItemId");
//     console.log(itemvalue);
//     selectedExistingModifierItemForEdit =
//       selectedExistingModifierItemForEdit.filter(
//         (item) => item.itemId != itemvalue
//       );
//     showModifierItems();
//   });
//   $(document).on('click','.emptytheModal',function(){
//     selectedExistingModifierItemForEdit.length = 0;
//     currentMode="";
//     console.log(selectedExistingModifierItemForEdit)
//     console.log(currentMode)
//   })
//   // showbadges
//   function showModifierItems() {
//     debugger
//     selectedContainerForEditModifiergroup.empty();
//     selectedExistingModifierItemForEdit.forEach((item) => {
//       selectedContainerForEditModifiergroup.append(`
//                   <span class='selected-modifierItem badge border rounded-pill  text-primary px-2' data-ModifierItemId ='${item.itemId}'>
//                           ${item.itemName}
//                          <i class='fa-solid fa-xmark ms-1 removeSelected-modifierItem' data-ModifierItemId='${item.itemId}' style='color: #c2c2c2;'></i>
//                       </span>
//                       `);
//     });
//     console.log(selectedExistingModifierItemForEdit);
//     // location.reload();
//   }
//   // restoring checkboxes
//   function restoreCheckmark() {
//     let selectedIds = selectedExistingModifierItemForEdit.map(
//       (item) => item.itemId
//     );

//     $(".checkedIt").each(function () {
//       let checkbox = $(this);
//       let modifierId = checkbox.attr("data-mid");

//       checkbox.prop("checked", selectedIds.includes(modifierId)); 
//     });
//   }
//   // pagination of existing modifier
//   $("#SelectExistingModal").on("shown.bs.modal", function () {
//     let pageSize = $("#items").val();
//     let pageNumber = 1;
//     let searchFilter;
//     getAllModifiersItemList();

//     $(document).on("keyup", "#SelectExistingModifierSearch", function () {
//       searchFilter = $(this).val();
//       console.log(searchFilter);
//       pageNumber = 1;
//       getAllModifiersItemList();
//     });
//     $(document).on("change", "#ExistingModifierItemPageSize", function () {
//       pageSize = $(this).val();
//       pageNumber = 1;
//       getAllModifiersItemList();
//     });
//     $(document).on("click", "#ExistingModifierItemNext", function () {
//       pageNumber = pageNumber + 1;
//       getAllModifiersItemList();
//     });
//     $(document).on("click", "#ExistingModifierItemPrevious", function () {
//       pageNumber = pageNumber - 1;
//       getAllModifiersItemList();
//     });
//     function getAllModifiersItemList() {
//       $.ajax({
//         url: "/SuperAdmin/GetAllModifierItemList",
//         type: "GET",
//         data: {
//           PageSize: pageSize,
//           CurrentPage: pageNumber,
//           SearchFilter: searchFilter,
//         },
//         success: function (data) {
//           $("#SelectExistingModifiers").html(data);
//           restoreCheckmark();
//         },
//         error: function (xhr, status, error) {
//           console.log("error occured", error);
//           alert("something went wrong");
//         },
//       });
//     }
//   });
//   // select all for exisitng modifier modal
//   $(document).on("change", "#SelectAllModifiersinExisiting", function () {
//     // if ($(this).is(":checked")) {
//     //   $(".selectModifierinExisting").prop("checked", true);
//     // } else {
//     //   $(".selectModifierinExisting").prop("checked", false);
//     // }
//     if ($(this).is(":checked")) {
//       $(".selectModifierinExisting").prop("checked", true).trigger("change");
//   } else {
//       $(".selectModifierinExisting").prop("checked", false).trigger("change");
//   }
//   });
//   $(document).on("change", ".selectModifierinExisting", function () {
//     if ($(".selectModifierinExisting:checked").length === $(".selectModifierinExisting").length) {
//       $("#SelectAllModifiersinExisiting").prop("checked", true);
//     } else {
//       $("#SelectAllModifiersinExisiting").prop("checked", false);
//     }
//   });
// });
// // ajax call for adding modifier group
// function addModifierGroup(addModifierGroupFormData) {
//   $.ajax({
//     url: "/SuperAdmin/AddModifiers",
//     type: "POST",
//     data: addModifierGroupFormData,
//     processData: false,
//     contentType: false,
//     success: function (data) {
     
//       window.location.href = data.redirectUrl;
//     },
//     error: function (xhr, err) {
//       console.log(err);
//       var errResponse = JSON.parse(xhr.responseText);
//       window.location.href = errResponse.redirectUrl;
//     },
//   });
// }
// //ajax call for fetching modifierGroup for editing
// function GetModifierForEdit(currentModifierGroupId) {
//   console.log(currentModifierGroupId);
//   $.ajax({
//     url: "/SuperAdmin/GetModifierForEdit",
//     type: "Get",
//     data: { id: currentModifierGroupId },
//     success: function (data) {
//       $("#edit-selectedModifiers").val(data.modifierGroupId)
//       $("#editModifierGroupName").val(data.modifierGroupName);
//       $("#editModifierGroupDescription").val(data.description);
//       $("#existingModifiersForEdit").empty();
//       selectedExistingModifierItemForEdit.length = 0;
//       data.modifierItemsData.forEach((i) => {
//         selectedExistingModifierItemForEdit.push(i);
//       });
//       if (selectedExistingModifierItemForEdit != 0) {
//         selectedExistingModifierItemForEdit.forEach((i) => {
//           $("#existingModifiersForEdit").append(`
//                         <span class='selected-modifierItem badge border rounded-pill bg-white text-secondary px-2' data-ModifierItemId ='${i.itemId}'>
//                             ${i.itemName}
//                              <i class='fa-solid fa-xmark ms-1 removeSelected-modifierItem' data-ModifierItemId ='${i.itemId}'  style='color: #c2c2c2;'></i>
//                         </span>
//                         `);
//         });
//       }
//       console.log(selectedExistingModifierItemForEdit);
//     },
//     error: function (xhr, error) {
//       console.log(err);
//       var errResponse = JSON.parse(xhr.responseText);
//       window.location.href = errResponse.redirectUrl;
//     },
//   });
// }
// // ajax call for delete Modifier Group
// function DeleteModifierGroup(modifierGroupIdForDelete) {
//   $.ajax({
//     url: "/superAdmin/DeleteModifierGroup",
//     type: "GET",
//     data: {
//       id: modifierGroupIdForDelete,
//     },
//     success: function (data) {
//       window.location.href = data.redirectUrl;
//     },
//     error: function (xhr, error) {
//       console.log(err);
//       var errResponse = JSON.parse(xhr.responseText);
//       window.location.href = errResponse.redirectUrl;
//     },
//   });

// }
// // ajax call for editModifierGroup
// function editModifierGroup(editModifierGroupFormData) {
//   console.log("ajax fro edit called")
//   $.ajax({
//     url: "/SuperAdmin/EditModifierGroupPost",
//     type: "POST",
//     data: editModifierGroupFormData,
//     processData: false,
//     contentType: false,
//     success: function (data) {
      
//       window.location.href = data.redirectUrl;
//     },
//     error: function (xhr, err) {
     
//       console.log(err);
//       var errResponse = JSON.parse(xhr.responseText);
//       window.location.href = errResponse.redirectUrl;
//     },
//   });
// }


let selectedExistingModifierItemForEdit = [];
let selectedContainerForEditModifiergroup;
let currentMode = "";
$(document).ready(function () {
  let selectedItem = [];

  // for adding modifier group
  $(document).on("click", "#addModifierGroupBtn", function (e) {
    e.preventDefault();
    let addModifierGroupForm = $("#addModifierGroupForm");
    $.validator.unobtrusive.parse(addModifierGroupForm);
    let addModifierGroupFormData = new FormData(addModifierGroupForm[0]);

    selectedExistingModifierItemForEdit.forEach(function (item) {
      addModifierGroupFormData.append(
        "AddModifiers.ModifierItems[]",
        item.itemId
      );
    });
    for (let pair of addModifierGroupFormData.entries()) {
      console.log(pair[0] + ": " + pair[1]);
    }
    if (addModifierGroupForm.valid()) {
    
      addModifierGroup(addModifierGroupFormData);
    }
  });
  // fetching modifierGroup for editing
  $(document).on("click", ".edit-btn-modifier", function () {
    let currentModifierGroupId = $(this).attr("data-id");
    console.log(currentModifierGroupId);
    GetModifierForEdit(currentModifierGroupId);
  });
  // for editing modifier group
  $(document).on('click', "#editModifierGroupBtn", function (e) {
    e.preventDefault();
    let editModifierGroupForm = $('#editModifierGroupForm');
    $.validator.unobtrusive.parse(editModifierGroupForm);
    let editModifierGroupFormData = new FormData(editModifierGroupForm[0]);

    selectedExistingModifierItemForEdit.forEach(function (item) {
      editModifierGroupFormData.append(
        "AddModifiers.ModifierItems[]",
        item.itemId
      );
    });
    for (let pair of editModifierGroupFormData.entries()) {
      console.log(pair[0] + ": " + pair[1]);
    }
    if (editModifierGroupForm.valid()) {
  
      editModifierGroup(editModifierGroupFormData);
    }
  })
  // delete Modifier group
  let modifierGroupIdForDelete;
  $(document).on("click", ".delete-modifierGroup", function () {
    modifierGroupIdForDelete = $(this).attr("data-id");
    console.log("modifiergroupIds", modifierGroupIdForDelete);
  });
// delete
  $(document).on("click", "#deleteUserBtn", function () {
    console.log("modifiergroupIds", modifierGroupIdForDelete);
    if (modifierGroupIdForDelete != null) {
      DeleteModifierGroup(modifierGroupIdForDelete);
    }
  });
  // for intailizing the container to show the badges of items
  $(document).on('click','#addMGroup',function(){
    currentMode = $(this).attr("data-mode");
    selectedContainerForEditModifiergroup =
    currentMode == "edit"
      ? $("#existingModifiersForEdit")
      : $("#existingModifiersForAdd");
      console.log(selectedContainerForEditModifiergroup);
      console.log(currentMode);
  })
  $(document).on('click','.edit-btn-modifier',function(){
    currentMode = $(this).attr("data-mode");
    selectedContainerForEditModifiergroup =
    currentMode == "edit"
      ? $("#existingModifiersForEdit")
      : $("#existingModifiersForAdd");
      console.log(selectedContainerForEditModifiergroup);
      console.log(currentMode);
  })
  // for edit modifiergroup existing modifiers
  $(document).on("click", "#edit-existingModifiers", function () {
    currentMode = $(this).attr("data-mode");
    selectedContainerForEditModifiergroup =
    currentMode == "edit"
      ? $("#existingModifiersForEdit")
      : $("#existingModifiersForAdd");
      console.log(selectedContainerForEditModifiergroup);
      console.log(currentMode);
  });
  $(document).on("change", ".checkedIt", function () {
    // selectedContainerForEditModifiergroup =
    //   currentMode == "edit"
    //     ? $("#existingModifiersForEdit")
    //     : $("#existingModifiersForAdd");
    let ModifierItemId = $(this).attr("data-mid");
    let ModifierItemName = $(this).attr("data-modifier");

    if ($(this).is(":checked")) {
      selectedExistingModifierItemForEdit.push({
        itemId: ModifierItemId,
        itemName: ModifierItemName,
      });
      console.log(selectedExistingModifierItemForEdit);
    } else {
      selectedExistingModifierItemForEdit =
        selectedExistingModifierItemForEdit.filter(
          (item) => item.itemId != ModifierItemId
        );

      console.log(selectedExistingModifierItemForEdit);
    }
    console.log(selectedExistingModifierItemForEdit);
  });
  //   adding the badges
  $(document).on("click", "#addExistingModifier", function () {
    debugger
    showModifierItems();
    $('.SelectExistingModal').addClass('show').css('display','block');
  });
  //removing the badges
  $(document).on("click", ".removeSelected-modifierItem", function () {
    console.log("removed called");
    let itemvalue = $(this).attr("data-ModifierItemId");
    console.log(itemvalue);
    selectedExistingModifierItemForEdit =
      selectedExistingModifierItemForEdit.filter(
        (item) => item.itemId != itemvalue
      );
    showModifierItems();
  });
  $(document).on('click','.emptytheModal',function(){
    selectedExistingModifierItemForEdit.length = 0;
    currentMode="";
    console.log(selectedExistingModifierItemForEdit)
    console.log(currentMode)
  })
  // showbadges
  // function showModifierItems() {
  //   debugger
  //   selectedContainerForEditModifiergroup.empty();
  //   selectedExistingModifierItemForEdit.forEach((item) => {
  //     selectedContainerForEditModifiergroup.append(`
  //                 <span class='selected-modifierItem badge border rounded-pill bg-white text-secondary px-2' data-ModifierItemId ='${item.itemId}'>
  //                         ${item.itemName}
  //                        <i class='fa-solid fa-xmark ms-1 removeSelected-modifierItem' data-ModifierItemId='${item.itemId}' style='color: #c2c2c2;'></i>

  //                     </span>
  //                     `);
  //   });
  //   console.log(selectedExistingModifierItemForEdit);
  // }
  function showModifierItems() {
    debugger
    selectedContainerForEditModifiergroup.empty();
    selectedExistingModifierItemForEdit.forEach((item) => {
        selectedContainerForEditModifiergroup.append(` 
            <span class='selected-modifierItem badge border rounded-pill bg-white text-secondary px-2' data-ModifierItemId ='${item.itemId}'> 
                ${item.itemName} 
                <i class='fa-solid fa-xmark ms-1 removeSelected-modifierItem' data-ModifierItemId='${item.itemId}' style='color: #c2c2c2;'></i> 
            </span> 
        `);
    });
    console.log(selectedExistingModifierItemForEdit);
}

  // restoring checkboxes
  function restoreCheckmark() {
    let selectedIds = selectedExistingModifierItemForEdit.map(
      (item) => item.itemId
    );

    $(".checkedIt").each(function () {
      let checkbox = $(this);
      let modifierId = checkbox.attr("data-mid");

      checkbox.prop("checked", selectedIds.includes(modifierId));
    });
  }
  // pagination of existing modifier
  $("#SelectExistingModal").on("shown.bs.modal", function () {
    let pageSize = $("#items").val();
    let pageNumber = 1;
    let searchFilter;
    getAllModifiersItemList();

    $(document).on("keyup", "#SelectExistingModifierSearch", function () {
      searchFilter = $(this).val();
      console.log(searchFilter);
      pageNumber = 1;
      getAllModifiersItemList();
    });
    $(document).on("change", "#ExistingModifierItemPageSize", function () {
      pageSize = $(this).val();
      pageNumber = 1;
      getAllModifiersItemList();
    });
    $(document).on("click", "#ExistingModifierItemNext", function () {
      pageNumber = pageNumber + 1;
      getAllModifiersItemList();
    });
    $(document).on("click", "#ExistingModifierItemPrevious", function () {
      pageNumber = pageNumber - 1;
      getAllModifiersItemList();
    });
    function getAllModifiersItemList() {
      $.ajax({
        url: "/SuperAdmin/GetAllModifierItemList",
        type: "GET",
        data: {
          PageSize: pageSize,
          CurrentPage: pageNumber,
          SearchFilter: searchFilter,
        },
        success: function (data) {
          $("#SelectExistingModifiers").html(data);
          restoreCheckmark();
        },
        error: function (xhr, status, error) {
          console.log("error occured", error);
          alert("something went wrong");
        },
      });
    }
  });
  // select all for exisitng modifier modal
  $(document).on("change", "#SelectAllModifiersinExisiting", function () {
    // if ($(this).is(":checked")) {
    //   $(".selectModifierinExisting").prop("checked", true);
    // } else {
    //   $(".selectModifierinExisting").prop("checked", false);
    // }
    if ($(this).is(":checked")) {
      $(".selectModifierinExisting").prop("checked", true).trigger("change");
  } else {
      $(".selectModifierinExisting").prop("checked", false).trigger("change");
  }
  });
  $(document).on("change", ".selectModifierinExisting", function () {
    if ($(".selectModifierinExisting:checked").length === $(".selectModifierinExisting").length) {
      $("#SelectAllModifiersinExisiting").prop("checked", true);
    } else {
      $("#SelectAllModifiersinExisiting").prop("checked", false);
    }
  });
});
// ajax call for adding modifier group
function addModifierGroup(addModifierGroupFormData) {
  $.ajax({
    url: "/SuperAdmin/AddModifiers",
    type: "POST",
    data: addModifierGroupFormData,
    processData: false,
    contentType: false,
    success: function (data) {
    
      window.location.href = data.redirectUrl;
    },
    error: function (xhr, err) {
  
      console.log(err);
      var errResponse = JSON.parse(xhr.responseText);
      window.location.href = errResponse.redirectUrl;
    },
  });
}
//ajax call for fetching modifierGroup for editing
function GetModifierForEdit(currentModifierGroupId) {
  console.log(currentModifierGroupId);
  $.ajax({
    url: "/SuperAdmin/GetModifierForEdit",
    type: "Get",
    data: { id: currentModifierGroupId },
    success: function (data) {
      $("#edit-selectedModifiers").val(data.modifierGroupId)
      $("#editModifierGroupName").val(data.modifierGroupName);
      $("#editModifierGroupDescription").val(data.description);
      $("#existingModifiersForEdit").empty();
      selectedExistingModifierItemForEdit.length = 0;
      data.modifierItemsData.forEach((i) => {
        selectedExistingModifierItemForEdit.push(i);
      });
      if (selectedExistingModifierItemForEdit != 0) {
        selectedExistingModifierItemForEdit.forEach((i) => {
          $("#existingModifiersForEdit").append(`
                        <span class='selected-modifierItem badge border rounded-pill bg-white text-secondary px-2' data-ModifierItemId ='${i.itemId}'>
                            ${i.itemName}
                             <i class='fa-solid fa-xmark ms-1 removeSelected-modifierItem' data-ModifierItemId ='${i.itemId}'  style='color: #c2c2c2;'></i>
                        </span>
                        `);
        });
      }
      console.log(selectedExistingModifierItemForEdit);
    },
    error: function (xhr, error) {
      console.log(err);
      var errResponse = JSON.parse(xhr.responseText);
      window.location.href = errResponse.redirectUrl;
    },
  });
}
// ajax call for delete Modifier Group
function DeleteModifierGroup(modifierGroupIdForDelete) {
  $.ajax({
    url: "/SuperAdmin/DeleteModifierGroup",
    type: "GET",
    data: {
      id: modifierGroupIdForDelete,
    },
    success: function (data) {
      window.location.href = data.redirectUrl;
    },
    error: function (xhr, error) {
      console.log(err);
      var errResponse = JSON.parse(xhr.responseText);
      window.location.href = errResponse.redirectUrl;
    },
  });

}
// ajax call for editModifierGroup
function editModifierGroup(editModifierGroupFormData) {
  console.log("ajax fro edit called")
  $.ajax({
    url: "/SuperAdmin/EditModifierGroupPost",
    type: "POST",
    data: editModifierGroupFormData,
    processData: false,
    contentType: false,
    success: function (data) {
     
      window.location.href = data.redirectUrl;
    },
    error: function (xhr, err) {

      console.log(err);
      var errResponse = JSON.parse(xhr.responseText);
      window.location.href = errResponse.redirectUrl;
    },
  });
}