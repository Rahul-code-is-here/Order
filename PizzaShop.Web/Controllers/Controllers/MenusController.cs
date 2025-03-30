using BussinessLogicLayer.Interface;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using PizzaShopRMS.Helpers;

namespace PizzaShopRMS.Controllers;

public class MenusController : Controller
{
    private readonly IMenuRepository _menuRepository;
    private readonly ICommonRepository _commonRepository;
    private readonly IWebHostEnvironment _webHostEnvironmnet;

    // private readonly 
    public MenusController(IMenuRepository menuRepository, ICommonRepository commonRepository, IWebHostEnvironment webHostEnvironmnet)
    {
        _menuRepository = menuRepository;
        _commonRepository = commonRepository;
        _webHostEnvironmnet = webHostEnvironmnet;
    }
    //Displays the menu page
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" },"Menu","View")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> Menu()
    {
        //state management for tabs
        try
        {
            if (HttpContext.Session.GetString("SelectedTab") == null || HttpContext.Session.GetString("SelectedTab") == "Items")
            {
                HttpContext.Session.SetString("SelectedTab", "Items");
            }
            else
            {
                HttpContext.Session.SetString("SelectedTab", "Modifiers");
            }
            ViewData["ActivePage"] = "Menu";
            var categoryList = await _menuRepository.GetCategoryList();
            var ModifiersList = await _menuRepository.GetModifiersList();
            var Units = await _menuRepository.GetUnits();
            MenuViewModel menu = new MenuViewModel()
            {
                CategoryList = categoryList,
                ModifiersList = ModifiersList,
                Units = Units
            };

            return View(menu);
        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return RedirectToAction("Menu", "Menus");
        }
    }

    // Handles adding a new category
    [HttpPost]
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" },"Menu","AddOrEdit")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> AddCategory(AddCategoryViewModel addCategoryViewModel)
    {
        try
        {
            //state management for tabs
            HttpContext.Session.SetString("SelectedTab", "Items");
            if (!ModelState.IsValid)
            {
                TempData["ToasterMessage"] = "Category Name is Required";
                TempData["ToasterType"] = "error";
                return RedirectToAction("Menu", "Menus");
            }
            var token = Request.Cookies["JwtToken"];
            var decodedToken = await _commonRepository.ValidateToken(token);
            var userId = decodedToken.UserId;
            addCategoryViewModel.CreatedBy = userId;
            var result = await _menuRepository.AddCategory(addCategoryViewModel);
            if (result == "Category Added Successfully")
            {
                TempData["ToasterMessage"] = result;
                TempData["ToasterType"] = "success";
                return RedirectToAction("Menu", "Menus");
            }
            TempData["ToasterMessage"] = result;
            TempData["ToasterType"] = "error";
            return RedirectToAction("Menu", "Menus");

        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return RedirectToAction("Menu", "Menus");
        }
    }

    // Retrieves a category for editing
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "Menu", "AddOrEdit")]
    public async Task<IActionResult> GetCategoryForEdit(string id)
    {
        try
        {
            //state management for tabs
            HttpContext.Session.SetString("SelectedTab", "Items");
            var category = await _menuRepository.GetCategory(id);
            return Json(category);
        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return RedirectToAction("Menu", "Menus");
        }
    }

    //Handles editing an existing category
    [HttpPost]
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "Menu", "AddOrEdit")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> EditCategoryPost(AddCategoryViewModel addCategoryViewModel)
    {
        try
        {
            //state management for tabs
            HttpContext.Session.SetString("SelectedTab", "Items");
            if (!ModelState.IsValid)
            {
                TempData["ToasterMessage"] = "Category Name is Required";
                TempData["ToasterType"] = "error";
                return RedirectToAction("Menu", "Menus");
            }
            var token = Request.Cookies["JwtToken"];
            var decodedToken = await _commonRepository.ValidateToken(token);
            var userId = decodedToken.UserId;
            addCategoryViewModel.EditedBy = userId;
            var result = await _menuRepository.EditCategory(addCategoryViewModel);
            if (result == "Category Edited Successfully")
            {
                TempData["ToasterMessage"] = result;
                TempData["ToasterType"] = "success";
                return RedirectToAction("Menu", "Menus");
            }
            TempData["ToasterMessage"] = result;
            TempData["ToasterType"] = "error";
            return RedirectToAction("Menu", "Menus");

        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return RedirectToAction("Menu", "Menus");
        }
    }

    //Handles deleting a category
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "Menu", "Delete")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> DeleteCategory(string id)
    {
        try
        {
            //state management for tabs
            HttpContext.Session.SetString("SelectedTab", "Items");
            var result = await _menuRepository.DeleteCategory(id);
            if (result == "Category Deleted Successfully")
            {
                TempData["ToasterMessage"] = result;
                TempData["ToasterType"] = "success";
                return RedirectToAction("Menu", "Menus");
            }
            TempData["ToasterMessage"] = result;
            TempData["ToasterType"] = "error";
            return RedirectToAction("Menu", "Menus");

        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return RedirectToAction("Menu", "Menus");
        }
    }

    // Fetches a paginated list of items
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "Menu", "View")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> ItemsListPartialView(Pagination<ItemsViewModel> itemsList, string CategoryId)
    {
        ViewData["ActivePage"] = "Menu";
        try
        {
            //state management for tabs
            HttpContext.Session.SetString("SelectedTab", "Items");
            var data = await _menuRepository.GetItemsList(itemsList, CategoryId);
            return PartialView("_ItemsPartialView", data);
        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            return RedirectToAction("Menu", "Menus");
        }
    }

    // Handles adding a new item
    [HttpPost]
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "Menu", "AddOrEdit")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> AddItemPost(AddItemsViewModel addItemsViewModel)
    {
        try
        {
            //state management for tabs
            HttpContext.Session.SetString("SelectedTab", "Items");
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, redirectUrl = "/Menus/Menu" });
            }
            var token = Request.Cookies["JwtToken"];
            var decodedToken = await _commonRepository.ValidateToken(token);
            var role = decodedToken.Role;
            if (!await _commonRepository.canAddOrEdit("Menu"))
            {
                return RedirectToAction("AccessDenied", "Login");
            }
            var userId = decodedToken.UserId;
            addItemsViewModel.CreatedBy = userId;
            string uploadFolder = Path.Combine(_webHostEnvironmnet.WebRootPath, "uploads");
            var result = await _menuRepository.AddItems(addItemsViewModel, uploadFolder);

            switch (result)
            {
                case "provide unique shortcode":
                case "Items already exist":
                case "Only jpg, png, jpeg images are allowed":
                case "Image must be less than 3MB":
                    TempData["ToasterMessage"] = result; // Store message
                    TempData["ToasterType"] = "error";   // Error type
                    return Json(new { success = false, redirectUrl = "/Menus/Menu" });

                default:
                    TempData["ToasterMessage"] = result; // Store success message
                    TempData["ToasterType"] = "success"; // Success type
                    return Json(new { success = true, redirectUrl = "/Menus/Menu" });
            }
        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = ex.Message;
            TempData["ToasterType"] = "error";
            return Json(new { success = false, redirectUrl = "/Menus/Menu" });
        }
    }

    //Retrieves an item for editing
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "Menu", "AddOrEdit")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> GetItemForEdit(string id)
    {
        try
        {
            var token = Request.Cookies["JwtToken"];
            var decodedToken = await _commonRepository.ValidateToken(token);
            var role = decodedToken.Role;
            if (!await _commonRepository.canAddOrEdit("Menu"))
            {
                return RedirectToAction("AccessDenied", "Login");
            }
            //state management for tabs
            HttpContext.Session.SetString("SelectedTab", "Items");
            var items = await _menuRepository.GetItemForEdit(id);
            return Json(new { success = true, items, redirectUrl = "/Menus/Menu" });
        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = ex.Message;
            TempData["ToasterType"] = "error";
            return Json(new { success = false, redirectUrl = "/Menus/Menu" });
        }
    }
    //Handles editing an existing item
    [HttpPost]
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "Menu", "AddOrEdit")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> EditItemPost(EditItemsViewModel editItemsViewModel)
    {
        //state management for tabs
        HttpContext.Session.SetString("SelectedTab", "Items");
        try
        {
            var token = Request.Cookies["JwtToken"];
            var decodedToken = await _commonRepository.ValidateToken(token);
            var role = decodedToken.Role;
            if (!await _commonRepository.canAddOrEdit("Menu"))
            {
                return RedirectToAction("AccessDenied", "Login");
            }
            var userId = decodedToken.UserId;
            editItemsViewModel.EditedBy = userId;
            string uploadFolder = Path.Combine(_webHostEnvironmnet.WebRootPath, "uploads");
            var result = await _menuRepository.EditItem(editItemsViewModel, uploadFolder);
            switch (result)
            {
                case "provide unique shortcode":
                case "Item with the name already exist":
                case "Only jpg, png, jpeg images are allowed":
                case "Image must be less than 3MB":
                    TempData["ToasterMessage"] = result; // Store message
                    TempData["ToasterType"] = "error";   // Error type
                    return Json(new { success = false, redirectUrl = "/Menus/Menu" });

                default:
                    TempData["ToasterMessage"] = result; // Store success message
                    TempData["ToasterType"] = "success"; // Success type
                    return Json(new { success = true, redirectUrl = "/Menus/Menu" });
            }

        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = ex.Message;
            TempData["ToasterType"] = "error";
            return Json(new { success = false, redirectUrl = "/Menus/Menu" });
        }
    }
    //Handles deleting an item
    [HttpPost]
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "Menu", "Delete")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> DeleteItems(List<string> itemIds)
    {
        try
        {
            //state management for tabs
            HttpContext.Session.SetString("SelectedTab", "Items");
            var result = await _menuRepository.DeleteItem(itemIds);
            TempData["ToasterMessage"] = result;
            TempData["ToasterType"] = "success";
            return Json(new { success = true, redirectUrl = "/Menus/Menu" });
        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/Menus/Menu" });
        }
    }
    [HttpPost]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "Menu", "AddOrEdit")]
    public async Task<IActionResult> AddModifiers(ModifiersViewModel AddModifiers)
    {
        try
        {
            //state management for tabs
            HttpContext.Session.SetString("SelectedTab", "Modifiers");
            if (!ModelState.IsValid)
            {
                TempData["ToasterMessage"] = "Model state is not valid";
                TempData["ToasterType"] = "error";
                return Json(new { success = false, activeTab = "nav-item-tab", redirectUrl = "/Menus/Menu" });
            }
            var token = Request.Cookies["JwtToken"];
            var decodedToken = await _commonRepository.ValidateToken(token);
            var userId = decodedToken.UserId;
            AddModifiers.CreatedBy = userId;
            var result = await _menuRepository.AddModifier(AddModifiers);
            if (result == "Modifier Added Successfully")
            {
                TempData["ToasterMessage"] = result;
                TempData["ToasterType"] = "success";
                return Json(new { success = true, activeTab = "nav-item-tab", redirectUrl = "/Menus/Menu" });
            }
            TempData["ToasterMessage"] = result;
            TempData["ToasterType"] = "error";
            return Json(new { success = true, activeTab = "nav-item-tab", redirectUrl = "/Menus/Menu" });
        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = true, activeTab = "nav-item-tab", redirectUrl = "/Menus/Menu" });
        }
    }

    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "Menu", "View")]
    public async Task<IActionResult> GetModifierItemsList(Pagination<ModifierItemsViewModel> modifierItemList, string ModifierGroupId)
    {
        try
        {
            //state management for tabs
            HttpContext.Session.SetString("SelectedTab", "Modifiers");
            var data = await _menuRepository.GetModifierItemsList(modifierItemList, ModifierGroupId);
            return PartialView("_ModifiersItemsPartialView", data);
        }
        catch (Exception ex)
        {

            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/Menus/Menu" });
        }
    }

    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "Menu", "View")]
    public async Task<IActionResult> GetAllModifierItemList(Pagination<ModifierItemsViewModel> modifierItemList)
    {
        try
        {
            //state management for tabs
            HttpContext.Session.SetString("SelectedTab", "Modifiers");
            var data = await _menuRepository.GetAllModifierItemsList(modifierItemList);
            return PartialView("_ExistingModifiersPartailView", data);
        }
        catch (Exception ex)
        {

            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = true, redirectUrl = "/Menus/Menu" });
        }
    }

    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "Menu", "AddOrEdit")]
    public async Task<IActionResult> GetModifierForEdit(string id)
    {
        try
        {
            //state management for tabs
            HttpContext.Session.SetString("SelectedTab", "Modifiers");
            var modifier = await _menuRepository.GetModifierForEdit(id);
            return Json(modifier);
        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = true, redirectUrl = "/Menus/Menu" });
        }
    }
    [HttpPost]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "Menu", "AddOrEdit")]
    public async Task<IActionResult> EditModifierGroupPost(ModifiersViewModel AddModifiers)
    {
        try
        {
            //state management for tabs
            HttpContext.Session.SetString("SelectedTab", "Modifiers");
            string? currentTab = HttpContext.Session.GetString("SelectedTab");

            // Log the session value (for debugging)
            Console.WriteLine($"Current Session Tab: {currentTab}");

            if (!ModelState.IsValid)
            {
                TempData["ToasterMessage"] = "Model state is not valid";
                TempData["ToasterType"] = "error";
                return Json(new { success = false, redirectUrl = "/Menus/Menu" });
            }
            var token = Request.Cookies["JwtToken"];
            var decodedToken = await _commonRepository.ValidateToken(token);
            var userId = decodedToken.UserId;
            AddModifiers.EditedBy = userId;
            var result = await _menuRepository.EditModifier(AddModifiers);
            switch (result)
            {
                case "Modifier with the name already exist":
                    TempData["ToasterMessage"] = result;
                    TempData["ToasterType"] = "error";
                    return Json(new { success = false, redirectUrl = "/Menus/Menu" });
                default:
                    TempData["ToasterMessage"] = result;
                    TempData["ToasterType"] = "success";
                    return Json(new { success = true, redirectUrl = "/Menus/Menu" });
            }
        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = true, redirectUrl = "/Menus/Menu" });
        }
    }

    // 
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "Menu", "Delete")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> DeleteModifierGroup(string id)
    {
        try
        {
            //state management for tabs
            HttpContext.Session.SetString("SelectedTab", "Modifiers");
            var result = await _menuRepository.DeleteModifier(id);
            TempData["ToasterMessage"] = result;
            TempData["ToasterType"] = "success";
            return Json(new { success = true, redirectUrl = "/Menus/Menu" });
        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/Menus/Menu" });
        }
    }
    [HttpPost]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "Menu", "AddOrEdit")]
    public async Task<IActionResult> AddModifierItem(AddModifierItemViewModel addModifierItemViewModel)
    {
        try
        {
            //state management for tabs
            HttpContext.Session.SetString("SelectedTab", "Modifiers");
            if (!ModelState.IsValid)
            {
                TempData["ToasterMessage"] = "Modifier Name is Required";
                TempData["ToasterType"] = "error";
                return Json(new { success = false, redirectUrl = "/Menus/Menu" });
            }
            var token = Request.Cookies["JwtToken"];
            var decodedToken = await _commonRepository.ValidateToken(token);
            var userId = decodedToken.UserId;
            addModifierItemViewModel.CreatedBy = userId;
            var result = await _menuRepository.AddModifierItem(addModifierItemViewModel);
            if (result == "Modifier Item Added Successfully")
            {
                TempData["ToasterMessage"] = result;
                TempData["ToasterType"] = "success";
                return Json(new { success = true, redirectUrl = "/Menus/Menu" });
            }
            TempData["ToasterMessage"] = result;
            TempData["ToasterType"] = "error";
            return Json(new { success = false, redirectUrl = "/Menus/Menu" });

        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = true, redirectUrl = "/Menus/Menu" });
        }
    }

    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "Menu", "AddOrEdit")]
    public async Task<IActionResult> GetModifierItemForEdit(string id)
    {
        try
        {
            //state management for tabs
            HttpContext.Session.SetString("SelectedTab", "Modifiers");
            var modifierItem = await _menuRepository.GetModifierItemForEdit(id);
            return Json(modifierItem);
        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = true, redirectUrl = "/Menus/Menu" });
        }
    }

    [HttpPost]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "Menu", "AddOrEdit")]
    public async Task<IActionResult> EditModifierPost(AddModifierItemViewModel addModifierItemViewModel)
    {
        try
        {
            //state management for tabs
            HttpContext.Session.SetString("SelectedTab", "Modifiers");
            if (!ModelState.IsValid)
            {
                TempData["ToasterMessage"] = "Model state is not valid";
                TempData["ToasterType"] = "error";
                return Json(new { success = false, redirectUrl = "/Menus/Menu" });

            }
            var token = Request.Cookies["JwtToken"];
            var decodedToken = await _commonRepository.ValidateToken(token);
            var userId = decodedToken.UserId;
            addModifierItemViewModel.EditedBy = userId;
            var result = await _menuRepository.EditModifierItem(addModifierItemViewModel);
            switch (result)
            {
                case "Modifier item with the name already exist":
                    TempData["ToasterMessage"] = result; // Store message
                    TempData["ToasterType"] = "error";   // Error type
                    return Json(new { success = false, redirectUrl = "/Menus/Menu" });
                default:
                    TempData["ToasterMessage"] = result; // Store success message
                    TempData["ToasterType"] = "success"; // Success type
                    return Json(new { success = true, redirectUrl = "/Menus/Menu" });
            }
        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = true, redirectUrl = "/Menus/Menu" });
        }
    }
    [HttpPost]
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "Menu", "Delete")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> DeleteModifierItems(List<ModifierItemsViewModel> modifierItemsViewModels)
    {
        try
        {
            //state management for tabs
            HttpContext.Session.SetString("SelectedTab", "Modifiers");
            var result = await _menuRepository.DeleteModifierItem(modifierItemsViewModels);
            TempData["ToasterMessage"] = result;
            TempData["ToasterType"] = "success";
            return Json(new { success = true, redirectUrl = "/Menus/Menu" });
        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/Menus/Menu" });
        }
    }
    // for getting modifierlist for items based on modifiergroup
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "Menu", "View")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> ModifierList(string ModifierGroupId)
    {
        try
        {
            //state management for tabs
            HttpContext.Session.SetString("SelectedTab", "Items");
            var result = await _menuRepository.GetModifierListForItems(ModifierGroupId);
            return PartialView("_ModifierForItem", result);
        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/Menus/Menu" });
        }
    }

    //
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "Menu", "AddOrEdit")]
    public async Task<IActionResult> FetchingModifierListforEditItem(string ModifierId, string ItemId)
    {
        try
        {
            //state management for tabs
            HttpContext.Session.SetString("SelectedTab", "Items");
            var result = await _menuRepository.GetModifiersforItemsForEdit(ModifierId, ItemId);
            return PartialView("_ModifierForItem", result);
        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/Menus/Menu" });
        }
    }

}
