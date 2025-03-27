let selectedModidferGroupForEdit = []
$(document).ready(function () {
  // add for multi selecting modifers
  let selectedModidferGroup = [];
  $(document).on("change", ".multi-select-checkbox", function () {
    var selectedContainer = $("#selectedItemsContainer");
    var value = $(this).val();
    var text = $(this).next("label").text();
    if ($(this).is(":checked")) {
      // Append the selected item as a badge
      selectedContainer.append(`
                    <span class="selected-item badge badge-light text-dark me-2 p-2 d-flex align-items-center border" data-value="${value}">
                        ${text} <button type="button" class="btn-close btn-close-dark ms-2 remove-item"></button>
                    </span>
                `);
      selectedModidferGroup.push(value);
      console.log(selectedModidferGroup);
    } else {
      // Remove the badge if unchecked
      selectedContainer.find(`.selected-item[data-value='${value}']`).remove();
      selectedModidferGroup = selectedModidferGroup.filter(    // removing value from arr
        (item) => item != value
      );
      console.log(selectedModidferGroup);
    }
  });

  // Remove item when clicking the close button fro add modifierItems
  $(document).on("click", ".remove-item", function () {
    var badge = $(this).closest(".selected-item");
    var value = badge.data("value");
    badge.remove();
    $(`.multi-select-checkbox[value='${value}']`).prop("checked", false); // Uncheck the checkbox
    selectedModidferGroup = selectedModidferGroup.filter(
      (item) => item != value
    );
    console.log(selectedModidferGroup);
  });

  // for taking form data for addmodifier items
  $(document).on("click", "#AddModifierItembtn", function () {
    let AddModifierItem = $("#AddModifierItemForm");
    $.validator.unobtrusive.parse(AddModifierItem);
    let AddModifierItemFormData = new FormData(AddModifierItem[0]);
    selectedModidferGroup.forEach(function (item) {
      console.log(item);
      AddModifierItemFormData.append('AddModifierItemViewModel.ModifierGroupIds[]', item);
    });
    console.log(AddModifierItem.valid());
    if (AddModifierItem.valid()) {
     
      // Log FormData properly
      for (let pair of AddModifierItemFormData.entries()) {
        console.log(pair[0] + ": " + pair[1]);
      }
      AddModifierItems(AddModifierItemFormData);
    }
  });

  //fetching modifier Items for edit
  $(document).on("click", ".editModifiersItems", function () {
    let modifierItemId = $(this).attr("data-modifierItemId");
    console.log(modifierItemId);
    $('.multi-select-checkbox-forEdit').prop('checked', false);
    selectedModidferGroupForEdit.length=0;
    getModifierForEdit(modifierItemId);
  });
  //add for multi selecting modifers in edit modifier modal
  $(document).on("change", ".multi-select-checkbox-forEdit", function () {
    let selectedContainer = $("#selectedItemsContainerForEdit");
    let value = $(this).val();
    let text = $(this).next("label").text();
    if ($(this).is(":checked")) {
      // Append the selected item as a badge
      selectedContainer.append(`
                    <span class="selected-item-forEdit badge badge-light text-dark me-2 p-2 d-flex align-items-center border" data-value="${value}">
                        ${text} <button type="button" class="btn-close btn-close-dark ms-2 remove-item-forEdit"></button>
                    </span>
                `);
      selectedModidferGroupForEdit.push({
        modifierGroupId: value,
        modifierGroupName: text
      });
      console.log(selectedModidferGroupForEdit);
    } else {
      // Remove the badge if unchecked
      selectedContainer.find(`.selected-item-forEdit[data-value='${value}']`).remove();
      selectedModidferGroupForEdit = selectedModidferGroupForEdit.filter(
        (item) => item.modifierGroupId != value
      );
      console.log(selectedModidferGroupForEdit);
    }
  });
  //Remove item when clicking the close button for edit modifierItems
  $(document).on("click", ".remove-item-forEdit", function () {
    let badge = $(this).closest(".selected-item-forEdit");
    let value = badge.data("value");
    console.log(value);
    badge.remove();
    $(`.multi-select-checkbox-forEdit[value='${value}']`).prop("checked", false); // Uncheck the checkbox
    selectedModidferGroupForEdit = selectedModidferGroupForEdit.filter(
      (item) => item.modifierGroupId != value
    );
    console.log("arjun");
    console.log(selectedModidferGroupForEdit);
  });
  // emptying the list
  // editing modifier item
  $(document).on("click", "#editModifierItembtn", function () {
    let editModifierItem = $("#editModifierItemForm");
    $.validator.unobtrusive.parse(editModifierItem);
    let editModifierItemFormData = new FormData(editModifierItem[0]);
    selectedModifierGroupForItemArray.forEach((item, index) => {
      formData.append(`AddItemsViewModel.ModiferDatas[${index}].ModifierGroupId`, item.ModifierGroupId);
      formData.append(`AddItemsViewModel.ModiferDatas[${index}].MinValue`, item.MinValue);
      formData.append(`AddItemsViewModel.ModiferDatas[${index}].MaxValue`, item.MaxValue);
  });
    selectedModidferGroupForEdit.forEach((i,index)=>{
      editModifierItemFormData.append(`AddModifierItemViewModel.ModifierValues[${index}].ModifierGroupId`, i.modifierGroupId)
      editModifierItemFormData.append(`AddModifierItemViewModel.ModifierValues[${index}].ModifierGroupName`, i.modifierGroupName)
    })
    console.log(editModifierItem.valid());
    if (editModifierItem.valid()) {
     
      // Log FormData properly
      for (let pair of editModifierItemFormData.entries()) {
        console.log(pair[0] + ": " + pair[1]);
      }
      EditModifierItem(editModifierItemFormData);
    }
  });

  // mass delete for modifier items
  $(document).on('change', '#selectAllModifierItems', function () {
    if ($(this).is(':checked')) {
      $('.selectModifierItem').prop('checked', true);
    }
    else {
      $('.selectModifierItem').prop('checked', false);
    }
  })
  $(document).on('change', '.selectModifierItem', function () {
    if ($('.selectModifierItem:checked').length === $('.selectModifierItem').length) {
      $('#selectAllModifierItems').prop('checked', true);
    } else {
      $('#selectAllModifierItems').prop('checked', false);
    }
  });
  let modifiersId = [];
  $(document).on('click', '#deleteSelectedModifiersItems', function () {
    console.log("callled arjun")
    modifiersId.length == 0;//emptying each time
    $('.selectModifierItem:checked').each(function () {
      var modifierObj = {
        ModifierItemid: $(this).attr('data-Delete-ModifierItemId'),
        modifierGroupId: $(this).attr('data-ModifierGroup-id')
      };
      modifiersId.push(modifierObj);
    })
    console.log(modifiersId)
    if (modifiersId.length > 0) {
      $('#Delete').modal('show');
    }
    else {
      toastr.error("No items selected");
    }
  })
  $(document).on('click', '.deleteModifierItem', function () {
    modifiersId.length = 0;//emptying the array each time 
    var modifierObj = {
      ModifierItemid: $(this).attr('data-Delete-ModifierItemId'),
      modifierGroupId: $(this).attr('data-ModifierGroup-id')
    };
    modifiersId.push(modifierObj);
    console.log("print modifier array", modifiersId)
  })
  console.log("print modifier array", modifiersId)
  // for confirm delete
  $(document).on('click', '#deleteUserBtn', function () {
    if (modifiersId.length != 0) {
      DeleteModifer(modifiersId);
    }
    console.log("arjun")
  })
});
// ajax call for addMofifierItem
function AddModifierItems(AddModifierItemFormData) {
  $.ajax({
    url: "/SuperAdmin/AddModifierItem",
    type: "POST",
    data: AddModifierItemFormData,
    processData: false, // Required for FormData
    contentType: false, // Required for FormData
    success: function (data) {
     
      if (data.redirectUrl) {
        window.location.href = data.redirectUrl;
      }
    },
    error: function (xhr, status, error) {
     
      try {
        var errResponse = JSON.parse(xhr.responseText);
        if (errResponse.redirectUrl) {
          window.location.href = errResponse.redirectUrl;
        }
      } catch (e) {
        console.error("Error parsing JSON:", e);
      }
    },
  });
}
//ajax call for edit modifier Items
function getModifierForEdit(modifierItemId) {
  console.log(modifierItemId)
  $.ajax({
    url: "/SuperAdmin/GetModifierItemForEdit",
    type: "GET",
    data: {
      id: modifierItemId,
    },
    success: function (data) {
      
      $("#editModifierItemGroupId").val(data.modifierGroupId);
      $("#editModifierItemId").val(data.modifierItemId);
      $("#editModifierItemName").val(data.modifierItemName);
      $("#editModifierItemRate").val(data.quantity);
      $("#editModifierItemQuantity").val(data.rate);
      $("#editModifierItemUnit").val(data.unit);
      $("#editModifierItemDescription").val(data.description);
      $('#selectedItemsContainerForEdit').empty();
      selectedModidferGroupForEdit.length=0;
      data.modifierValues.forEach((i) => {
        selectedModidferGroupForEdit.push(i);
      })
      if (selectedModidferGroupForEdit.length != 0) {
        console.log("arjun",selectedModidferGroupForEdit)
        selectedModidferGroupForEdit.forEach((i) => {
          $('#selectedItemsContainerForEdit').append(`
                    <span class="selected-item-forEdit badge badge-light text-dark me-2 p-2 d-flex align-items-center border" data-value="${i.modifierGroupId}">
                        ${i.modifierGroupName} <button type="button" class="btn-close btn-close-dark ms-2 remove-item-forEdit"></button>
                    </span>
            `)
          $(`.multi-select-checkbox-forEdit[value='${i.modifierGroupId}']`).prop("checked", true);
        })
      }
      console.log(selectedModidferGroupForEdit)

    },
    error: function (xhr, error) {
      var errResponse = JSON.parse(xhr.responseText);
      window.location.href = errResponse.redirectUrl;
      console.log("error:", error);
    },
  });
}
// ajax call for updating modifieritems
function EditModifierItem(editModifierItemFormData) {
  $.ajax({
    url: "/superAdmin/EditModifierPost",
    type: "POST",
    data: editModifierItemFormData,
    processData: false,
    contentType: false,
    success: function (data) {
    
      if (data.redirectUrl) {
        window.location.href = data.redirectUrl;
      }
    },
    error: function (xhr, error) {
      
      var errResponse = JSON.parse(xhr.responseText);
      window.location.href = errResponse.redirectUrl;
      console.log("error:", error);
    },
  });
}
// ajax call fro deleting button
function DeleteModifer(modifiersId) {
  console.log(modifiersId)
  $.ajax({
    url: '/SuperAdmin/DeleteModifierItems',
    type: 'POST',
    data: {
      modifierItemsViewModels: modifiersId
    },
    success: function (data) {
      window.location.href = data.redirectUrl;
    },
    error: function (xhr, error) {
      var errResponse = JSON.parse(xhr.responseText)
      window.location.href = errResponse.redirectUrl;
      console.log('error:', error)
    }
  })
}
