using System.Linq;
using BussinessLogicLayer.Interface;
// using DataAccessLayer.Models;
// using DataAccessLayer.ViewModels;
using Microsoft.EntityFrameworkCore;
// using Org.BouncyCastle.Math.EC.Rfc7748;
using PizzaShop.Domain.DataContext;
using PizzaShop.Domain.DataModels;
using PizzaShop.Domain.ViewModels;
using BC = BCrypt.Net.BCrypt;

namespace PizzaShop.Repository.Implementations;

public class SuperAdminRepository : ISuperAdminRepository
{
    private readonly PizzaShemaContext _db;
    // private readonly ICommonRepository _commonRepository;
    // private readonly IEmailRepository _emailRepository;

    public SuperAdminRepository(PizzaShemaContext db)
    {
        _db = db;
        // _commonRepository = commonRepository;
        // _emailRepository = emailRepository;

    }


    //Adds a new category after checking if it already exists.
    public async Task<string> AddCategory(AddCategoryViewModel addCategoryViewModel)
    {
        var IsCategory = await _db.Categories.AnyAsync(u => u.CategoryName.ToLower() == addCategoryViewModel.CategoryName.Trim().ToLower());
        if (IsCategory)
        {
            return "Category already exist";
        }
        Category category = new Category()
        {
            CategoryName = addCategoryViewModel.CategoryName.Trim(),
            // CreatedBy = addCategoryViewModel.CreatedBy,
            // CreatedDate = DateTime.Now,
        };
        if (addCategoryViewModel.CategoryDescription != null)
        {
            category.Description = addCategoryViewModel.CategoryDescription;
        }
        await _db.Categories.AddAsync(category);
        await _db.SaveChangesAsync();
        return "Category Added Successfully";
    }

    //Retrieves a list of active categories.
    public async Task<List<AddCategoryViewModel>> GetCategoryList()
    {
        var categoryList = await (from c in _db.Categories
                                  where c.IsDeleted == false
                                  orderby c.CategoryName
                                  select new AddCategoryViewModel()
                                  {
                                      CategoryName = c.CategoryName,
                                      CategoryId = c.Id.ToString(),
                                  }).ToListAsync();
        return categoryList;
    }

    //Retrieves a category by its ID and returns an AddCategoryViewModel.
    public async Task<AddCategoryViewModel> GetCategory(string id)
    {
        var category = await (from c in _db.Categories
                              where c.Id == int.Parse(id)
                              select new AddCategoryViewModel()
                              {
                                  CategoryName = c.CategoryName,
                                  CategoryDescription = c.Description,
                                  CategoryId = id,
                              }).FirstOrDefaultAsync();
        return category;
    }

    //Retrieves a list of all available units and returns them in a dropdown-friendly format.
    public async Task<List<DropDownViewModel>> GetUnits()
    {
        var units = await _db.Units.Select(u => new DropDownViewModel()
        {
            Text = u.Name,
            Value = u.Id.ToString()

        }).ToListAsync();

        units = units.OrderBy(u => u.Text).ToList();
        return units;
    }

    //Edits an existing category based on the provided AddCategoryViewModel.
    public async Task<string> EditCategory(AddCategoryViewModel addCategoryViewModel)
    {
        var IsCategory = await _db.Categories.AnyAsync(u => u.CategoryName.ToLower() == addCategoryViewModel.CategoryName.Trim().ToLower() && u.Id.ToString() != addCategoryViewModel.CategoryId);
        if (IsCategory)
        {
            return "Category already exist";
        }
        var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id.ToString() == addCategoryViewModel.CategoryId);

        category.CategoryName = addCategoryViewModel.CategoryName.Trim();
        category.Description = addCategoryViewModel.CategoryDescription.Trim();
        // category.EditedBy = addCategoryViewModel.EditedBy;
        // category.EditDate = DateTime.Now;
        await _db.SaveChangesAsync();
        return "Category Edited Successfully";
    }

    // Marks a Category as deleted along with its associated items
    public async Task<string> DeleteCategory(string Id)
    {
        var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id.ToString() == Id);
        if (category == null)
        {
            return "error while deleteing category";
        }
        category.IsDeleted = true;
        var items = await _db.Menuitems.Where(e => e.Id.ToString() == Id).ToListAsync();
        foreach (var obj in items)
        {
            obj.IsDeleted = true;
        }
        await _db.SaveChangesAsync();
        return "Category Deleted Successfully";
    }

    //Retrieves a paginated list of items under a specific category.
    public async Task<Pagination<ItemsViewModel>> GetItemsList(Pagination<ItemsViewModel> itemList, string categoryId)
    {
        var items = from i in _db.Menuitems
                    where i.CategoryId.ToString() == categoryId && i.IsDeleted == false
                    select new ItemsViewModel()
                    {
                        CategoryId = i.CategoryId,
                        ItemType = i.ItemType,
                        ItemId = i.Id,
                        IsAvailable = (bool)i.IsAvailable,
                        Img = i.ImagePath,
                        ItemName = i.ItemName,
                        Quantity = (int)i.Quantity,
                        Rate = (int)i.Rate,
                    };
        if (!string.IsNullOrEmpty(itemList.SearchFilter))
        {
            items = items.Where(i => i.ItemName.ToLower().Contains(itemList.SearchFilter.Trim().ToLower()));
        }

        items= items.OrderBy(i => i.ItemName);
        itemList.NumberOfItems = await items.CountAsync();

        var temp = (itemList.CurrentPage - 1) * itemList.PageSize;
        itemList.StartIndex = temp + 1;
        itemList.EndIndex = temp + itemList.PageSize;
        itemList.TotalPages = (int)Math.Ceiling((double)itemList.NumberOfItems / itemList.PageSize);
        itemList.Items = await items.Skip(temp).Take(itemList.PageSize).ToListAsync();
        return itemList;
    }

    //Adds a new item to the database with validation checks for duplicate names and shortcodes.
    public async Task<string> AddItems(AddItemsViewModel addItemsViewModel, string uploadFolder)
    {
        var IsItem = await _db.Menuitems.AnyAsync(i => i.Id == addItemsViewModel.CategoryId && i.ItemName.ToLower() == addItemsViewModel.ItemName.Trim().ToLower());
      
        if (IsItem)
        {
            return "Items already exist";
        }
        if (addItemsViewModel.ShortCode != null)
        {
            var IsShortCodeAvailable = await _db.Menuitems.AnyAsync(i => i.Shortcode == addItemsViewModel.ShortCode.Trim());
            if (IsShortCodeAvailable)
            {
                return "provide unique shortcode";
            }
        }
        if (addItemsViewModel.ShortCode == null){
            addItemsViewModel.ShortCode="";
        }
        if(addItemsViewModel.Description==null){
            addItemsViewModel.Description="";
        }
        var ext = Path.GetExtension(addItemsViewModel.ImageUpload.FileName).ToLower();
        var size = addItemsViewModel.ImageUpload.Length;//inbytes
        if (!ext.Equals(".png") && !ext.Equals(".jpeg") && !ext.Equals("jpg"))
        {
            return "Only jpg, png,jpeg  images are allowed";
        }
        if (size >= 3000000)
        {
            return "Image must be less than 3MB";
        }
        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(addItemsViewModel.ImageUpload.FileName);
        string filepath = Path.Combine(uploadFolder, uniqueFileName);
        using (var fileStream = new FileStream(filepath, FileMode.Create))
        {
            await addItemsViewModel.ImageUpload.CopyToAsync(fileStream);
        }

        Menuitem item = new Menuitem()
        {
            // Id = addItemsViewModel.CategoryId,
            ItemName = addItemsViewModel.ItemName.Trim().ToLower(),
            CategoryId= addItemsViewModel.CategoryId,
            Rate = int.Parse(addItemsViewModel.Rate),
            ItemType = addItemsViewModel.ItemType == "1" ? "Vegetarian" : "Non-Vegetarian",
            Quantity = int.Parse(addItemsViewModel.Quantity),
            IsAvailable = addItemsViewModel.IsAvailable,
            DefaultTax = addItemsViewModel.DefaultTax,
            TaxPercentage = int.Parse(addItemsViewModel.TaxPercentage),
            Shortcode = addItemsViewModel.ShortCode,
            Description = addItemsViewModel.Description,
            ImagePath = uniqueFileName,
            // CreatedBy = addItemsViewModel.CreatedBy,
            // CreatedDate = DateTime.Now,
            // Taxesid = 1,
            UnitId = int.Parse(addItemsViewModel.Unit),
        };
        await _db.Menuitems.AddAsync(item);
        await _db.SaveChangesAsync();
        if (addItemsViewModel.ModiferDatas != null)
        {
            var bunchItems = new List<MappingMenuItemWithModifier>();
            foreach (var modifier in addItemsViewModel.ModiferDatas)
            {
                bunchItems.Add(new MappingMenuItemWithModifier()
                {
                    // Id = int.Parse(modifier.ModifierGroupId),
                    MenuItemId = item.Id,
                    ModifierGroupId= int.Parse(modifier.ModifierGroupId),
                    MaxSelectionAllowed = int.Parse(modifier.MaxValue),
                    MinSelectionRequired = int.Parse(modifier.MinValue)
                });
            }
            await _db.MappingMenuItemWithModifiers.AddRangeAsync(bunchItems);
            await _db.SaveChangesAsync();
        }
        return "Items Added Successfully";
    }

    //Retrieves an item by its ID for editing
    public async Task<EditItemsViewModel> GetItemForEdit(string id)
    {
        var item = await (from i in _db.Menuitems
                          join u in _db.Units on i.UnitId equals u.Id
                          where i.Id.ToString() == id
                          select new EditItemsViewModel()
                          {
                              ItemId = i.Id,
                              CategoryId = i.CategoryId,
                              ItemName = i.ItemName,
                              ItemType = i.ItemType.ToLower() == "vegetarian" ? "1" : "2",
                              Rate = i.Rate.ToString(),
                              Quantity = i.Quantity.ToString(),
                              Unit = i.UnitId.ToString(),
                              IsAvailable = (bool)i.IsAvailable,
                              DefaultTax = (bool)i.DefaultTax,
                              TaxPercentage = i.TaxPercentage.ToString(),
                              ShortCode = i.Shortcode,
                              Description = i.Description,
                          }).FirstOrDefaultAsync();
        item.ModiferDatas = await _db.MappingMenuItemWithModifiers.Where(i => i.MenuItemId.ToString() == id).Select(i => new SelectedModifierDataForItemViewModel
        {
            ModifierGroupId = i.ModifierGroupId.ToString(),
            MaxValue = i.MaxSelectionAllowed.ToString(),
            MinValue = i.MinSelectionRequired.ToString()
        }).ToListAsync();
        return item;
    }
    //Updates an existing item with new details
    public async Task<string> EditItem(EditItemsViewModel editItemsViewModel, string uploadFolder)
    {
        var IsUnique = await _db.Menuitems.AnyAsync(i => i.ItemName == editItemsViewModel.ItemName.Trim() && i.Id != editItemsViewModel.ItemId);
        if (IsUnique)
        {
            return "Item with the name already exist";
        }
        if (editItemsViewModel.ShortCode != null)
        {
            var IsShortCodeAvailable = await _db.Menuitems.AnyAsync(i => i.Shortcode == editItemsViewModel.ShortCode.Trim() && i.Id != editItemsViewModel.ItemId);
            if (IsShortCodeAvailable)
            {
                return "provide unique shortcode";
            }
        }
         if (editItemsViewModel.ShortCode == null){
            editItemsViewModel.ShortCode="";
        }
        if(editItemsViewModel.Description==null){
            editItemsViewModel.Description="";
        }
        string uniqueFileName = "";
        if (editItemsViewModel.ImageUpload != null)
        {
            var ext = Path.GetExtension(editItemsViewModel.ImageUpload.FileName).ToLower();
            var size = editItemsViewModel.ImageUpload.Length;
            if (!ext.Equals(".png") && !ext.Equals(".jpeg") && !ext.Equals("jpg"))
            {
                return "Only jpg, png, jpeg images are allowed";
            }
            if (size >= 3000000)
            {
                return "Image must be less than 3MB";
            }
            uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(editItemsViewModel.ImageUpload.FileName);
            string filepath = Path.Combine(uploadFolder, uniqueFileName);
            using (var fileStream = new FileStream(filepath, FileMode.Create))
            {
                await editItemsViewModel.ImageUpload.CopyToAsync(fileStream);
            }
        }
        var item = await _db.Menuitems.FirstOrDefaultAsync(i => i.Id == editItemsViewModel.ItemId);
        if (editItemsViewModel.ImageUpload != null)
        {
            item.ImagePath = uniqueFileName;
        }
        item.CategoryId = editItemsViewModel.CategoryId;
        item.ItemName = editItemsViewModel.ItemName;
        item.ItemType = editItemsViewModel.ItemType=="2"? "Vegetarian" : "Non-Vegetarian";
        item.Rate = decimal.Parse(editItemsViewModel.Rate);
        item.Quantity = int.Parse(editItemsViewModel.Quantity);
        item.UnitId = int.Parse(editItemsViewModel.Unit);
        item.IsAvailable = editItemsViewModel.IsAvailable;
        item.DefaultTax = editItemsViewModel.DefaultTax;
        item.TaxPercentage = decimal.Parse(editItemsViewModel.TaxPercentage);
        item.Shortcode = editItemsViewModel.ShortCode;
        item.Description = editItemsViewModel.Description;

     var mapping = await _db.MappingMenuItemWithModifiers.Where(i => i.MenuItemId == editItemsViewModel.ItemId).ToListAsync();
        _db.MappingMenuItemWithModifiers.RemoveRange(mapping);
        if (editItemsViewModel.ModiferDatas != null)
        {
            var bunchItems = new List<MappingMenuItemWithModifier>();
            foreach (var modifier in editItemsViewModel.ModiferDatas)
            {
                bunchItems.Add(new MappingMenuItemWithModifier()
                {
                    ModifierGroupId = int.Parse(modifier.ModifierGroupId),
                    MenuItemId = item.Id,
                    MaxSelectionAllowed = int.Parse(modifier.MaxValue),
                    MinSelectionRequired = int.Parse(modifier.MinValue)
                });
            }
            await _db.MappingMenuItemWithModifiers.AddRangeAsync(bunchItems);
        }
        await _db.SaveChangesAsync();
        return "item updated successfully";

        // await _db.SaveChangesAsync();
        // return "item updated successfully";
    }

    // Marks an item as deleted
    public async Task<string> DeleteItem(List<string> itemIds)
    {
        var items = await _db.Menuitems.Where(i => itemIds.Contains(i.Id.ToString())).ToListAsync();
        foreach (var item in items)
        {
            item.IsDeleted = true;
        }
        await _db.SaveChangesAsync();
        return "Items Deleted Successfully";
    }

    //Retrieves a list of active categories.
    public async Task<List<ModifiersViewModel>> GetModifiersList()
    {
        var modifiersList = await (from m in _db.Modifiergroups
                                   where m.IsDeleted == false
                                   orderby m.Name
                                   select new ModifiersViewModel()
                                   {
                                       ModifierGroupName = m.Name,
                                       ModifierGroupId = m.Id.ToString()
                                   }).ToListAsync();

                                   modifiersList = modifiersList.OrderBy(m => m.ModifierGroupName).ToList();
        return modifiersList;
    }
    public async Task<string> AddModifier(ModifiersViewModel addModifier)
    {
        var IsModifier = await _db.Modifiergroups.AnyAsync(u => u.Name.ToLower() == addModifier.ModifierGroupName.Trim().ToLower());
        if (IsModifier)
        {
            return "Modifier already exist";
        }

        Modifiergroup modifierGroup = new Modifiergroup()
        {
            Name = addModifier.ModifierGroupName.Trim(),
            // CreatedBy = addModifier.CreatedBy,
            // CreatedDate = DateTime.Now,
        };
        if (addModifier.Description != null)
        {
            modifierGroup.Description = addModifier.Description;
        }
        await _db.Modifiergroups.AddAsync(modifierGroup);
        await _db.SaveChangesAsync();


        if (addModifier.ModifierItems != null)
        {
            var bunchItems = new List<Modifiergroupmodifier>();
            foreach (var item in addModifier.ModifierItems)
            {
                bunchItems.Add(new Modifiergroupmodifier() { Id = modifierGroup.Id, ModifierId = int.Parse(item) });
            }
            await _db.Modifiergroupmodifiers.AddRangeAsync(bunchItems);
            await _db.SaveChangesAsync();
        }
        return "Modifier Added Successfully";
    }

    public async Task<string> DeleteModifier(string id)
    {
        var modifierGroup = await _db.Modifiergroups.FirstOrDefaultAsync(i => i.Id.ToString() == id);
        modifierGroup.IsDeleted = true;
        var mapping = await _db.Modifiergroupmodifiers.Where(i => i.Id.ToString() == id).ToListAsync();
        _db.Modifiergroupmodifiers.RemoveRange(mapping);
        await _db.SaveChangesAsync();
        return "Modifier Delted Successfully";
    }
    public async Task<Pagination<ModifierItemsViewModel>> GetModifierItemsList(Pagination<ModifierItemsViewModel> modifierItemList, string modifierId)
    {

        var modifierItems = from m in _db.Modifiergroups
                            join x in _db.Modifiergroupmodifiers
                            on m.Id equals x.ModifierGroupId
                            join mi in _db.Modifiers
                            on x.ModifierId equals mi.Id
                            join u in _db.Units
                            on mi.UnitId equals u.Id
                            where m.Id.ToString() == modifierId && mi.IsDeleted == false
                            select new ModifierItemsViewModel()
                            {
                                ModifierGroupId = modifierId,
                                ModifierItemId = mi.Id.ToString(),
                                ModifierItemName = mi.ModifierName,
                                Quantity = mi.Quantity.ToString(),
                                Rate = mi.Rate.ToString(),
                                Unit = u.Name

                            };
        if (!string.IsNullOrEmpty(modifierItemList.SearchFilter))
        {
            modifierItems = modifierItems.Where(i => i.ModifierItemName.ToLower().Contains(modifierItemList.SearchFilter.Trim().ToLower()));
        }
 
modifierItems = modifierItems.OrderBy(i => i.ModifierItemName);  // for sorted order list of modifier item

        modifierItemList.NumberOfItems = await modifierItems.CountAsync();
        var temp = (modifierItemList.CurrentPage - 1) * modifierItemList.PageSize;
        modifierItemList.StartIndex = temp + 1;
        modifierItemList.EndIndex = temp + modifierItemList.PageSize;
        modifierItemList.TotalPages = (int)Math.Ceiling((double)modifierItemList.NumberOfItems / modifierItemList.PageSize);
        modifierItemList.Items = await modifierItems.Skip(temp).Take(modifierItemList.PageSize).ToListAsync();
        return modifierItemList;
    }
    public async Task<Pagination<ModifierItemsViewModel>> GetAllModifierItemsList(Pagination<ModifierItemsViewModel> modifierItemList)
    {
        var allModifiersItemList = from mi in _db.Modifiers
                                   join u in _db.Units
                                   on mi.UnitId equals u.Id
                                   where mi.IsDeleted == false
                                   orderby mi.ModifierName ascending
                                   select new ModifierItemsViewModel()
                                   {
                                       ModifierItemId = mi.Id.ToString(),
                                       ModifierItemName = mi.ModifierName,
                                       Quantity = mi.Quantity.ToString(),
                                       Rate = mi.Rate.ToString(),
                                       Unit = u.Name
                                   };
        if (!string.IsNullOrEmpty(modifierItemList.SearchFilter))
        {
            allModifiersItemList = allModifiersItemList.Where(mi => mi.ModifierItemName.ToLower().Contains(modifierItemList.SearchFilter.Trim().ToLower()));
        }
        modifierItemList.NumberOfItems = await allModifiersItemList.CountAsync();
        var temp = (modifierItemList.CurrentPage - 1) * modifierItemList.PageSize;
        modifierItemList.StartIndex = temp + 1;
        modifierItemList.EndIndex = temp + modifierItemList.PageSize;
        modifierItemList.TotalPages = (int)Math.Ceiling((double)modifierItemList.NumberOfItems / modifierItemList.PageSize);
        modifierItemList.Items = await allModifiersItemList.Skip(temp).Take(modifierItemList.PageSize).ToListAsync();
        return modifierItemList;
    }
    public async Task<ModifiersViewModel> GetModifierForEdit(string id)
    {
        var modifier = await (from m in _db.Modifiergroups
                              where m.Id == int.Parse(id)
                              select new ModifiersViewModel()
                              {
                                  ModifierGroupName = m.Name,
                                  ModifierGroupId = m.Id.ToString(),
                                  Description = m.Description,
                              }).FirstOrDefaultAsync();

        modifier.ModifierItemsData = await (from mi in _db.Modifiers
                                            join map in _db.Modifiergroupmodifiers
                                            on mi.Id equals map.ModifierId
                                            where map.ModifierGroupId.ToString() == id
                                            select new ModifierItemValuesViewModel()
                                            {
                                                ItemId = map.ModifierId.ToString(),
                                                ItemName = mi.ModifierName
                                            }).ToListAsync();

        return modifier;
    }
    public async Task<string> EditModifier(ModifiersViewModel editModifier)
    {
        var IsUnique = await _db.Modifiergroups.AnyAsync(i => i.Name.ToLower() == editModifier.ModifierGroupName.Trim().ToLower() && i.Id.ToString() != editModifier.ModifierGroupId);
        if (IsUnique)
        {
            return "Modifier with the name already exist";
        }
        var ModifierGroup = await _db.Modifiergroups.FirstOrDefaultAsync(i => i.Id.ToString() == editModifier.ModifierGroupId);
        ModifierGroup.Name = editModifier.ModifierGroupName;
        if (editModifier.Description != null)
        {
            ModifierGroup.Description = editModifier.Description;
        }
        // ModifierGroup.EditedBy = editModifier.EditedBy;
        // ModifierGroup.EditDate = DateTime.Now;
        // Modifier items for which we need to unlink the modifiers groups
        var mapping = await _db.Modifiergroupmodifiers.Where(i => i.ModifierGroupId.ToString() == editModifier.ModifierGroupId).ToListAsync();
        var modifierItemTobeRemoved = mapping.Where(m => !editModifier.ModifierItems.Contains(m.ModifierId.ToString())).ToList();

        if (modifierItemTobeRemoved.Any())
        {
            _db.Modifiergroupmodifiers.RemoveRange(modifierItemTobeRemoved);
        }
        // new Modifier items for which we need to link the modifiers groups
        var onlyMappingIds = mapping.Select(i => i.Id).ToList();
        var modifierItemTobeAdded = editModifier.ModifierItems.Where(m => !onlyMappingIds.Contains(int.Parse(m))).ToList();

        var newMapping = modifierItemTobeAdded.Select(m => new Modifiergroupmodifier
        {
            ModifierGroupId = int.Parse(editModifier.ModifierGroupId),
            ModifierId = int.Parse(m)
        });
        _db.Modifiergroupmodifiers.AddRange(newMapping);

        await _db.SaveChangesAsync();
        return "ModifierGroup updated successfully";
    }
    public async Task<string> AddModifierItem(AddModifierItemViewModel addModifierItemViewModel)
    {
        var IsModifier = await _db.Modifiers.AnyAsync(i => i.ModifierName.ToLower() == addModifierItemViewModel.ModifierItemName.ToLower());
        if (IsModifier)
        {
            return "Modifier already exist";
        }
        Modifier newModifierItem = new Modifier()
        {
            ModifierName = addModifierItemViewModel.ModifierItemName,
            Rate = addModifierItemViewModel.Rate,
            Quantity = addModifierItemViewModel.Quantity,
            UnitId = addModifierItemViewModel.Unit,
            Description = addModifierItemViewModel.Description,
            // CreatedBy = addModifierItemViewModel.CreatedBy,
        };
        await _db.Modifiers.AddAsync(newModifierItem);
        await _db.SaveChangesAsync();

        var bunchModifiers = new List<Modifiergroupmodifier>();
        foreach (var items in addModifierItemViewModel.ModifierGroupIds)
        {
            bunchModifiers.Add(new Modifiergroupmodifier() { ModifierGroupId = int.Parse(items), ModifierId = newModifierItem.Id });
        }
        await _db.Modifiergroupmodifiers.AddRangeAsync(bunchModifiers);
        await _db.SaveChangesAsync();

        return "Modifier Item Added Successfully";
    }
    public async Task<AddModifierItemViewModel> GetModifierItemForEdit(string id)
    {
        var modifierItem = await (from mi in _db.Modifiers
                                  join map in _db.Modifiergroupmodifiers
                                  on mi.Id equals map.ModifierId
                                  join u in _db.Units
                                  on mi.UnitId equals u.Id
                                  where mi.Id == int.Parse(id)
                                  select new AddModifierItemViewModel()
                                  {
                                      ModifierItemId = mi.Id,
                                      ModifierItemName = mi.ModifierName,
                                      Description = mi.Description,
                                      Quantity = (int)mi.Quantity,
                                      Rate = (int)mi.Rate,
                                      Unit = (int)mi.UnitId,
                                  }).FirstOrDefaultAsync();
        modifierItem.ModifierValues = await (from m in _db.Modifiergroups
                                             join map in _db.Modifiergroupmodifiers
                                             on m.Id equals map.ModifierGroupId
                                             where map.ModifierId.ToString() == id
                                             select new ModifierValue
                                             {
                                                 ModifierGroupId = m.Id.ToString(),
                                                 ModifierGroupName = m.Name,
                                             }).ToListAsync();
        return modifierItem;
    }
    public async Task<string> EditModifierItem(AddModifierItemViewModel editModifierItemViewModel)
    {

        var IsUnique = await _db.Modifiers.AnyAsync(i => i.ModifierName.ToLower() == editModifierItemViewModel.ModifierItemName.Trim().ToLower() && i.Id != editModifierItemViewModel.ModifierItemId);
        if (IsUnique)
        {
            return "Modifier item with the name already exist";
        }
        var modifierItem = await _db.Modifiers.FirstOrDefaultAsync(i => i.Id == editModifierItemViewModel.ModifierItemId);
        modifierItem.ModifierName = editModifierItemViewModel.ModifierItemName;
        modifierItem.Rate = editModifierItemViewModel.Rate;
        modifierItem.Quantity = editModifierItemViewModel.Quantity;
        modifierItem.UnitId = editModifierItemViewModel.Unit;
        modifierItem.Description = editModifierItemViewModel.Description;
        // modifierItem.EditedBy = editModifierItemViewModel.EditedBy;
        // modifierItem.EditDate = DateTime.Now;

        var mapping = await _db.Modifiergroupmodifiers.Where(i => i.ModifierId == editModifierItemViewModel.ModifierItemId).ToListAsync();

        // Modifier groups for which we need to unlink the modifiers items
        var OnlyModifiersIds = editModifierItemViewModel.ModifierValues.Select(i => i.ModifierGroupId).ToList();
        var ModifiersTobeRemoved = mapping.Where(m => !OnlyModifiersIds.Contains(m.ModifierGroupId.ToString())).ToList();

        if (ModifiersTobeRemoved.Any())
        {
            _db.Modifiergroupmodifiers.RemoveRange(ModifiersTobeRemoved);
        }
        // new Modifier groups for which we need to link the modifiers items
        var onlymappingIds = mapping.Select(i => i.ModifierGroupId).ToList();
        var modifiersTobeAdded = editModifierItemViewModel.ModifierValues.Where(m => !onlymappingIds.Contains(int.Parse(m.ModifierGroupId))).ToList();

        var newMapping = modifiersTobeAdded.Select(m => new Modifiergroupmodifier
        {
            ModifierGroupId = int.Parse(m.ModifierGroupId),
            ModifierId = (int)editModifierItemViewModel.ModifierItemId
        });
        _db.Modifiergroupmodifiers.AddRange(newMapping);

        await _db.SaveChangesAsync();
        return "item updated successfully";
    }
    public async Task<string> DeleteModifierItem(List<ModifierItemsViewModel> ModifierItems)
    {
        var modifierGroupIds = ModifierItems.Select(i => i.ModifierGroupId).ToList();
        var modifierItemIds = ModifierItems.Select(i => i.ModifierItemId).ToList();

        var mapping = await _db.Modifiergroupmodifiers.Where(i => modifierGroupIds.Contains(i.ModifierGroupId.ToString()) && modifierItemIds.Contains(i.ModifierId.ToString())).ToListAsync();
        _db.Modifiergroupmodifiers.RemoveRange(mapping);
        await _db.SaveChangesAsync();
        return "Modifer Items Deleted Successfully";
    }
    public async Task<ModifiersforItems> GetModifierListForItems(string ModifierGroupId)
    {
        var modifierItemList = await (from map in _db.Modifiergroupmodifiers
                                      join m in _db.Modifiers
                                      on map.ModifierId equals m.Id
                                      where map.ModifierGroupId.ToString() == ModifierGroupId
                                      select new ModifierNameRate
                                      {
                                          ModifierItemName = m.ModifierName,
                                          Rate = m.Rate.ToString()
                                      }).ToListAsync();
        var modiferDetails = await _db.Modifiergroups.FirstOrDefaultAsync(i => i.Id.ToString() == ModifierGroupId);
        ModifiersforItems modifier = new ModifiersforItems()
        {
            MaxValue = null,
            MinValue = null,
            ModifierGroupId = modiferDetails.Id.ToString(),
            ModifierGroupName = modiferDetails.Name,
            ModifierItems = modifierItemList
        };
        return modifier;
    }

    public async Task<ModifiersforItems> GetModifiersforItemsForEdit(string ModifierId, string ItemId)
    {
        var modifierItemList = await (from map in _db.Modifiergroupmodifiers
                                      join m in _db.Modifiers
                                      on map.ModifierId equals m.Id
                                      where map.ModifierGroupId.ToString() == ModifierId
                                      select new ModifierNameRate
                                      {
                                          ModifierItemName = m.ModifierName,
                                          Rate = m.Rate.ToString()
                                      }).ToListAsync();
        var modiferDetails = await _db.Modifiergroups.FirstOrDefaultAsync(i => i.Id.ToString() == ModifierId);
        var ModifierGroupForItem = await _db.MappingMenuItemWithModifiers.FirstOrDefaultAsync(i => i.ModifierGroupId.ToString() == ModifierId && i.MenuItemId.ToString() == ItemId);
        ModifiersforItems modifier = new ModifiersforItems()
        {
            MaxValue = ModifierGroupForItem.MaxSelectionAllowed.ToString(),
            MinValue = ModifierGroupForItem.MinSelectionRequired.ToString(),
            ModifierGroupId = modiferDetails.Id.ToString(),
            ModifierGroupName = modiferDetails.Name,
            ModifierItems = modifierItemList
        };
        return modifier;
    }
}

