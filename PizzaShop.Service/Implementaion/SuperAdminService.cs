using BussinessLogicLayer.Interface;
using PizzaShop.Domain.ViewModels;

namespace PizzaShop.Service.Implementaion;

public class SuperAdminService
{

    private readonly ISuperAdminRepository _superAdminRepository;

    public SuperAdminService(ISuperAdminRepository superAdminService)
    {
        _superAdminRepository = superAdminService;
    }


    public async Task<string> AddCategory(AddCategoryViewModel addCategoryViewModel)
    {
        return await _superAdminRepository.AddCategory(addCategoryViewModel);
    }

    public async Task<List<AddCategoryViewModel>> GetCategoryList()
    {
        return await _superAdminRepository.GetCategoryList();
    }
    public async Task<List<ModifiersViewModel>> GetModifiersList()
    {
        return await _superAdminRepository.GetModifiersList();
    }
    public async Task<List<DropDownViewModel>> GetUnits()
    {
        return await _superAdminRepository.GetUnits();
    }
    public async Task<AddCategoryViewModel> GetCategory(string id)
    {
        return await _superAdminRepository.GetCategory(id);
    }
    public async Task<string> EditCategory(AddCategoryViewModel addCategoryViewModel)
    {
        return await _superAdminRepository.EditCategory(addCategoryViewModel);
    }
    public async Task<string> DeleteCategory(string id)
    {
        return await _superAdminRepository.DeleteCategory(id);
    }
    public async Task<string> AddItems(AddItemsViewModel addItemsViewModel, string uploadFolder)
    {
        return await _superAdminRepository.AddItems(addItemsViewModel, uploadFolder);
    }
    public async Task<EditItemsViewModel> GetItemForEdit(string id)
    {
        return await _superAdminRepository.GetItemForEdit(id);
    }
    public async Task<string> EditItem(EditItemsViewModel editItemsViewModel, string uploadFolder)
    {
        return await _superAdminRepository.EditItem(editItemsViewModel, uploadFolder);
    }
    public async Task<string> DeleteItem(List<string> itemIds)
    {
        return await _superAdminRepository.DeleteItem(itemIds);
    }
    public async Task<string> AddModifier(ModifiersViewModel addModifier)
    {
        return await _superAdminRepository.AddModifier(addModifier);
    }
    public async Task<string> DeleteModifier(string id)
    {
        return await _superAdminRepository.DeleteModifier(id);
    }
    public async Task<ModifiersViewModel> GetModifierForEdit(string id)
    {
        return await _superAdminRepository.GetModifierForEdit(id);
    }

    public async Task<string> EditModifier(ModifiersViewModel addModifier)
    {
        return await _superAdminRepository.EditModifier(addModifier);
    }
    public async Task<Pagination<ItemsViewModel>> GetItemsList(Pagination<ItemsViewModel> itemList, string categoryId)
    {
        return await _superAdminRepository.GetItemsList(itemList, categoryId);
    }
    public async Task<Pagination<ModifierItemsViewModel>> GetModifierItemsList(Pagination<ModifierItemsViewModel> modifierItemList, string modifierId)
    {
        return await _superAdminRepository.GetModifierItemsList(modifierItemList, modifierId);
    }
    public async Task<Pagination<ModifierItemsViewModel>> GetAllModifierItemsList(Pagination<ModifierItemsViewModel> modifierItemList)
    {
        return await _superAdminRepository.GetAllModifierItemsList(modifierItemList);
    }
    public async Task<string> AddModifierItem(AddModifierItemViewModel addModifierItemViewModel)
    {
        return await _superAdminRepository.AddModifierItem(addModifierItemViewModel);
    }
    public async Task<AddModifierItemViewModel> GetModifierItemForEdit(string id)
    {
        return await _superAdminRepository.GetModifierItemForEdit(id);
    }
    public async Task<string> EditModifierItem(AddModifierItemViewModel addModifierItemViewModel)
    {
        return await _superAdminRepository.EditModifierItem(addModifierItemViewModel);
    }

    public async Task<string> DeleteModifierItem(List<ModifierItemsViewModel> ModifierItems)
    {
        return await _superAdminRepository.DeleteModifierItem(ModifierItems);
    }

    public async Task<ModifiersforItems> GetModifierListForItems(string ModifierGroupId)
    {
        return await _superAdminRepository.GetModifierListForItems(ModifierGroupId);
    }

    public async Task<ModifiersforItems> GetModifiersforItemsForEdit(string ModifierId, string ItemId)
    {
        return await _superAdminRepository.GetModifiersforItemsForEdit(ModifierId, ItemId);
    }
}
