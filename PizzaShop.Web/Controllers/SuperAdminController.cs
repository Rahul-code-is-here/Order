using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
// using AspNetCoreHero.ToastNotification.Abstractions;
using BussinessLogicLayer.Interface;
// using DataAccessLayer.ViewModels;
using PizzaShop.Domain.ViewModels;
using PizzaShop.Repository.Interface;
using PizzaShop.Domain.DataModels;
using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore.Metadata.Internal;t
using PizzaShop.Domain.ViewModels;
using PizzaShop.Service.Interface;
// using PizzaShopRMS.Helpers;

namespace PizzaShopRMS.Controllers;


public class SuperAdminController : Controller
{
    private readonly ISuperAdminRepository _superAdminRepository;

    // private readonly ISuperAdminService _superAdminService;

    private readonly IUserServices _userServices;

    private readonly IWebHostEnvironment _webHostEnvironmnet;//required to access wwwrootfolder directly

    public SuperAdminController(ISuperAdminRepository superAdminService, IWebHostEnvironment webHostEnvironmnet, IUserServices userServices)
    {

        _superAdminRepository = superAdminService;
        _webHostEnvironmnet = webHostEnvironmnet;
        _userServices = userServices;
     
    }

    private async Task SetUserProfileInViewBag()
    {
        var token = Request.Cookies["AuthToken"];
        var email = _userServices.ExtractEmailFromToken(token);

        if (string.IsNullOrEmpty(email))
        {
            // Ensure the correct login page URL
            HttpContext.Response.Redirect("/Login", true);
            return;
        }

        var userProfile = await _userServices.GetUserProfileAsync(email);
        if (userProfile != null)
        {
            ViewBag.UserName = userProfile.UserName;
            ViewBag.UserImage = userProfile.PathOfProfilePicture;
        }
    }

    //Displays the menu page
    public async Task<IActionResult> Menu()
    {

          await SetUserProfileInViewBag();

        var token = Request.Cookies["AuthToken"];
        var email = _userServices.ExtractEmailFromToken(token);


        bool isFirstLogin = await _userServices.IsFirstLoginAsync(email);
        if (isFirstLogin)
        {
            return RedirectToAction("ChangePassword", "Login");
        }

        ViewData["ActivePage"] = "Menu";
        MenuViewModel menu = new MenuViewModel();
        menu.CategoryList = await _superAdminRepository.GetCategoryList();
        menu.ModifiersList = await _superAdminRepository.GetModifiersList();
        menu.Units = await _superAdminRepository.GetUnits();

        //  menu.ItemList = await _superAdminRepository.GetItemsList(new Pagination<ItemsViewModel> { CurrentPage = 1, PageSize = 10 }, null);

        return View(menu);
    }

    // Handles adding a new category
    [HttpPost]

    public async Task<IActionResult> AddCategory(AddCategoryViewModel addCategoryViewModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {

                return RedirectToAction("Menu", "SuperAdmin");
            }
            // var token = Request.Cookies["JwtToken"];

            // var userId = decodedToken.UserId;
            // addCategoryViewModel.CreatedBy = userId;
            var result = await _superAdminRepository.AddCategory(addCategoryViewModel);
            if (result == "Category Added Successfully")
            {
                TempData["Success"] = result;

                return RedirectToAction("Menu", "SuperAdmin");
            }
            TempData["error"] = result;
            
            return RedirectToAction("Menu", "SuperAdmin");

        }
        catch (Exception ex)
        {
            TempData["error"] = "An Unexpected error got occured";

            Console.WriteLine($"Error from controller :{ex.Message}");
            return RedirectToAction("Menu", "SuperAdmin");
        }
    }

    // Retrieves a category for editing

    public async Task<IActionResult> GetCategoryForEdit(string id)
    {
        try
        {
            var category = await _superAdminRepository.GetCategory(id);
            return Json(category);
        }
        catch (Exception ex)
        {
            TempData["error"] = "An Unexpected error got occured";

            Console.WriteLine($"Error from controller :{ex.Message}");
            return RedirectToAction("Menu", "SuperAdmin");
        }
    }

    //Handles editing an existing category
    [HttpPost]

    public async Task<IActionResult> EditCategoryPost(AddCategoryViewModel addCategoryViewModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Model state is not valid";
                return RedirectToAction("Menu", "SuperAdmin");
            }
            // var token = Request.Cookies["JwtToken"];

            // var userId = decodedToken.UserId;
            // addCategoryViewModel.EditedBy = userId;
            var result = await _superAdminRepository.EditCategory(addCategoryViewModel);
            if (result == "Category Edited Successfully")
            {
                 TempData["Success"] = result;
                return RedirectToAction("Menu", "SuperAdmin");
            }

            return RedirectToAction("Menu", "SuperAdmin");

        }
        catch (Exception ex)
        {
            TempData["error"] = "Error while editing Category";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return RedirectToAction("Menu", "SuperAdmin");
        }
    }

    //Handles deleting a category

    public async Task<IActionResult> DeleteCategory(string id)
    {
        try
        {
            var result = await _superAdminRepository.DeleteCategory(id);
            if (result == "Category Deleted Successfully")
            {
                TempData["Success"] = result;

                return RedirectToAction("Menu", "SuperAdmin");
            }
            TempData["error"] = result;

            return RedirectToAction("Menu", "SuperAdmin");

        }
        catch (Exception ex)
        {
            // _notfy.Error("An Unexpected error got occured");
            Console.WriteLine($"Error from controller :{ex.Message}");
            return RedirectToAction("Menu", "SuperAdmin");
        }
    }

    // Fetches a paginated list of items
    public async Task<IActionResult> ItemsListPartialView(Pagination<ItemsViewModel> itemsList, string CategoryId)
    {
        ViewData["ActivePage"] = "Menu";
        try
        {
            var data = await _superAdminRepository.GetItemsList(itemsList, CategoryId);
            return PartialView("_ItemsPartialView", data);
        }
        catch (Exception ex)
        {
            TempData["error"] = "An Unexpected error got occured";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return RedirectToAction("Menu", "SuperAdmin");
        }
    }

    // Handles adding a new item
    [HttpPost]

    // public async Task<JsonResult> AddItemPost(AddItemsViewModel addItemsViewModel)
    // {
    //     try
    //     {
    //         if (!ModelState.IsValid)
    //         {
    //             return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });
    //         }
    //         var token = Request.Cookies["JwtToken"];
    //         // var userId = decodedToken.UserId;
    //         // addItemsViewModel.CreatedBy = userId;
    //         string uploadFolder = Path.Combine(_webHostEnvironmnet.WebRootPath, "uploads");
    //         var result = await _superAdminRepository.AddItems(addItemsViewModel, uploadFolder);

    //         switch (result)
    //         {
    //             case "provide unique shortcode":
    //             case "Items already exist":
    //             case "Only jpg, png, jpeg images are allowed":
    //             case "Image must be less than 3MB":
    //                 TempData["error"] = result; // Store message

    //                 return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });

    //             default:
    //                 TempData["success"] = result; // Store success message

    //                 return Json(new { success = true, redirectUrl = "/SuperAdmin/Menu" });
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         TempData["error"] = ex.Message;

    //         return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });
    //     }
    // }

    [HttpPost]
public async Task<IActionResult> AddItemPost(AddItemsViewModel addItemsViewModel)
{
    try
    {
        // if (!ModelState.IsValid)
        // {
        //     return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });
        // }
        var token = Request.Cookies["JwtToken"];
        string uploadFolder = Path.Combine(_webHostEnvironmnet.WebRootPath, "uploads");
        var result = await _superAdminRepository.AddItems(addItemsViewModel, uploadFolder);

        switch (result)
        {
            case "provide unique shortcode":
            case "Items already exist":
            case "Only jpg, png, jpeg images are allowed":
            case "Image must be less than 3MB":
                TempData["error"] = result; // Store message
                return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });

            default:
                TempData["success"] = result; // Store success message
                
                // Get the category ID from the view model
                string categoryId = addItemsViewModel.CategoryId.ToString();
                
                // Get the updated items list for this category
                var pagination = new Pagination<ItemsViewModel> { CurrentPage = 1, PageSize = 10 };
                var itemsList = await _superAdminRepository.GetItemsList(pagination, categoryId);
                
                // Return the partial view as a string
                return PartialView("_ItemsPartialView", itemsList);
        }
    }
    catch (Exception ex)
    {
        TempData["error"] = ex.Message;
        return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });
    }
}

    //Retrieves an item for editing
    public async Task<IActionResult> GetItemForEdit(string id)
    {
        try
        {

            var items = await _superAdminRepository.GetItemForEdit(id);
            return Json(new { success = true, items, redirectUrl = "/SuperAdmin/Menu" });
        }
        catch (Exception ex)
        {
            TempData["error"] = ex.Message;


            return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });
        }
    }

    //Handles editing an existing item
    [HttpPost]

    public async Task<IActionResult> EditItemPost(EditItemsViewModel editItemsViewModel)
    {

        try
        {
            var token = Request.Cookies["JwtToken"];
            // var userId = decodedToken.UserId;
            // editItemsViewModel.EditedBy = userId;
            string uploadFolder = Path.Combine(_webHostEnvironmnet.WebRootPath, "uploads");
            var result = await _superAdminRepository.EditItem(editItemsViewModel, uploadFolder);
            switch (result)
            {
                case "provide unique shortcode":
                case "Item with the name already exist":
                case "Only jpg, png, jpeg images are allowed":
                case "Image must be less than 3MB":
                    TempData["error"] = result; // Store message
                    return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });

                default:
                    TempData["success"] = result; // Store success message

                    return Json(new { success = true, redirectUrl = "/SuperAdmin/Menu" });
            }

        }
        catch (Exception ex)
        {
            TempData["error"] = ex.Message;

            return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });
        }
    }
    //Handles deleting an item
    [HttpPost]

    public async Task<IActionResult> DeleteItems(List<string> itemIds)
    {
        try
        {
            var result = await _superAdminRepository.DeleteItem(itemIds);
            TempData["success"] = result;
            return Json(new { success = true, redirectUrl = "/SuperAdmin/Menu" });
        }
        catch (Exception ex)
        {
            TempData["error"] = "An Unexpected error got occured";

            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });
        }
    }
    [HttpPost]

    public async Task<IActionResult> AddModifiers(ModifiersViewModel AddModifiers)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Model state is not valid";


                return Json(new { success = false, activeTab = "nav-item-tab", redirectUrl = "/SuperAdmin/Menu" });
            }
            var token = Request.Cookies["JwtToken"];

            // var userId = decodedToken.UserId;
            // AddModifiers.CreatedBy = userId;
            var result = await _superAdminRepository.AddModifier(AddModifiers);
            if (result == "Modifier Added Successfully")
            {
                TempData["success"] = result;

                return Json(new { success = true, activeTab = "nav-item-tab", redirectUrl = "/SuperAdmin/Menu" });
            }
            TempData["error"] = result;

            return Json(new { success = true, activeTab = "nav-item-tab", redirectUrl = "/SuperAdmin/Menu" });
        }
        catch (Exception ex)
        {
            TempData["error"] = ex.Message;

            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = true, activeTab = "nav-item-tab", redirectUrl = "/SuperAdmin/Menu" });
        }
    }

    public async Task<IActionResult> GetModifierItemsList(Pagination<ModifierItemsViewModel> modifierItemList, string ModifierGroupId)
    {
        try
        {
            var data = await _superAdminRepository.GetModifierItemsList(modifierItemList, ModifierGroupId);
            return PartialView("_ModifiersItemsPartialView", data);
        }
        catch (Exception ex)
        {
            TempData["error"] = "An Unexpected error got occured";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = true, redirectUrl = "/SuperAdmin/Menu" });
        }
    }

    public async Task<IActionResult> GetAllModifierItemList(Pagination<ModifierItemsViewModel> modifierItemList)
    {
        try
        {
            var data = await _superAdminRepository.GetAllModifierItemsList(modifierItemList);
            return PartialView("_ExistingModifiersPartailView", data);
        }
        catch (Exception ex)
        {
            TempData["error"] = "An Unexpected error got occured";

            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = true, redirectUrl = "/SuperAdmin/Menu" });
        }
    }

    public async Task<IActionResult> GetModifierForEdit(string id)
    {
        try
        {
            var modifier = await _superAdminRepository.GetModifierForEdit(id);
            return Json(modifier);
        }
        catch (Exception ex)
        {
            TempData["error"] = "An Unexpected error got occured";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = true, redirectUrl = "/SuperAdmin/Menu" });
        }
    }
    [HttpPost]
    public async Task<IActionResult> EditModifierGroupPost(ModifiersViewModel AddModifiers)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Model state is not valid";

                return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });
            }
            var token = Request.Cookies["JwtToken"];
            // var userId = decodedToken.UserId;
            // AddModifiers.EditedBy = userId;
            var result = await _superAdminRepository.EditModifier(AddModifiers);
            switch (result)
            {
                case "Modifier with the name already exist":
                    TempData["error"] = result;

                    return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });
                default:
                    TempData["success"] = result;

                    return Json(new { success = true, redirectUrl = "/SuperAdmin/Menu" });
            }
        }
        catch (Exception ex)
        {
            TempData["error"] = "An Unexpected error got occured";

            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = true, redirectUrl = "/SuperAdmin/Menu" });
        }
    }
    public async Task<IActionResult> DeleteModifierGroup(string id)
    {
        try
        {
            var result = await _superAdminRepository.DeleteModifier(id);
            TempData["success"] = result;

            return Json(new { success = true, redirectUrl = "/SuperAdmin/Menu" });
        }
        catch (Exception ex)
        {
            TempData["error"] = "An Unexpected error got occured";

            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });
        }
    }
    [HttpPost]
    public async Task<IActionResult> AddModifierItem(AddModifierItemViewModel addModifierItemViewModel)
    {
        try
        {

            var token = Request.Cookies["JwtToken"];
            // var userId = decodedToken.UserId;
            // addModifierItemViewModel.CreatedBy = userId;
            var result = await _superAdminRepository.AddModifierItem(addModifierItemViewModel);
            if (result == "Modifier Item Added Successfully")
            {
                TempData["Success"] = result;

                return Json(new { success = true, redirectUrl = "/SuperAdmin/Menu" });
            }
            TempData["Success"] = result;

            return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });

        }
        catch (Exception ex)
        {
            TempData["error"] = "An Unexpected error got occured";

            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = true, redirectUrl = "/SuperAdmin/Menu" });
        }
    }

    public async Task<IActionResult> GetModifierItemForEdit(string id)
    {
        try
        {
            var modifierItem = await _superAdminRepository.GetModifierItemForEdit(id);
            return Json(modifierItem);
        }
        catch (Exception ex)
        {
            TempData["error"] = "An Unexpected error got occured";

            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = true, redirectUrl = "/SuperAdmin/Menu" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> EditModifierPost(AddModifierItemViewModel addModifierItemViewModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Model state is not valid";

                return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });

            }
            var token = Request.Cookies["JwtToken"];
            // var decodedToken = await _commonRepository.ValidateToken(token);
            // var userId = decodedToken.UserId;
            // addModifierItemViewModel.EditedBy = userId;
            var result = await _superAdminRepository.EditModifierItem(addModifierItemViewModel);
            switch (result)
            {
                case "Modifier item with the name already exist":
                    TempData["error"] = result;

                    return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });
                default:
                    TempData["Success"] = result;

                    return Json(new { success = true, redirectUrl = "/SuperAdmin/Menu" });
            }
        }
        catch (Exception ex)
        {
            TempData["error"] = "An Unexpected error got occured";

            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = true, redirectUrl = "/SuperAdmin/Menu" });
        }
    }
    public async Task<IActionResult> DeleteModifierItems(List<ModifierItemsViewModel> modifierItemsViewModels)
    {
        try
        {
            var result = await _superAdminRepository.DeleteModifierItem(modifierItemsViewModels);
            TempData["Success"] = result;

            return Json(new { success = true, redirectUrl = "/SuperAdmin/Menu" });
        }
        catch (Exception ex)
        {
            TempData["error"] = "An Unexpected error got occured";

            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });
        }
    }
    // for getting modifierlist for items based on modifiergroup
    public async Task<IActionResult> ModifierList(string ModifierGroupId)
    {
        try
        {
            var result = await _superAdminRepository.GetModifierListForItems(ModifierGroupId);
            return PartialView("_ModifierForItem", result);
        }
        catch (Exception ex)
        {
            TempData["error"] = "An Unexpected error got occured";

            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });
        }
    }

    public async Task<IActionResult> FetchingModifierListforEditItem(string ModifierId, string ItemId)
    {
        try
        {
            var result = await _superAdminRepository.GetModifiersforItemsForEdit(ModifierId, ItemId);
            return PartialView("_ModifierForItem", result);
        }
        catch (Exception ex)
        {
            TempData["error"] = "An Unexpected error got occured";

            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });
        }
    }
}

