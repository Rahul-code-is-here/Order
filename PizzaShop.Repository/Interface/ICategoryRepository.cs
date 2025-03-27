// using PizzaShop.Domain.DataModels;
// using PizzaShop.Domain.ViewModels;
// using System.Collections.Generic;
// using System.Linq.Expressions;
// using System.Threading.Tasks;

// namespace PizzaShop.Repository.Interfaces
// {
//     public interface ICategoryRepository
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

//         Task AddAsync(Menuitem menuItem);

//         Task AddMenuItemModifierAsync(MappingMenuItemWithModifier menuItemModifier);

//         // Task<Menuitem> GetMenuItemByIdAsync(int id);
//         // Task UpdateMenuItemAsync(Menuitem menuItem);

//         Task UpdateMenuItemAsync(int id, Menuitem menuItem);

//         Task UpdateMenuItemAsync(MenuItemViewModel model);

//         Task<List<Modifiergroup>> GetModifierAsync();

//         Task<bool> SoftDeleteItemAsync(int id);

//         Task<bool> BulkSoftDeleteItems(List<int> itemIds);

//         Task<bool> BulkSoftDeleteModifiers(List<int> itemIds);

//         Task<bool> AddAsync(Modifiergroup modifierGroup);

//         // Task<bool> AddModifierAsync(ModifiersViewModel model);
//         Task<bool> AddModifierAsync(ModifiersViewModel model);

//         Task<Modifier?> GetModifierByIdAsync(int id);

//         Task UpdateModifierGroupsAsync(int modifierId, List<int> modifierGroupIds);

//         Task<bool> UpdateModifierAsync(Modifier modifier);

//         // ModifiersViewModel GetModifierDetails(int id);
//          ModifiersViewModel GetModifierById(int id);

//          Task<bool> SoftDeleteModifiersAsync(int id);

//     }
// }