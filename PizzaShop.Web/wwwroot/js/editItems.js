// $(document).ready(function () {
//   let existingModifiers = [];
//   let itemId = 0;
//   $(document).on("click", ".editItems", function () {
//     itemId = $(this).attr("data-itemId");
//     console.log(itemId);
//     getItemForEdit();
//   });
//   // ajax call to get item for editItem
//   function getItemForEdit() {
//     $.ajax({
//       url: "/SuperAdmin/GetItemForEdit",
//       type: "GET",
//       data: {
//         id: itemId,
//       },
//       success: function (data) {
//         $("#editItemId").val(data.items.itemId);
//         $("#editItemCategoryId").val(data.items.categoryId);
//         $("#editItemName").val(data.items.itemName);
//         $("#editItemType").val(data.items.itemType);
//         $("#editItemRate").val(data.items.rate);
//         $("#editItemQuantity").val(data.items.quantity);
//         $("#editItemUnit").val(data.items.unit);
//         $("#editItemIsAvailable").prop("checked", data.items.isAvailable);
//         $("#editItemDefualt").prop("checked", data.items.isAvailable);
//         $("#editItemTaxPercentage").val(data.items.taxPercentage);
//         $("#editItemShortCode").val(data.items.shortCode);
//         $("#editItemDescription").val(data.items.description);
//         existingModifiers = data.items.modiferDatas
//         console.log(existingModifiers);
//         existingModifiers.forEach((item) => {
//           fetchingExistingModifiersOfItemforEdit(item.modifierGroupId, data.items.itemId)
//         });
//         // checking the chekboxes
//         $('.edit-ModifierGroup-checkbox')
//       },
//       error: function () {

//       },
//     });
//   }

// //check box handle in edit item modifier
//   $(document).on("change", ".edit-ModifierGroup-checkbox", function () {
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
//       ModifierGroupFetched(ModifierGroupValue);
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



//   let editForm;
//   $(document).on("click", "#editItemSubmit", function (e) {
//     e.preventDefault();

//     console.log($.fn.validate);
//     console.log($.validator);
//     console.log($.validator.unobtrusive);

//     $.validator.unobtrusive.parse("#editItemsForm");
//     console.log($("#editItemsForm").valid());

//     var editFormSelector = $("#editItemsForm");
//     editForm = new FormData(editFormSelector[0]);

//     //add modifierGroup data to the form
//     selectedModifierGroupForItemArray.forEach((item, index) => {
//       editForm.append(
//         `AddItemsViewModel.ModiferDatas[${index}].ModifierGroupId`,
//         item.ModifierGroupId
//       );
//       editForm.append(
//         `AddItemsViewModel.ModiferDatas[${index}].MinValue`,
//         item.MinValue
//       );
//       editForm.append(
//         `AddItemsViewModel.ModiferDatas[${index}].MaxValue`,
//         item.MaxValue
//       );
//     });

//     console.log(editForm);
//     if (editFormSelector.valid()) {

//       editItemPost();
//     }
//   });
//   function editItemPost() {
//     console.log("ajax call");
//     $.ajax({
//       url: "/SuperAdmin/EditItemPost",
//       type: "POST",
//       data: editForm,
//       processData: false,
//       contentType: false,
//       success: function (data) {

//         window.location.href = data.redirectUrl;
//       },
//       error: function () {
//         console.log("error")
//       },
//     });
//   }
// });

// // function ajax call for the fetched data of edit
// function fetchingExistingModifiersOfItemforEdit(modifierGroupId, itemId) {
//   $.ajax({
//     url: '/SuperAdmin/FetchingModifierListforEditItem',
//     type: 'GET',
//     data: {
//       ModifierId: modifierGroupId,
//       ItemId: itemId
//     },
//     success: function (data) {
//       $("#edit-ajaxResponseOfModifierList").append(data);
//     },
//     error: function (xhr, error) {

//     }
//   })
// }
// // function restoreCheckBoX



// let selectedModifierGroupForItemArray = [];

let existingModifiers = [];
$(document).ready(function () {
  let itemId = 0;
  $(document).on("click",".editItems", function () {
    itemId = $(this).attr("data-itemId");
    console.log(itemId);
    getItemForEdit();
  });
  //render modifier group for selected checkbox
  $(document).on("change", ".edit-ModifierGroup-checkbox", function () {
    let selectedModifierGroupForItemContainer = $(
      "#selectedModifierGroupForItem"
    );
    let ModifierGroupValue = $(this).val();
    console.log(ModifierGroupValue);
    var ModifierGroupText = $(this).next("label").text();
    if ($(this).is(":checked")) {
      existingModifiers.push({
        modifierGroupId:ModifierGroupValue,
        minValue:"0",
        maxValue:"0"
      })
      console.log("calling ajax");
      // ModifierGroupFetch(ModifierGroupValue);
      EditModifierGroupFetch(ModifierGroupValue)
    } else {
      console.log("uncheked called")
      $("#edit-ajaxResponseOfModifierList")
        .find(`.selectedModifier[data-value=${ModifierGroupValue}]`)
        .remove();
        existingModifiers = existingModifiers.filter(item => item.modifierGroupId != ModifierGroupValue);
        console.log("edited");
        console.log(existingModifiers);
    }
    console.log(existingModifiers);
  });
    // handle min selection and max selection
    $(document).on('change','.modifier-min-select',function(){
      let parentModifierDiv = $(this).closest(".selectedModifier");
      let modifierGroupId = parentModifierDiv.data("value");
      let minValue = $(this).val();
      let index = existingModifiers.findIndex(item => item.modifierGroupId == modifierGroupId);
      if (index !== -1) {
        existingModifiers[index].minValue = minValue;
      }
  
      console.log("Updated Array:", existingModifiers);
  });
  $(document).on('change','.modifier-max-select',function(){
    let parentModifierDiv = $(this).closest(".selectedModifier");
    let modifierGroupId = parentModifierDiv.data("value");
    let maxValue = $(this).val();
    let index = existingModifiers.findIndex(item => item.modifierGroupId == modifierGroupId);
    if (index !== -1) {
      existingModifiers[index].maxValue = maxValue;
    }
    console.log("Updated Array:", existingModifiers);
  });
   // remove SelectedModifierPartailView While Clicking delete button
   $(document).on("click", ".deleteSelectedModifierTrash", function () {
    let partailSeletedModifier = $(this).closest(".selectedModifier");
    let value = partailSeletedModifier.data("value");
    partailSeletedModifier.remove();
    existingModifiers = existingModifiers.filter(item =>item.modifierGroupId!=value);
    console.log("delete called");
    $(`.edit-ModifierGroup-checkbox[value=${value}]`).prop("checked", false);
  });
  // ajax call to get item for editItem
  function getItemForEdit() {
    $.ajax({
      url: "/SuperAdmin/GetItemForEdit",
      type: "GET",
      data: {
        id: itemId,
      },
      success: function (data) {
        $("#editItemId").val(data.items.itemId);
        $("#editItemCategoryId").val(data.items.categoryId);
        $("#editItemName").val(data.items.itemName);
        $("#editItemType").val(data.items.itemType);
        $("#editItemRate").val(data.items.rate);
        $("#editItemQuantity").val(data.items.quantity);
        $("#editItemUnit").val(data.items.unit);
        $("#editItemIsAvailable").prop("checked", data.items.isAvailable);
        $("#editItemDefualt").prop("checked", data.items.defaultTax);
        $("#editItemTaxPercentage").val(data.items.taxPercentage);
        $("#editItemShortCode").val(data.items.shortCode);
        $("#editItemDescription").val(data.items.description);
        existingModifiers= data.items.modiferDatas
        console.log(existingModifiers);
        existingModifiers.forEach((item) => {
          fetchingExistingModifiersOfItemforEdit(item.modifierGroupId,data.items.itemId)
        });
        restoreCheckBoxForModifierGroup()
        // checking the chekboxes
      },
      error: function () {
       console.log("error");
      },
    });
  }
  let editForm;
  $(document).on("click", "#editItemSubmit", function (e) {
    debugger
    e.preventDefault();
    $.validator.unobtrusive.parse("#editItemsForm");
    console.log($("#editItemsForm").valid());
   
    let editFormSelector = $("#editItemsForm");
    editForm = new FormData(editFormSelector[0]);
    existingModifiers.forEach((item, index) => {
      editForm.append(`EditItemsViewModel.ModiferDatas[${index}].ModifierGroupId`, item.modifierGroupId);
      editForm.append(`EditItemsViewModel.ModiferDatas[${index}].MinValue`, item.minValue);
      editForm.append(`EditItemsViewModel.ModiferDatas[${index}].MaxValue`, item.maxValue);
  });
  for (let pair of editForm.entries()) {
    console.log(pair[0] + ': ' + pair[1]);  
}
    console.log(editForm);
    if (editFormSelector.valid()) {
       
        editItemPost();
    }
  });
  function editItemPost() {
    console.log("ajax call");
    $.ajax({
      url: "/SuperAdmin/EditItemPost",
      type: "POST",
      data: editForm,
      processData: false, 
      contentType: false,
      success: function (data) {
        
        window.location.href = data.redirectUrl;
      },
      error: function () {
        console.log("error");
      },
    });
  }
});

// function ajax call for the fetched data of edit
function fetchingExistingModifiersOfItemforEdit(modifierGroupId,itemId){
    $.ajax({
      url:'/SuperAdmin/FetchingModifierListforEditItem',
      type:'GET',
      data:{
        ModifierId:modifierGroupId,
        ItemId:itemId
      },
      success:function(data){
        $("#edit-ajaxResponseOfModifierList").append(data);
      },
      error:function(xhr,error){
        console.log("error while fetching existing modifiers for edit" + error);
      }
    })
}
// restore checkbox
function restoreCheckBoxForModifierGroup(){
  console.log("Rahul calling")
  let selectedIds = existingModifiers.map((m)=>m.modifierGroupId)
  $('.edit-ModifierGroup-checkbox').each(function(){
    let checkbox = $(this);
    let modifierId = checkbox.attr("data-edit-ModifierGroup-itemid");
    checkbox.prop("checked", selectedIds.includes(modifierId));
  })
}
// fetching modifier group
function EditModifierGroupFetch(id) {
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
    error: function () {},
  });
}
// const EditfileTag = document.getElementById("Edit-actual-file-btn");
// const EditshowTag = document.getElementById("Edit-Update-tag");

// EditfileTag.addEventListener('change',()=>
//     {
//         let fileName = EditfileTag.files[0].name;
//         EditshowTag.innerHTML = fileName;
//     });