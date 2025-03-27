// using Microsoft.EntityFrameworkCore;
// using PizzaShop.Domain.DataContext;
// using PizzaShop.Domain.DataModels;
// using PizzaShop.Domain.ViewModels;
// using PizzaShop.Repository.Interfaces;
// using System;
// using System.Collections.Generic;
// using System.Linq.Expressions;
// using System.Threading.Tasks;

// namespace PizzaShop.Repository.Implementations
// {
//     public class CategoryRepository : ICategoryRepository
//     {
//         private readonly PizzaShemaContext _context;

//         public CategoryRepository(PizzaShemaContext context)
//         {
//             _context = context;
//         }

//         public async Task<List<Category>> GetCategoriesAsync()
//         {
//             return await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
//         }

//         public async Task<List<Modifiergroup>> GetModifierAsync()
//         {
//             return await _context.Modifiergroups.Where(m => !m.IsDeleted).ToListAsync();
//         }

//         public async Task AddCategoryAsync(Category category)
//         {
//             if (category == null) throw new ArgumentNullException(nameof(category));

//             try
//             {
//                 await _context.Categories.AddAsync(category);
//                 await _context.SaveChangesAsync();
//             }
//             catch (Exception ex)
//             {
//                 throw new Exception("Error adding category to database.", ex);
//             }
//         }

//         public async Task UpdateCategoryAsync(Category category)
//         {
//             var existingCategory = await _context.Categories.FindAsync(category.Id);
//             if (existingCategory != null)
//             {
//                 existingCategory.CategoryName = category.CategoryName;
//                 existingCategory.Description = category.Description;
//                 await _context.SaveChangesAsync();
//             }
//         }

//         public async Task UpdateModifierAsync(Modifiergroup modifier)
//         {
//             var existingModifier = await _context.Modifiergroups.FindAsync(modifier.Id);
//             if (existingModifier != null)
//             {
//                 existingModifier.Name = modifier.Name;
//                 existingModifier.Description = modifier.Description;
//                 await _context.SaveChangesAsync();
//             }
//         }


//         public async Task DeleteCategoryAsync(int categoryId)
//         {
//             var category = await _context.Categories.FindAsync(categoryId);
//             if (category != null)
//             {
//                 _context.Categories.Remove(category);
//                 await _context.SaveChangesAsync();
//             }
//         }

//         public async Task SoftDeleteCategoryAsync(int id)
//         {
//             var category = await _context.Categories.FindAsync(id);
//             if (category != null)
//             {
//                 category.IsDeleted = true;
//                 _context.Entry(category).State = EntityState.Modified;
//                 await _context.SaveChangesAsync();
//             }
//         }

//         public async Task SoftDeleteModifierAsync(int id)
//         {
//             var modifier = await _context.Modifiergroups.FindAsync(id);
//             if (modifier != null)
//             {
//                 modifier.IsDeleted = true;
//                 _context.Entry(modifier).State = EntityState.Modified;
//                 await _context.SaveChangesAsync();
//             }
//         }

//         public async Task<List<Menuitem>> GetMenuItemsByCategoryAsync(int categoryId)
//         {
//             return await _context.Menuitems
//                 .Where(mi => mi.CategoryId == categoryId && !mi.IsDeleted)
//                 .ToListAsync();
//         }

//         public async Task<List<Modifier>> GetModifiersByGroupAsync(int groupId)
//         {

//                 return await _context.Modifiergroupmodifiers
//             .Where(mgm => mgm.ModifierGroupId == groupId && !mgm.Modifier.IsDeleted)
//             .Select(mgm => mgm.Modifier).ToListAsync();
//             // return await _context.Modifiers
//             //     .Where(m => m.Modifiergroupid == groupId && !m.IsDeleted)
//             //     .ToListAsync();
//         }

//         public async Task<List<string>> GetItemTypesAsync()
//         {
//             return await _context.Menuitems.Select(mi => mi.ItemType).Distinct().ToListAsync();
//         }


//         public async Task<List<Unit>> GetUnitsAsync()
//         {
//             return await _context.Units.Where(u => !u.IsDeleted).ToListAsync();
//         }

//         // public async Task AddAsync(Menuitem menuItem)
//         // {
//         //     // Fetch the list of valid unit IDs
//         //     // var validUnitIds = await _context.Units.Select(u => u.Id).ToListAsync();

//         //     await _context.Menuitems.AddAsync(menuItem);
//         //     await _context.SaveChangesAsync();
//         // }

//         public async Task AddAsync(Menuitem menuItem)
//         {
//             await _context.Menuitems.AddAsync(menuItem);
//             await _context.SaveChangesAsync(); // Save changes here to get the generated ID
//         }

//         public async Task AddMenuItemModifierAsync(MappingMenuItemWithModifier menuItemModifier)
//         {
//             await _context.MappingMenuItemWithModifiers.AddAsync(menuItemModifier);
//             await _context.SaveChangesAsync();
//         }


//         public async Task UpdateMenuItemAsync(int id, Menuitem menuItem)
//         {
//             var existingMenuItem = await _context.Menuitems.FindAsync(id);
//             if (existingMenuItem != null)
//             {
//                 existingMenuItem.CategoryId = menuItem.CategoryId;
//                 existingMenuItem.ItemName = menuItem.ItemName;
//                 existingMenuItem.ItemType = menuItem.ItemType;
//                 existingMenuItem.Rate = menuItem.Rate;
//                 existingMenuItem.Quantity = menuItem.Quantity;
//                 existingMenuItem.Unit = menuItem.Unit;
//                 existingMenuItem.IsAvailable = menuItem.IsAvailable;
//                 existingMenuItem.DefaultTax = menuItem.DefaultTax;
//                 existingMenuItem.TaxPercentage = menuItem.TaxPercentage;
//                 existingMenuItem.Shortcode = menuItem.Shortcode;
//                 existingMenuItem.Description = menuItem.Description;
//                 await _context.SaveChangesAsync();
//             }
//         }

//         public async Task UpdateMenuItemAsync(MenuItemViewModel model)
//         {
//             var menuItem = await _context.Menuitems.FindAsync(model.Id);
//             if (menuItem != null)
//             {
//                 menuItem.ItemName = model.ItemName;
//                 menuItem.ItemType = model.ItemType;
//                 menuItem.Rate = model.Rate;
//                 menuItem.Quantity = model.Quantity;
//                 menuItem.IsAvailable = model.IsAvailable;
//                 menuItem.CategoryId = model.CategoryId;
//                 menuItem.UnitId = int.Parse(model.Unit);
//                 menuItem.DefaultTax = model.DefaultTax;
//                 menuItem.TaxPercentage = (decimal)model.TaxPercentage;
//                 menuItem.Shortcode = model.Shortcode;
//                 menuItem.Description = model.Description;

//                 await _context.SaveChangesAsync();
//             }
//         }


//         // public class GenericRepository<TEntity> where TEntity : class
//         // {
//         //     private readonly DbContext _context;
//         //     private readonly DbSet<TEntity> _dbSet;

//         //     public GenericRepository(DbContext context)
//         //     {
//         //         _context = context;
//         //         _dbSet = context.Set<TEntity>();
//         //     }

//         // }

//         public async Task<bool> SoftDeleteItemAsync(int id)
//         {
//             var item = await _context.Menuitems.FindAsync(id);
//             if (item == null)
//             {
//                 return false;
//             }

//             item.IsDeleted = true;
//             _context.Menuitems.Update(item);
//             await _context.SaveChangesAsync();
//             return true;
//         }

//         public async Task<bool> BulkSoftDeleteItems(List<int> itemIds)
//         {
//             var items = _context.Menuitems.Where(item => itemIds.Contains(item.Id)).ToList();
//             if (items.Count == 0)
//             {
//                 return false;
//             }

//             foreach (var item in items)
//             {
//                 item.IsDeleted = true;
//             }

//             await _context.SaveChangesAsync();
//             return true;
//         }

//         public async Task<bool> BulkSoftDeleteModifiers(List<int> itemIds)
//         {
//             var items = _context.Modifiers.Where(item => itemIds.Contains(item.Id)).ToList();
//             if (items.Count == 0)
//             {
//                 return false;
//             }

//             foreach (var item in items)
//             {
//                 item.IsDeleted = true;
//             }

//             await _context.SaveChangesAsync();
//             return true;
//         }

//         public async Task<bool> AddAsync(Modifiergroup modifierGroup)
//         {
//             _context.Modifiergroups.Add(modifierGroup);
//             await _context.SaveChangesAsync();
//             return true;
//         }

//         // public async Task<bool> AddModifierAsync(ModifiersViewModel model)
//         // {
//         //     var modifier = new Modifier
//         //     {
//         //         ModifierName = model.Name,
//         //         Rate = model.Price,
//         //         UnitId = int.Parse(model.Unittype),
//         //         Quantity = model.Quantity,
//         //         Description = model.Description,

//         //     };

//         //     _context.Modifiers.Add(modifier);
//         //     await _context.SaveChangesAsync();


//         //     if (model.ModifierGroupIds != null && model.ModifierGroupIds.Any())
//         //     {
//         //         var mappings = model.ModifierGroupIds.Select(groupId => new Modifiergroupmodifier
//         //         {
//         //             ModifierId = modifier.Id,
//         //             ModifierGroupId = groupId

//         //         });

//         //         await _context.Modifiergroupmodifiers.AddRangeAsync(mappings);
//         //     }

//         //     return await _context.SaveChangesAsync() > 0;
//         // }
//         public async Task<bool> AddModifierAsync(ModifiersViewModel model)
//         {
//             if (model.ModifierGroupIds == null || !model.ModifierGroupIds.Any())
//             {
//                 // If no ModifierGroupIds are provided, create a single modifier
//                 var modifier = new Modifier
//                 {
//                     ModifierName = model.Name,
//                     Rate = model.Price,
//                     UnitId = int.Parse(model.Unittype),
//                     Quantity = model.Quantity,
//                     Description = model.Description
//                 };

//                 _context.Modifiers.Add(modifier);
//                 await _context.SaveChangesAsync();

//                 return true; // Single modifier added successfully
//             }

//             foreach (var groupId in model.ModifierGroupIds)
//             {
//                 var modifier = new Modifier
//                 {
//                     ModifierName = model.Name,
//                     Rate = model.Price,
//                     UnitId = int.Parse(model.Unittype),
//                     Quantity = model.Quantity,
//                     Description = model.Description
//                 };

//                 _context.Modifiers.Add(modifier);
//                 await _context.SaveChangesAsync(); // Save to get the generated Id

//                 var mapping = new Modifiergroupmodifier
//                 {
//                     ModifierId = modifier.Id,
//                     ModifierGroupId = groupId
//                 };

//                 _context.Modifiergroupmodifiers.Add(mapping);
//                 await _context.SaveChangesAsync();
//             }

//             return true; // All modifiers added successfully
//         }

//         public async Task<Modifier?> GetModifierByIdAsync(int id)
//         {
//             return await _context.Modifiers
//                 .FirstOrDefaultAsync(m => m.Id == id);
//         }

//         public async Task UpdateModifierGroupsAsync(int modifierId, List<int> modifierGroupIds)
//         {

//             var existingMappings = await _context.Modifiergroupmodifiers
//                 .Where(mgm => mgm.ModifierId == modifierId)
//                 .ToListAsync();

//             var activeGroupIds = existingMappings

//                 .Select(mgm => mgm.ModifierGroupId)
//                 .ToList();

//             var groupsToRemove = activeGroupIds.Except(modifierGroupIds).ToList();
//             var groupsToAdd = modifierGroupIds.Except(activeGroupIds).ToList();


//             // foreach (var mapping in existingMappings.Where(mgm => groupsToRemove.Contains(mgm.ModifierGroupId)))
//             // {
//             //     mapping.Isdeleted = true;
//             // }

//             foreach (var mapping in existingMappings.Where(mgm => groupsToAdd.Contains(mgm.ModifierGroupId)))
//             {
//                 // mapping.Isdeleted = false;
//                 groupsToAdd.Remove(mapping.ModifierGroupId);
//             }


//             if (groupsToAdd.Any())
//             {
//                 var newMappings = groupsToAdd.Select(groupId => new Modifiergroupmodifier
//                 {
//                     ModifierId = modifierId,
//                     ModifierGroupId = groupId,
//                     // Isdeleted = false
//                 }).ToList();

//                 await _context.Modifiergroupmodifiers.AddRangeAsync(newMappings);
//             }

//             await _context.SaveChangesAsync();
//         }

//         public async Task<bool> UpdateModifierAsync(Modifier modifie)
//         {
//             _context.Modifiers.Update(modifie);
//             return await _context.SaveChangesAsync() > 0;
//         }



//         // public ModifiersViewModel GetModifierDetails(int id)
//         // {
//         //     // Assuming you have a Modifier and ModifierGroup data model
//         //     var modifier = _context.Modifiers
//         //         .Where(m => m.Id == id)
//         //         .Select(m => new Modifier // Or your actual ViewModel
//         //         {
//         //             Id = m.Id,
//         //             ModifierName = m.ModifierName,
//         //             Rate = m.Rate,
//         //             Quantity = m.Quantity,
//         //             // Unittype = m.UnitId.ToString(),
//         //             Description = m.Description,
//         //             ModifierGroups = m.ModifierGroups.Select(mg => new ModifierGroupViewModel
//         //             {
//         //                 Id = mg.Id,
//         //                 Name = mg.Name
//         //             }).ToList()
//         //         }).FirstOrDefault();

//         //     return modifier;
//         // }


//         public ModifiersViewModel GetModifierById(int id)
//         {
//             var modifier = _context.Modifiers
//                 .Where(x => x.Id == id)
//                 .Select(x => new ModifiersViewModel
//                 {
//                     Id = x.Id,
//                     Name = x.ModifierName,
//                     Price = x.Rate,
//                     Unittype = x.UnitId.ToString(),
//                     // Unittype="kg",
//                     Quantity = x.Quantity,
//                     Description = x.Description,
//                     Isdeleted = x.IsDeleted,
//                     ModifierGroupIds = _context.Modifiergroupmodifiers
//                         .Where(mgm => mgm.ModifierId == x.Id)
//                         .Select(mgm => mgm.ModifierGroupId)
//                         .ToList()
//                 })
//                 .FirstOrDefault();

//             return modifier;
//         }

//         public async Task<bool> SoftDeleteModifiersAsync(int id)
//         {
//             var modifier = await _context.Modifiers.FindAsync(id);
//             if (modifier == null)
//             {
//                 return false;
//             }

//             modifier.IsDeleted = true;
//             _context.Modifiers.Update(modifier);
//             await _context.SaveChangesAsync();
//             return true;
//         }
//     }
// }

