let selectedModifierGroupForItemArray = [];
// $(document).ready(function () {
//   // for modifier group
//   $(document).on("change", ".ModifierGroup-checkbox", function () {
//     let selectedModifierGroupForItemContainer = $(
//       "#selectedModifierGroupForItem"
//     );
//     let ModifierGroupValue = $(this).val();
//     console.log(ModifierGroupValue);
//     var ModifierGroupText = $(this).next("label").text();
//     if ($(this).is(":checked")) {
//       selectedModifierGroupForItemArray.push({
//         ModifierGroupId: ModifierGroupValue,
//         MinValue: "",
//         MaxValue: "",
//       });
//       console.log("calling ajax");
//       ModifierGroupFetch(ModifierGroupValue);
//     } else {
//       $("#ajaxResponseOfModifierList")
//         .find(`.selectedModifier[data-value=${ModifierGroupValue}]`)
//         .remove();
//       selectedModifierGroupForItemArray =
//         selectedModifierGroupForItemArray.filter(
//           (item) => item.ModifierGroupId != ModifierGroupValue
//         );
//     }
//     console.log(selectedModifierGroupForItemArray);
//     // remove item when clicking the close button
//     // $(document).on('click','.remove-ModifierGroupForItem',function(){
//     //   let badge = $(this).closest('.selected-ModifierGroupforItem');
//     //   let value = badge.data("value");
//     //   badge.remove()
//     //   $(`.ModifierGroup-checkbox[value=${value}]`).prop("checked",false);
//     //   selectedModifierGroupForItemArray =selectedModifierGroupForItemArray.filter(item =>item !=value)
//     // })
//     // handle min selection and max selection
//     $(document).on("change", ".modifier-min-select", function () {
//       let parentModifierDiv = $(this).closest(".selectedModifier");
//       let modifierGroupId = parentModifierDiv.data("value");
//       let minValue = $(this).val();
//       let index = selectedModifierGroupForItemArray.findIndex(
//         (item) => item.ModifierGroupId == modifierGroupId
//       );
//       if (index !== -1) {
//         selectedModifierGroupForItemArray[index].MinValue = minValue;
//       }

//       console.log("Updated Array:", selectedModifierGroupForItemArray);
//     });
//     $(document).on("change", ".modifier-max-select", function () {
//       let parentModifierDiv = $(this).closest(".selectedModifier");
//       let modifierGroupId = parentModifierDiv.data("value");
//       let minValue = $(this).val();
//       let index = selectedModifierGroupForItemArray.findIndex(
//         (item) => item.ModifierGroupId == modifierGroupId
//       );
//       if (index !== -1) {
//         selectedModifierGroupForItemArray[index].MaxValue = minValue;
//       }

//       console.log("Updated Array:", selectedModifierGroupForItemArray);
//     });
//   });


//   //////////   fro edit item modifier

// // $(document).on("change", ".edit-ModifierGroup-checkbox", function () {
// //   let selectedModifierGroupForItemContainer = $(
// //     "#selectedModifierGroupForItem"
// //   );
// //   let ModifierGroupValue = $(this).val();
// //   console.log(ModifierGroupValue);
// //   var ModifierGroupText = $(this).next("label").text();
// //   if ($(this).is(":checked")) {
// //     selectedModifierGroupForItemArray.push({
// //       ModifierGroupId: ModifierGroupValue,
// //       MinValue: "",
// //       MaxValue: "",
// //     });
// //     console.log("calling ajax");
// //     ModifierGroupFetched(ModifierGroupValue);
// //   } else {
// //     $("#ajaxResponseOfModifierList")
// //       .find(`.selectedModifier[data-value=${ModifierGroupValue}]`)
// //       .remove();
// //     selectedModifierGroupForItemArray =
// //       selectedModifierGroupForItemArray.filter(
// //         (item) => item.ModifierGroupId != ModifierGroupValue
// //       );
// //   }
// //   console.log(selectedModifierGroupForItemArray);
// //   // remove item when clicking the close button
// //   // $(document).on('click','.remove-ModifierGroupForItem',function(){
// //   //   let badge = $(this).closest('.selected-ModifierGroupforItem');
// //   //   let value = badge.data("value");
// //   //   badge.remove()
// //   //   $(`.ModifierGroup-checkbox[value=${value}]`).prop("checked",false);
// //   //   selectedModifierGroupForItemArray =selectedModifierGroupForItemArray.filter(item =>item !=value)
// //   // })
// //   // handle min selection and max selection
// //   $(document).on("change", ".modifier-min-select", function () {
// //     let parentModifierDiv = $(this).closest(".selectedModifier");
// //     let modifierGroupId = parentModifierDiv.data("value");
// //     let minValue = $(this).val();
// //     let index = selectedModifierGroupForItemArray.findIndex(
// //       (item) => item.ModifierGroupId == modifierGroupId
// //     );
// //     if (index !== -1) {
// //       selectedModifierGroupForItemArray[index].MinValue = minValue;
// //     }

// //     console.log("Updated Array:", selectedModifierGroupForItemArray);
// //   });
// //   $(document).on("change", ".modifier-max-select", function () {
// //     let parentModifierDiv = $(this).closest(".selectedModifier");
// //     let modifierGroupId = parentModifierDiv.data("value");
// //     let minValue = $(this).val();
// //     let index = selectedModifierGroupForItemArray.findIndex(
// //       (item) => item.ModifierGroupId == modifierGroupId
// //     );
// //     if (index !== -1) {
// //       selectedModifierGroupForItemArray[index].MaxValue = minValue;
// //     }

// //     console.log("Updated Array:", selectedModifierGroupForItemArray);
// //   });
// // });

//   // remove SelectedModifierPartailView While Clicking delete button
//   $(document).on("click", ".deleteSelectedModifierTrash", function () {
//     let partailSeletedModifier = $(this).closest(".selectedModifier");
//     let value = partailSeletedModifier.data("value");
//     partailSeletedModifier.remove();
//     selectedModifierGroupForItemArray =
//       selectedModifierGroupForItemArray.filter(
//         (item) => item.ModifierGroupId != value
//       );
//     console.log("delete called");
//     $(`.ModifierGroup-checkbox[value=${value}]`).prop("checked", false);
//     $(`.edit-ModifierGroup-checkbox[value=${value}]`).prop("checked", false);
//   });
//   //
// });
// // For adding Items
// $(document).on("click", "#saveblock", function (e) {
//   e.preventDefault();
//   var formElement = $("#addItems");
//   $.validator.unobtrusive.parse(formElement);
//   if (formElement.valid()) {
//     var formData = new FormData(formElement[0]);
//     selectedModifierGroupForItemArray.forEach((item, index) => {
//       formData.append(
//         `AddItemsViewModel.ModiferDatas[${index}].ModifierGroupId`,
//         item.ModifierGroupId
//       );
//       formData.append(
//         `AddItemsViewModel.ModiferDatas[${index}].MinValue`,
//         item.MinValue
//       );
//       formData.append(
//         `AddItemsViewModel.ModiferDatas[${index}].MaxValue`,
//         item.MaxValue
//       );
//     });

//     $.ajax({
//       url: "/SuperAdmin/AddItemPost",
//       type: "POST",
//       data: formData,
//       processData: false,
//       contentType: false,
//       success: function (result) {

//         window.location.href = result.redirectUrl;
//       },
//       error: function () {

//         console.error("An unexpected error occurred.");
//       },
//     });
//   }
// });

// ///// for edit item

// // $(document).on("click", "#editItemSubmit", function (e) {
// //   e.preventDefault();
// //   var formElement = $("#editItems");
// //   $.validator.unobtrusive.parse(formElement);
// //   if (formElement.valid()) {
// //     var formData = new FormData(formElement[0]);
// //     selectedModifierGroupForItemArray.forEach((item, index) => {
// //       formData.append(
// //         `AddItemsViewModel.ModiferDatas[${index}].ModifierGroupId`,
// //         item.ModifierGroupId
// //       );
// //       formData.append(
// //         `AddItemsViewModel.ModiferDatas[${index}].MinValue`,
// //         item.MinValue
// //       );
// //       formData.append(
// //         `AddItemsViewModel.ModiferDatas[${index}].MaxValue`,
// //         item.MaxValue
// //       );
// //     });

// //     $.ajax({
// //       url: "/SuperAdmin/AddItemPost",
// //       type: "POST",
// //       data: formData,
// //       processData: false,
// //       contentType: false,
// //       success: function (result) {

// //         window.location.href = result.redirectUrl;
// //       },
// //       error: function () {

// //         console.error("An unexpected error occurred.");
// //       },
// //     });
// //   }
// // });
// // mass delete and delete
// $(document).on("change", "#selectAllItems", function () {
//   if ($(this).is(":checked")) {
//     $(".selectItem").prop("checked", true);
//   } else {
//     $(".selectItem").prop("checked", false);
//   }
// });
// $(document).on("change", ".selectItem", function () {
//   if ($(".selectItem:checked").length === $(".selectItem").length) {
//     $("#selectAllItems").prop("checked", true);
//   } else {
//     $("#selectAllItems").prop("checked", false);
//   }
// });
// let itemIds = [];
// $(document).on("click", "#deleteSelectedItems", function () {
//   console.log("arkin");
//   itemIds.length = 0; //emptying the array each time
//   $(".selectItem:checked").each(function () {
//     itemIds.push($(this).attr("data-itemId"));
//   });
//   console.log(itemIds);
//   if (itemIds.length > 0) {
//     $("#Delete").modal("show");
//   } else {
//     toastr.error("No items selected");
//   }
// });
// $(document).on("click", ".deleteItem", function () {
//   itemIds.length = 0; //emptying the array each time
//   let itemId = $(this).attr("data-Delete-itemId");
//   itemIds.push(itemId);
// });
// // for confirm delete
// $(document).on("click", "#deleteUserBtn", function () {
//   if (itemIds.length != 0) {
//     Delete(itemIds);
//   }
//   console.log("Rahul");
// });
// // ajax call to delete Items
// function Delete(itemIds) {
//   console.log(itemIds);
//   $.ajax({
//     url: "/SuperAdmin/DeleteItems",
//     type: "POST",
//     data: {
//       itemIds: itemIds,
//     },
//     success: function (data) {
//       window.location.href = data.redirectUrl;
//     },
//     error: function (xhr, error) {
//       var errResponse = JSON.parse(xhr.responseText);
//       window.location.href = errResponse.redirectUrl;
//       console.log("error:", error);
//     },
//   });
// }

// function ModifierGroupFetch(id) {
//   console.log("ajax called ");
//   $.ajax({
//     url: "/SuperAdmin/ModifierList",
//     type: "GET",
//     data: {
//       ModifierGroupId: id,
//     },
//     success: function (data) {
//       $("#ajaxResponseOfModifierList").append(data);
//     },
//     error: function () {},
//   });
// }


// function ModifierGroupFetched(id) {
//   console.log("ajax called ");
//   $.ajax({
//     url: "/SuperAdmin/ModifierList",
//     type: "GET",
//     data: {
//       ModifierGroupId: id,
//     },
//     success: function (data) {
//       $("#edit-ajaxResponseOfModifierList").append(data);
//     },
//     error: function () {},
//   });
// }


// let selectedModifierGroupForItemArray = [];
$(document).ready(function () {
  // for modifier group
  $(document).on("change", ".ModifierGroup-checkbox", function () {
    debugger
    let selectedModifierGroupForItemContainer = $(
      "#selectedModifierGroupForItem"
    );
    let ModifierGroupValue = $(this).val();
    console.log(ModifierGroupValue);
    var ModifierGroupText = $(this).next("label").text();
    if ($(this).is(":checked")) {
      selectedModifierGroupForItemArray.push({
        ModifierGroupId: ModifierGroupValue,
        MinValue: "",
        MaxValue: "",
      });
      console.log("calling ajax");
      ModifierGroupFetch(ModifierGroupValue);
    } else {
      $("#ajaxResponseOfModifierList")
        .find(`.selectedModifier[data-value=${ModifierGroupValue}]`)
        .remove();
      selectedModifierGroupForItemArray =
        selectedModifierGroupForItemArray.filter(
          (item) => item.ModifierGroupId != ModifierGroupValue
        );
    }
    console.log(selectedModifierGroupForItemArray);

    $(document).on("change", ".modifier-min-select", function () {
      let parentModifierDiv = $(this).closest(".selectedModifier");
      let modifierGroupId = parentModifierDiv.data("value");
      let minValue = $(this).val();
      let index = selectedModifierGroupForItemArray.findIndex(
        (item) => item.ModifierGroupId == modifierGroupId
      );
      if (index !== -1) {
        selectedModifierGroupForItemArray[index].MinValue = minValue;
      }

      console.log("Updated Array:", selectedModifierGroupForItemArray);
    });
    $(document).on("change", ".modifier-max-select", function () {
      let parentModifierDiv = $(this).closest(".selectedModifier");
      let modifierGroupId = parentModifierDiv.data("value");
      let minValue = $(this).val();
      let index = selectedModifierGroupForItemArray.findIndex(
        (item) => item.ModifierGroupId == modifierGroupId
      );
      if (index !== -1) {
        selectedModifierGroupForItemArray[index].MaxValue = minValue;
      }

      console.log("Updated Array:", selectedModifierGroupForItemArray);
    });
  });


  //////////   fro edit item modifier

  // $(document).on("change", ".edit-ModifierGroup-checkbox", function () {
  //   let selectedModifierGroupForItemContainer = $(
  //     "#selectedModifierGroupForItem"
  //   );
  //   let ModifierGroupValue = $(this).val();
  //   console.log(ModifierGroupValue);
  //   var ModifierGroupText = $(this).next("label").text();
  //   if ($(this).is(":checked")) {
  //     selectedModifierGroupForItemArray.push({
  //       ModifierGroupId: ModifierGroupValue,
  //       MinValue: "",
  //       MaxValue: "",
  //     });
  //     console.log("calling ajax");
  //     ModifierGroupFetched(ModifierGroupValue);
  //   } else {
  //     $("#ajaxResponseOfModifierList")
  //       .find(`.selectedModifier[data-value=${ModifierGroupValue}]`)
  //       .remove();
  //     selectedModifierGroupForItemArray =
  //       selectedModifierGroupForItemArray.filter(
  //         (item) => item.ModifierGroupId != ModifierGroupValue
  //       );
  //   }
  //   console.log(selectedModifierGroupForItemArray);

  //   // handle min selection and max selection
  //   $(document).on("change", ".modifier-min-select", function () {
  //     let parentModifierDiv = $(this).closest(".selectedModifier");
  //     let modifierGroupId = parentModifierDiv.data("value");
  //     let minValue = $(this).val();
  //     let index = selectedModifierGroupForItemArray.findIndex(
  //       (item) => item.ModifierGroupId == modifierGroupId
  //     );
  //     if (index !== -1) {
  //       selectedModifierGroupForItemArray[index].MinValue = minValue;
  //     }

  //     console.log("Updated Array:", selectedModifierGroupForItemArray);
  //   });
  //   $(document).on("change", ".modifier-max-select", function () {
  //     let parentModifierDiv = $(this).closest(".selectedModifier");
  //     let modifierGroupId = parentModifierDiv.data("value");
  //     let minValue = $(this).val();
  //     let index = selectedModifierGroupForItemArray.findIndex(
  //       (item) => item.ModifierGroupId == modifierGroupId
  //     );
  //     if (index !== -1) {
  //       selectedModifierGroupForItemArray[index].MaxValue = minValue;
  //     }

  //     console.log("Updated Array:", selectedModifierGroupForItemArray);
  //   });
  // });

  // remove SelectedModifierPartailView While Clicking delete button
  $(document).on("click", ".deleteSelectedModifierTrash", function () {
    let partailSeletedModifier = $(this).closest(".selectedModifier");
    let value = partailSeletedModifier.data("value");
    partailSeletedModifier.remove();
    selectedModifierGroupForItemArray =
      selectedModifierGroupForItemArray.filter(
        (item) => item.ModifierGroupId != value
      );
    console.log("delete called");
    $(`.ModifierGroup-checkbox[value=${value}]`).prop("checked", false);
    $(`.edit-ModifierGroup-checkbox[value=${value}]`).prop("checked", false);
  });
  //
});
// For adding Items
// $(document).on("click", "#saveblock", function (e) {
//   debugger
//   e.preventDefault();
//   var formElement = $("#addItems");
//   $.validator.unobtrusive.parse(formElement);
//   if (formElement.valid()) {
//     var formData = new FormData(formElement[0]);
//     selectedModifierGroupForItemArray.forEach((item, index) => {
//       formData.append(
//         `AddItemsViewModel.ModiferDatas[${index}].ModifierGroupId`,
//         item.ModifierGroupId
//       );
//       formData.append(
//         `AddItemsViewModel.ModiferDatas[${index}].MinValue`,
//         item.MinValue
//       );
//       formData.append(
//         `AddItemsViewModel.ModiferDatas[${index}].MaxValue`,
//         item.MaxValue
//       );
//     });

//     $.ajax({
//       url: "/SuperAdmin/AddItemPost",
//       type: "POST",
//       data: formData,
//       processData: false,
//       contentType: false,
//       success: function (result) {

//         window.location.href = result.redirectUrl;
//       },
//       error: function () {

//         console.error("An unexpected error occurred.");
//       },
//     });
//   }
// });

$(document).on("click", "#saveblock", function (e) {
  debugger
  e.preventDefault();
  var formElement = $("#addItems");
  $.validator.unobtrusive.parse(formElement);
  if (formElement.valid()) {
    var formData = new FormData(formElement[0]);
    selectedModifierGroupForItemArray.forEach((item, index) => {
      formData.append(
        `AddItemsViewModel.ModiferDatas[${index}].ModifierGroupId`,
        item.ModifierGroupId
      );
      formData.append(
        `AddItemsViewModel.ModiferDatas[${index}].MinValue`,
        item.MinValue
      );
      formData.append(
        `AddItemsViewModel.ModiferDatas[${index}].MaxValue`,
        item.MaxValue
      );
    });

    $.ajax({
      url: "/SuperAdmin/AddItemPost",
      type: "POST",
      data: formData,
      processData: false,
      contentType: false,
      success: function (result) {
        // If result is HTML, it means we got the partial view
        if (typeof result === 'string') {
          // Find the container for the items partial view and replace its content
          $("#itemsContainer").html(result);


          // Close the modal
          // $('#AddItems').modal('hide');
           // Get reference to the modal
           var $modal = $('#AddItems');
                    
           // Close the modal
           $modal.modal('hide');
           
           // Remove the modal from DOM after it's hidden
           $modal.one('hidden.bs.modal', function () {
               // Remove the modal backdrop
               $('.modal-backdrop').remove();
               
               // Remove fade class if needed
               $modal.removeClass('fade');
               
               // Optional: Remove the modal from DOM
               // $modal.remove();
           });
          
          //  a success message
          toastr.success("Item added successfully");

        } else if (result.redirectUrl) {
          // If we got a redirect URL, redirect the page
          window.location.href = result.redirectUrl;
        }
      },
      error: function () {
        console.error("An unexpected error occurred.");
      },
    });
  }
});


// mass delete and delete
$(document).on("change", "#selectAllItems", function () {
  if ($(this).is(":checked")) {
    $(".selectItem").prop("checked", true);
  } else {
    $(".selectItem").prop("checked", false);
  }
});
$(document).on("change", ".selectItem", function () {
  if ($(".selectItem:checked").length === $(".selectItem").length) {
    $("#selectAllItems").prop("checked", true);
  } else {
    $("#selectAllItems").prop("checked", false);
  }
});
let itemIds = [];
$(document).on("click", "#deleteSelectedItems", function () {
  console.log("arkin");
  itemIds.length = 0; //emptying the array each time
  $(".selectItem:checked").each(function () {
    itemIds.push($(this).attr("data-itemId"));
  });
  console.log(itemIds);
  if (itemIds.length > 0) {
    $("#Delete").modal("show");
  } else {
    toastr.error("No items selected");
  }
});
$(document).on("click", ".deleteItem", function () {
  itemIds.length = 0; //emptying the array each time
  let itemId = $(this).attr("data-Delete-itemId");
  itemIds.push(itemId);
});
// for confirm delete
$(document).on("click", "#deleteUserBtn", function () {
  if (itemIds.length != 0) {
    Delete(itemIds);
  }
  console.log("arjun");
});
// ajax call to delete Items
function Delete(itemIds) {
  console.log(itemIds);
  $.ajax({
    url: "/SuperAdmin/DeleteItems",
    type: "POST",
    data: {
      itemIds: itemIds,
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

function ModifierGroupFetch(id) {
  console.log("ajax called ");
  $.ajax({
    url: "/SuperAdmin/ModifierList",
    type: "GET",
    data: {
      ModifierGroupId: id,
    },
    success: function (data) {
      $("#ajaxResponseOfModifierList").append(data);
    },
    error: function () { },
  });
}


function ModifierGroupFetched(id) {
  console.log("ajax called ");
  $.ajax({
    url: "/SuperAdmin/ModifierList",
    type: "GET",
    data: {
      ModifierGroupId: id,
    },
    success: function (data) {
      $("#edit-ajaxResponseOfModifierList").append(data);
    },
    error: function () { },
  });
}

