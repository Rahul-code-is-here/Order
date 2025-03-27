// using PizzaShop.Domain.DataModels;
// using PizzaShop.Domain.ViewModels;
// using System.Collections.Generic;
// using System.Threading.Tasks;

// namespace PizzaShop.Repository.Interfaces
// {
//     public interface ICategoryService
//     {
//         Task<List<Category>> GetCategoriesAsync();

//         Task AddCategoryAsync(Category category);

//         Task UpdateCategoryAsync(Category category);

//         Task UpdateModifierAsync(Modifiergroup modifier);
//         Task DeleteCategoryAsync(int categoryId);

//         Task SoftDeleteCategoryAsync(int id);

//         Task SoftDeleteModifierAsync(int id);

//         Task<List<Menuitem>> GetMenuItemsByCategoryAsync(int categoryId);

//         Task<List<Modifier>> GetModifiersByGroupAsync(int groupId);

//         Task<List<string>> GetItemTypesAsync();
//         Task<List<Unit>> GetUnitsAsync();

//         Task<Menuitem> AddMenuItemAsync(MenuItemViewModel model);

//         Task UpdateMenuItemAsync(int id, Menuitem menuItem);

//         Task UpdateMenuItemAsync(MenuItemViewModel model);

//         Task<List<Modifiergroup>> GetModifierAsync();

//         Task<bool> SoftDeleteItemAsync(int id);

//         Task<bool> BulkSoftDeleteItems(List<int> itemIds);

//         Task<bool> AddModifierGroupAsync(ModifierGroupViewModel model);

//         Task<bool> AddModifierAsync(ModifiersViewModel model);

//         Task<bool> UpdateModifierAsync(ModifiersViewModel model);

//         //  ModifiersViewModel GetModifierDetails(int id);
//         ModifiersViewModel GetModifierById(int id);

//         Task<bool> SoftDeleteModifiersAsync(int id);

//         Task<bool> BulkSoftDeleteModifiers(List<int> itemIds);

//         // Task<object> GetModifierGroupsByMenuItemIdAsync(int menuItemId);

//     }
// }