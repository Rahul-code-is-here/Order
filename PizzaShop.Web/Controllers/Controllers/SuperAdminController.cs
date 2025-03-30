using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using BussinessLogicLayer.Interface;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PizzaShopRMS.Helpers;

namespace PizzaShopRMS.Controllers;

// [CustomAuthorise(new string[] { "SuperAdmin" })]
public class SuperAdminController : Controller
{
    private readonly ISuperAdminRepository _superAdminRepository;

    private readonly ICommonRepository _commonRepository;

    private readonly INotyfService _notfy;
    private readonly IWebHostEnvironment _webHostEnvironmnet;//required to access wwwrootfolder directly

    public SuperAdminController(INotyfService notfy, ISuperAdminRepository superAdminService, ICommonRepository commonRepository, IWebHostEnvironment webHostEnvironmnet)
    {
        _notfy = notfy;
        _superAdminRepository = superAdminService;
        _webHostEnvironmnet = webHostEnvironmnet;
        _commonRepository = commonRepository;
    }

    //Dashboard view
    [CustomAuthorise(new string[] { "SuperAdmin"},"dashboard","dashboard")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> Dashboard()
    {
        ViewData["ActivePage"] = "Dashboard";
        return View();
    }

    //Fetches a paginated list of user
    // [HttpGet]
    // [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    // public async Task<IActionResult> UserList(Pagination<UserListViewModel> userList)
    // {
    //     ViewData["ActivePage"] = "User";
    //     try
    //     {
    //         var token = Request.Cookies["JwtToken"];
    //         var decodedToken = await _commonRepository.ValidateToken(token);
    //         var userEmail = decodedToken.UserEmail;
    //         var data = await _superAdminRepository.GetUserList(userList, userEmail);
    //         Console.WriteLine(data.NumberOfItems);
    //         return View(data);

    //     }
    //     catch (Exception ex)
    //     {
    //         _notfy.Error(ex.Message);
    //         return RedirectToAction("Dashboard", "SuperAdmin");
    //     }
    // }

    // //Fetches a partial view for the user list
    // [HttpGet]
    // [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    // public async Task<IActionResult> UserListPartialView(Pagination<UserListViewModel> userList)
    // {
    //     ViewData["ActivePage"] = "User";
    //     try
    //     {
    //         var token = Request.Cookies["JwtToken"];
    //         var decodedToken = await _commonRepository.ValidateToken(token);
    //         var userEmail = decodedToken.UserEmail;
    //         var data = await _superAdminRepository.GetUserList(userList, userEmail);
    //         return PartialView("_PartialUserData", data);
    //     }
    //     catch (Exception ex)
    //     {
    //         _notfy.Error(ex.Message);
    //         return RedirectToAction("Dashboard", "SuperAdmin");
    //     }
    // }

    // //Displays the form to add a new user
    // [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    // public async Task<IActionResult> AddNewUser()
    // {
    //     ViewData["ActivePage"] = "User";
    //     return View();
    // }

    // //Handles form submission to add a new user
    // [HttpPost]
    // [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    // public async Task<IActionResult> AddNewUserPost(AddNewUserViewModel addNewUserViewModel)
    // {
    //     ViewData["ActivePage"] = "User";
    //     try
    //     {
    //         if (!ModelState.IsValid)
    //         {
    //             return View("AddNewUser", addNewUserViewModel);
    //         }
    //         string uploadFolder = Path.Combine(_webHostEnvironmnet.WebRootPath, "uploads");
    //         var result = await _superAdminRepository.AddNewUser(addNewUserViewModel, uploadFolder);
    //         switch (result)
    //         {
    //             case "UserName already taken":
    //             case "phone number is already registered":
    //             case "User with email already present":
    //             case "Only jpg, png,jpeg  images are allowed":
    //             case "Image must be less than 3MB":
    //                 TempData["ToasterMessage"] = result;
    //                 TempData["ToasterType"] = "error";
    //                 return Json(new { success = false, redirectUrl = "/SuperAdmin/AddNewUser" });
    //             default:
    //                 TempData["ToasterMessage"] = result;
    //                 TempData["ToasterType"] = "success";
    //                 return Json(new { success = true, redirectUrl = "/SuperAdmin/UserList" });
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine($"error from Controller : {ex.Message}");
    //         TempData["ToasterMessage"] = "An Unexpected error got occured,Please try again";
    //         TempData["ToasterType"] = "error";
    //         return Json(new { success = false, redirectUrl = "/SuperAdmin/UserList" });

    //     }
    // }

    // //Displays the form to edit a user
    // [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    // public async Task<IActionResult> EditUser(string email)
    // {
    //     ViewData["ActivePage"] = "User";
    //     try
    //     {
    //         var userInfo = await _superAdminRepository.GetUserForEdit(email);
    //         return View(userInfo);
    //     }
    //     catch (Exception ex)
    //     {
    //         _notfy.Error("An Unexpected error got occured,Please try again");
    //         Console.WriteLine($"error from Controller : {ex.Message}");
    //         return RedirectToAction("EditUser", "SuperAdmin", new { email = email });
    //     }
    // }

    // //Handles form submission to update user details
    // [HttpPost]
    // [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    // public async Task<IActionResult> EditUserPost(EditUserViewModel editUserViewModel)
    // {
    //     ViewData["ActivePage"] = "User";
    //     try
    //     {
    //         if (!ModelState.IsValid)
    //         {
    //             return Json(new { success = false, redirectUrl = "/SuperAdmin/EditUser?email=" + editUserViewModel.Email });
    //         }
    //         string uploadFolder = Path.Combine(_webHostEnvironmnet.WebRootPath, "uploads");
    //         var result = await _superAdminRepository.EditUser(editUserViewModel, uploadFolder);
    //         switch (result)
    //         {
    //             case "UserName already taken":
    //             case "phone number is already registered":
    //             case "Only jpg, png,jpeg  images are allowed":
    //             case "Image must be less than 3MB":
    //                 TempData["ToasterMessage"] = result;
    //                 TempData["ToasterType"] = "error";
    //                 return Json(new { success = false, redirectUrl = "/SuperAdmin/EditUser?email=" + editUserViewModel.Email });
    //             default:
    //                 TempData["ToasterMessage"] = result; 
    //                 TempData["ToasterType"] = "success";
    //                 return Json(new { success = true, redirectUrl = "/SuperAdmin/UserList" });
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine($"error from Controller : {ex.Message}");
    //         TempData["ToasterMessage"] = "An Unexpected error got occured,Please try again";
    //         TempData["ToasterType"] = "error";
    //         return Json(new { success = false, redirectUrl = "/SuperAdmin/UserList" });
    //     }
    // }

    // //Deletes a user by email
    // [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    // public async Task<IActionResult> DeleteUser(string email)
    // {
    //     ViewData["ActivePage"] = "User";
    //     try
    //     {
    //         var result = await _superAdminRepository.DeleteUser(email);
    //         if (result == "user deleted successfully")
    //         {
    //             _notfy.Success(result);
    //             return RedirectToAction("UserList", "SuperAdmin");
    //         }
    //         _notfy.Error(result);
    //         return RedirectToAction("UserList", "SuperAdmin");
    //     }
    //     catch (Exception ex)
    //     {
    //         _notfy.Error("An Unexpected error got occured,Please try again");
    //         Console.WriteLine($"error from Controller : {ex.Message}");
    //         return RedirectToAction("UserList", "SuperAdmin");
    //     }
    // }

    //  Displays the roles page
    // [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    // public IActionResult Roles()
    // {
    //     ViewData["ActivePage"] = "Role";
    //     return View();
    // }
    // [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]

    // // Fetches permissions for a role
    // public async Task<IActionResult> Permissions(string role)
    // {
    //     ViewData["ActivePage"] = "Role";
    //     ViewBag.role = role;
    //     try
    //     {
    //         var permissions = await _superAdminRepository.GetPermissions(role);
    //         return View(permissions);
    //     }
    //     catch (Exception ex)
    //     {
    //         _notfy.Error("An Unexpected error got occured");
    //         Console.WriteLine($"Error from controller :{ex.Message}");
    //         return RedirectToAction("Permissions", "SuperAdmin", role);
    //     }
    // }

    // //Handles permission updates for a role
    // [HttpPost]
    // [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    // public async Task<IActionResult> PermissionPost(RolePermissionViewModel rolePermissionViewModel)
    // {
    //     try
    //     {
    //         var token = Request.Cookies["JwtToken"];
    //         var decodedToken = await _commonRepository.ValidateToken(token);
    //         var userId = decodedToken.UserId;
    //         if (!ModelState.IsValid)
    //         {
    //             return RedirectToAction("Permissions", "SuperAdmin", new { role = rolePermissionViewModel.Role });
    //         }
    //         rolePermissionViewModel.EditedBy = userId;
    //         var result = await _superAdminRepository.UpdatePermissions(rolePermissionViewModel);
    //         if (result == "Permissions Updated Sucessfully")
    //         {
    //             _notfy.Success("permissions updated Successfully");
    //             return RedirectToAction("Permissions", "SuperAdmin", new { role = rolePermissionViewModel.Role });
    //         }
    //         return RedirectToAction("Permissions", "SuperAdmin", new { role = rolePermissionViewModel.Role });
    //     }
    //     catch (Exception ex)
    //     {
    //         _notfy.Error("An Unexpected error got occured");
    //         Console.WriteLine($"Error from controller :{ex.Message}");
    //         return RedirectToAction("Permissions", "SuperAdmin", new { role = rolePermissionViewModel.Role });
    //     }
    // }

    //Displays the menu page
//     [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
//     public async Task<IActionResult> Menu()
//     {
//         ViewData["ActivePage"] = "Menu";
//         MenuViewModel menu = new MenuViewModel();
//         menu.CategoryList = await _superAdminRepository.GetCategoryList();
//         menu.ModifiersList = await _superAdminRepository.GetModifiersList();
//         menu.Units = await _superAdminRepository.GetUnits();
//         return View(menu);
//     }

//     // Handles adding a new category
//     [HttpPost]
//     [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
//     public async Task<IActionResult> AddCategory(AddCategoryViewModel addCategoryViewModel)
//     {
//         try
//         {
//             if (!ModelState.IsValid)
//             {
//                 _notfy.Error("Category Name is Required");
//                 return RedirectToAction("Menu", "SuperAdmin");
//             }
//             var token = Request.Cookies["JwtToken"];
//             var decodedToken = await _commonRepository.ValidateToken(token);
//             var userId = decodedToken.UserId;
//             addCategoryViewModel.CreatedBy = userId;
//             var result = await _superAdminRepository.AddCategory(addCategoryViewModel);
//             if (result == "Category Added Successfully")
//             {
//                 _notfy.Success(result);
//                 return RedirectToAction("Menu", "SuperAdmin");
//             }
//             _notfy.Error(result);
//             return RedirectToAction("Menu", "SuperAdmin");

//         }
//         catch (Exception ex)
//         {
//             _notfy.Error("An Unexpected error got occured");
//             Console.WriteLine($"Error from controller :{ex.Message}");
//             return RedirectToAction("Menu", "SuperAdmin");
//         }
//     }

//     // Retrieves a category for editing
//     [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
//     public async Task<IActionResult> GetCategoryForEdit(string id)
//     {
//         try
//         {
//             var category = await _superAdminRepository.GetCategory(id);
//             return Json(category);
//         }
//         catch (Exception ex)
//         {
//             _notfy.Error("An Unexpected error got occured");
//             Console.WriteLine($"Error from controller :{ex.Message}");
//             return RedirectToAction("Menu", "SuperAdmin");
//         }
//     }

//     //Handles editing an existing category
//     [HttpPost]
//     [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
//     public async Task<IActionResult> EditCategoryPost(AddCategoryViewModel addCategoryViewModel)
//     {
//         try
//         {
//             if (!ModelState.IsValid)
//             {
//                 _notfy.Error("Category Name is Required");
//                 return RedirectToAction("Menu", "SuperAdmin");
//             }
//             var token = Request.Cookies["JwtToken"];
//             var decodedToken = await _commonRepository.ValidateToken(token);
//             var userId = decodedToken.UserId;
//             addCategoryViewModel.EditedBy = userId;
//             var result = await _superAdminRepository.EditCategory(addCategoryViewModel);
//             if (result == "Category Edited Successfully")
//             {
//                 _notfy.Success(result);
//                 return RedirectToAction("Menu", "SuperAdmin");
//             }
//             _notfy.Error(result);
//             return RedirectToAction("Menu", "SuperAdmin");

//         }
//         catch (Exception ex)
//         {
//             _notfy.Error("An Unexpected error got occured");
//             Console.WriteLine($"Error from controller :{ex.Message}");
//             return RedirectToAction("Menu", "SuperAdmin");
//         }
//     }

//     //Handles deleting a category
//     [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
//     public async Task<IActionResult> DeleteCategory(string id)
//     {
//         try
//         {
//             var result = await _superAdminRepository.DeleteCategory(id);
//             if (result == "Category Deleted Successfully")
//             {
//                 _notfy.Success(result);
//                 return RedirectToAction("Menu", "SuperAdmin");
//             }
//             _notfy.Error(result);
//             return RedirectToAction("Menu", "SuperAdmin");

//         }
//         catch (Exception ex)
//         {
//             _notfy.Error("An Unexpected error got occured");
//             Console.WriteLine($"Error from controller :{ex.Message}");
//             return RedirectToAction("Menu", "SuperAdmin");
//         }
//     }

//     // Fetches a paginated list of items
//     [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
//     public async Task<IActionResult> ItemsListPartialView(Pagination<ItemsViewModel> itemsList, string CategoryId)
//     {
//         ViewData["ActivePage"] = "Menu";
//         try
//         {
//             var data = await _superAdminRepository.GetItemsList(itemsList, CategoryId);
//             return PartialView("_ItemsPartialView", data);
//         }
//         catch (Exception ex)
//         {
//             _notfy.Error(ex.Message);
//             return RedirectToAction("Dashboard", "SuperAdmin");
//         }
//     }

//     // Handles adding a new item
//     [HttpPost]
//     [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
//     public async Task<JsonResult> AddItemPost(AddItemsViewModel addItemsViewModel)
//     {
//         try
//         {
//             if (!ModelState.IsValid)
//             {
//                 return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });
//             }
//             var token = Request.Cookies["JwtToken"];
//             var decodedToken = await _commonRepository.ValidateToken(token);
//             var userId = decodedToken.UserId;
//             addItemsViewModel.CreatedBy = userId;
//             string uploadFolder = Path.Combine(_webHostEnvironmnet.WebRootPath, "uploads");
//             var result = await _superAdminRepository.AddItems(addItemsViewModel, uploadFolder);

//             switch (result)
//             {
//                 case "provide unique shortcode":
//                 case "Items already exist":
//                 case "Only jpg, png, jpeg images are allowed":
//                 case "Image must be less than 3MB":
//                     TempData["ToasterMessage"] = result; // Store message
//                     TempData["ToasterType"] = "error";   // Error type
//                     return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });

//                 default:
//                     TempData["ToasterMessage"] = result; // Store success message
//                     TempData["ToasterType"] = "success"; // Success type
//                     return Json(new { success = true, redirectUrl = "/SuperAdmin/Menu" });
//             }
//         }
//         catch (Exception ex)
//         {
//             TempData["ToasterMessage"] = ex.Message;
//             TempData["ToasterType"] = "error";
//             return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });
//         }
//     }

//     //Retrieves an item for editing
//     [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
//     public async Task<IActionResult> GetItemForEdit(string id)
//     {
//         try
//         {

//             var items = await _superAdminRepository.GetItemForEdit(id);
//             return Json(new { success = true, items, redirectUrl = "/SuperAdmin/Menu" });
//         }
//         catch (Exception ex)
//         {
//             TempData["ToasterMessage"] = ex.Message;
//             TempData["ToasterType"] = "error";
//             return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });
//         }
//     }
//     //Handles editing an existing item
//     [HttpPost]
//     [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
//     public async Task<IActionResult> EditItemPost(EditItemsViewModel editItemsViewModel)
//     {
//         // Thread.Sleep(5000);
//         try
//         {
//             var token = Request.Cookies["JwtToken"];
//             var decodedToken = await _commonRepository.ValidateToken(token);
//             var userId = decodedToken.UserId;
//             editItemsViewModel.EditedBy = userId;
//             string uploadFolder = Path.Combine(_webHostEnvironmnet.WebRootPath, "uploads");
//             var result = await _superAdminRepository.EditItem(editItemsViewModel, uploadFolder);
//             switch (result)
//             {
//                 case "provide unique shortcode":
//                 case "Item with the name already exist":
//                 case "Only jpg, png, jpeg images are allowed":
//                 case "Image must be less than 3MB":
//                     TempData["ToasterMessage"] = result; // Store message
//                     TempData["ToasterType"] = "error";   // Error type
//                     return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });

//                 default:
//                     TempData["ToasterMessage"] = result; // Store success message
//                     TempData["ToasterType"] = "success"; // Success type
//                     return Json(new { success = true, redirectUrl = "/SuperAdmin/Menu" });
//             }

//         }
//         catch (Exception ex)
//         {
//             TempData["ToasterMessage"] = ex.Message;
//             TempData["ToasterType"] = "error";
//             return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });
//         }
//     }
//     //Handles deleting an item
//     [HttpPost]
//     [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
//     public async Task<IActionResult> DeleteItems(List<string> itemIds)
//     {
//         try
//         {
//             var result = await _superAdminRepository.DeleteItem(itemIds);
//             TempData["ToasterMessage"] = result;
//             TempData["ToasterType"] = "success";
//             return Json(new { success = true, redirectUrl = "/SuperAdmin/Menu" });
//         }
//         catch (Exception ex)
//         {
//             TempData["ToasterMessage"] = "An Unexpected error got occured";
//             TempData["ToasterType"] = "error";
//             Console.WriteLine($"Error from controller :{ex.Message}");
//             return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });
//         }
//     }
//     [HttpPost]
//     [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
//     public async Task<IActionResult> AddModifiers(ModifiersViewModel AddModifiers)
//     {
//         try
//         {
//             if (!ModelState.IsValid)
//             {
//                 TempData["ToasterMessage"] = "Model state is not valid";
//                 TempData["ToasterType"] = "error";
//                 return Json(new { success = false, activeTab = "nav-item-tab", redirectUrl = "/SuperAdmin/Menu" });
//             }
//             var token = Request.Cookies["JwtToken"];
//             var decodedToken = await _commonRepository.ValidateToken(token);
//             var userId = decodedToken.UserId;
//             AddModifiers.CreatedBy = userId;
//             var result = await _superAdminRepository.AddModifier(AddModifiers);
//             if (result == "Modifier Added Successfully")
//             {
//                 TempData["ToasterMessage"] = result;
//                 TempData["ToasterType"] = "success";
//                 return Json(new { success = true, activeTab = "nav-item-tab", redirectUrl = "/SuperAdmin/Menu" });
//             }
//             TempData["ToasterMessage"] = result;
//             TempData["ToasterType"] = "error";
//             return Json(new { success = true, activeTab = "nav-item-tab", redirectUrl = "/SuperAdmin/Menu" });
//         }
//         catch (Exception ex)
//         {
//             TempData["ToasterMessage"] = "An Unexpected error got occured";
//             TempData["ToasterType"] = "error";
//             Console.WriteLine($"Error from controller :{ex.Message}");
//             return Json(new { success = true, activeTab = "nav-item-tab", redirectUrl = "/SuperAdmin/Menu" });
//         }
//     }

//     [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
//     public async Task<IActionResult> GetModifierItemsList(Pagination<ModifierItemsViewModel> modifierItemList, string ModifierGroupId)
//     {
//         try
//         {
//             var data = await _superAdminRepository.GetModifierItemsList(modifierItemList, ModifierGroupId);
//             return PartialView("_ModifiersItemsPartialView", data);
//         }
//         catch (Exception ex)
//         {

//             TempData["ToasterMessage"] = "An Unexpected error got occured";
//             TempData["ToasterType"] = "error";
//             Console.WriteLine($"Error from controller :{ex.Message}");
//             return Json(new { success = true, redirectUrl = "/SuperAdmin/Menu" });
//         }
//     }
    
//     [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
//     public async Task<IActionResult> GetAllModifierItemList(Pagination<ModifierItemsViewModel> modifierItemList)
//     {
//         try
//         {
//             var data = await _superAdminRepository.GetAllModifierItemsList(modifierItemList);
//             return PartialView("_ExistingModifiersPartailView", data);
//         }
//         catch (Exception ex)
//         {

//             TempData["ToasterMessage"] = "An Unexpected error got occured";
//             TempData["ToasterType"] = "error";
//             Console.WriteLine($"Error from controller :{ex.Message}");
//             return Json(new { success = true, redirectUrl = "/SuperAdmin/Menu" });
//         }
//     }

//     [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
//     public async Task<IActionResult> GetModifierForEdit(string id)
//     {
//         try
//         {
//             var modifier = await _superAdminRepository.GetModifierForEdit(id);
//             return Json(modifier);
//         }
//         catch (Exception ex)
//         {
//             TempData["ToasterMessage"] = "An Unexpected error got occured";
//             TempData["ToasterType"] = "error";
//             Console.WriteLine($"Error from controller :{ex.Message}");
//             return Json(new { success = true, redirectUrl = "/SuperAdmin/Menu" });
//         }
//     }
//     [HttpPost]
//     [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
//     public async Task<IActionResult> EditModifierGroupPost(ModifiersViewModel AddModifiers){
//         try{
//            if (!ModelState.IsValid)
//             {
//                 TempData["ToasterMessage"] = "Model state is not valid";
//                 TempData["ToasterType"] = "error";
//                 return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });
//             }
//             var token = Request.Cookies["JwtToken"];
//             var decodedToken = await _commonRepository.ValidateToken(token);
//             var userId = decodedToken.UserId;
//             AddModifiers.EditedBy = userId;
//             var result = await _superAdminRepository.EditModifier(AddModifiers);
//             switch (result)
//             {
//                 case "Modifier with the name already exist":
//                     TempData["ToasterMessage"] = result; 
//                     TempData["ToasterType"] = "error";  
//                     return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });
//                 default:
//                     TempData["ToasterMessage"] = result; 
//                     TempData["ToasterType"] = "success"; 
//                     return Json(new { success = true, redirectUrl = "/SuperAdmin/Menu" });
//             }
//         }catch(Exception ex){
//             TempData["ToasterMessage"] = "An Unexpected error got occured";
//             TempData["ToasterType"] = "error";
//             Console.WriteLine($"Error from controller :{ex.Message}");
//             return Json(new { success = true, redirectUrl = "/SuperAdmin/Menu" });
//         }
//     }
//     public async Task<IActionResult> DeleteModifierGroup(string id)
//     {
//         try
//         {
//             var result = await _superAdminRepository.DeleteModifier(id);
//             TempData["ToasterMessage"] = result;
//             TempData["ToasterType"] = "success";
//             return Json(new { success = true, redirectUrl = "/SuperAdmin/Menu" });
//         }
//         catch (Exception ex)
//         {
//             TempData["ToasterMessage"] = "An Unexpected error got occured";
//             TempData["ToasterType"] = "error";
//             Console.WriteLine($"Error from controller :{ex.Message}");
//             return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });
//         }
//     }
//     [HttpPost]
//     [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
//     public async Task<IActionResult> AddModifierItem(AddModifierItemViewModel addModifierItemViewModel)
//     {
//         try
//         {
//             // if (!ModelState.IsValid)
//             // {
//             //     TempData["ToasterMessage"] = "Modifier Name is Required";
//             //     TempData["ToasterType"] = "error";
//             //     return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });
//             // }
//             var token = Request.Cookies["JwtToken"];
//             var decodedToken = await _commonRepository.ValidateToken(token);
//             var userId = decodedToken.UserId;
//             addModifierItemViewModel.CreatedBy = userId;
//             var result = await _superAdminRepository.AddModifierItem(addModifierItemViewModel);
//             if (result == "Modifier Item Added Successfully")
//             {
//                 TempData["ToasterMessage"] = result;
//                 TempData["ToasterType"] = "success";
//                 return Json(new { success = true, redirectUrl = "/SuperAdmin/Menu" });
//             }
//             TempData["ToasterMessage"] = result;
//             TempData["ToasterType"] = "error";
//             return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });

//         }
//         catch (Exception ex)
//         {
//             TempData["ToasterMessage"] = "An Unexpected error got occured";
//             TempData["ToasterType"] = "error";
//             Console.WriteLine($"Error from controller :{ex.Message}");
//             return Json(new { success = true, redirectUrl = "/SuperAdmin/Menu" });
//         }
//     }

//     [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
//     public async Task<IActionResult> GetModifierItemForEdit(string id)
//     {
//         try
//         {
//             var modifierItem = await _superAdminRepository.GetModifierItemForEdit(id);
//             return Json(modifierItem);
//         }
//         catch (Exception ex)
//         {
//             TempData["ToasterMessage"] = "An Unexpected error got occured";
//             TempData["ToasterType"] = "error";
//             Console.WriteLine($"Error from controller :{ex.Message}");
//             return Json(new { success = true, redirectUrl = "/SuperAdmin/Menu" });
//         }
//     }

//     [HttpPost]
//     [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
//     public async Task<IActionResult> EditModifierPost(AddModifierItemViewModel addModifierItemViewModel)
//     {
//         try
//         {
//             if (!ModelState.IsValid)
//             {
//                 TempData["ToasterMessage"] = "Model state is not valid";
//                 TempData["ToasterType"] = "error";
//                 return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });

//             }
//             var token = Request.Cookies["JwtToken"];
//             var decodedToken = await _commonRepository.ValidateToken(token);
//             var userId = decodedToken.UserId;
//             addModifierItemViewModel.EditedBy = userId;
//             var result = await _superAdminRepository.EditModifierItem(addModifierItemViewModel);
//             switch (result)
//             {
//                 case "Modifier item with the name already exist":
//                     TempData["ToasterMessage"] = result; // Store message
//                     TempData["ToasterType"] = "error";   // Error type
//                     return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });
//                 default:
//                     TempData["ToasterMessage"] = result; // Store success message
//                     TempData["ToasterType"] = "success"; // Success type
//                     return Json(new { success = true, redirectUrl = "/SuperAdmin/Menu" });
//             }
//         }
//         catch (Exception ex)
//         {
//             TempData["ToasterMessage"] = "An Unexpected error got occured";
//             TempData["ToasterType"] = "error";
//             Console.WriteLine($"Error from controller :{ex.Message}");
//             return Json(new { success = true, redirectUrl = "/SuperAdmin/Menu" });
//         }
//     }
//     public async Task<IActionResult> DeleteModifierItems(List<ModifierItemsViewModel> modifierItemsViewModels)
//     {
//         try
//         {
//             var result = await _superAdminRepository.DeleteModifierItem(modifierItemsViewModels);
//             TempData["ToasterMessage"] = result;
//             TempData["ToasterType"] = "success";
//             return Json(new { success = true, redirectUrl = "/SuperAdmin/Menu" });
//         }
//         catch (Exception ex)
//         {
//             TempData["ToasterMessage"] = "An Unexpected error got occured";
//             TempData["ToasterType"] = "error";
//             Console.WriteLine($"Error from controller :{ex.Message}");
//             return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });
//         }
//     }
//     // for getting modifierlist for items based on modifiergroup
//     public async Task<IActionResult> ModifierList(string ModifierGroupId)
//     {
//         try
//         {
//             var result = await _superAdminRepository.GetModifierListForItems(ModifierGroupId);
//             return PartialView("_ModifierForItem", result);
//         }
//         catch (Exception ex)
//         {
//             TempData["ToasterMessage"] = "An Unexpected error got occured";
//             TempData["ToasterType"] = "error";
//             Console.WriteLine($"Error from controller :{ex.Message}");
//             return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });
//         }
//     }

//     public async Task<IActionResult> FetchingModifierListforEditItem(string ModifierId,string ItemId){
//         try
//         {
//             var result = await _superAdminRepository.GetModifiersforItemsForEdit(ModifierId,ItemId);
//             return PartialView("_ModifierForItem", result);
//         }
//         catch (Exception ex)
//         {
//             TempData["ToasterMessage"] = "An Unexpected error got occured";
//             TempData["ToasterType"] = "error";
//             Console.WriteLine($"Error from controller :{ex.Message}");
//             return Json(new { success = false, redirectUrl = "/SuperAdmin/Menu" });
//         }
//     }
}

