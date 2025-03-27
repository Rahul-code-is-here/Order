// using DataAccessLayer.Models;
// using DataAccessLayer.ViewModels;
using PizzaShop.Domain.ViewModels;
using PizzaShop.Domain.DataModels;
namespace BussinessLogicLayer.Interface;

public interface ISuperAdminRepository
{
  
    Task<string> AddCategory(AddCategoryViewModel addCategoryViewModel);
    Task<List<AddCategoryViewModel>> GetCategoryList();
    Task<List<ModifiersViewModel>> GetModifiersList();
    Task<List<DropDownViewModel>> GetUnits();
    Task<AddCategoryViewModel> GetCategory(string id);
    Task<string> EditCategory(AddCategoryViewModel addCategoryViewModel);
    Task<string> DeleteCategory(string id);
    Task<string> AddItems(AddItemsViewModel addItemsViewModel,string uploadFolder);
    Task<EditItemsViewModel> GetItemForEdit(string id);
    Task<string> EditItem(EditItemsViewModel editItemsViewModel,string uploadFolder);
    Task<string> DeleteItem(List<string> itemIds);
    Task<string> AddModifier(ModifiersViewModel addModifier);
    Task<string> DeleteModifier(string id);
    Task<ModifiersViewModel> GetModifierForEdit(string id);

    Task<string> EditModifier(ModifiersViewModel addModifier);
    Task<Pagination<ItemsViewModel>> GetItemsList(Pagination<ItemsViewModel> itemList,string categoryId);
    Task<Pagination<ModifierItemsViewModel>> GetModifierItemsList(Pagination<ModifierItemsViewModel> modifierItemList,string modifierId);
    Task<Pagination<ModifierItemsViewModel>> GetAllModifierItemsList(Pagination<ModifierItemsViewModel> modifierItemList);
    Task<string> AddModifierItem(AddModifierItemViewModel addModifierItemViewModel);
    Task<AddModifierItemViewModel> GetModifierItemForEdit(string id);
    Task<string> EditModifierItem(AddModifierItemViewModel addModifierItemViewModel);

    Task<string> DeleteModifierItem(List<ModifierItemsViewModel> ModifierItems);

    Task<ModifiersforItems> GetModifierListForItems(string ModifierGroupId);

    Task<ModifiersforItems> GetModifiersforItemsForEdit(string ModifierId,string ItemId);
}
