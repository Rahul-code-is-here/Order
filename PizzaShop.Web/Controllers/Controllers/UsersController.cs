using AspNetCoreHero.ToastNotification.Abstractions;
using BussinessLogicLayer.Interface;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using PizzaShopRMS.Helpers;

namespace PizzaShopRMS.Controllers;

public class UsersController : Controller
{
    private readonly ICommonRepository _commonRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly INotyfService _notfy;
    private readonly IWebHostEnvironment _webHostEnvironmnet;//required to access wwwrootfolder directly
    public UsersController(IUsersRepository usersRepository, ICommonRepository commonRepository, INotyfService notfy, IWebHostEnvironment webHostEnvironmnet)
    {
        _notfy = notfy;
        _usersRepository = usersRepository;
        _webHostEnvironmnet = webHostEnvironmnet;
        _commonRepository = commonRepository;
    }
    //Fetches a paginated list of user
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "User", "View")]
    public async Task<IActionResult> UserList(Pagination<UserListViewModel> userList)
    {
        ViewData["ActivePage"] = "User";
        try
        {
            var token = Request.Cookies["JwtToken"];
            var decodedToken = await _commonRepository.ValidateToken(token);
            var roleId = decodedToken.Role;
            // if (!await _commonRepository.canView("User"))
            // {
            //     return RedirectToAction("AccessDenied", "Login");
            // }
            var userEmail = decodedToken.UserEmail;
            var data = await _usersRepository.GetUserList(userList, userEmail);
            Console.WriteLine(data.NumberOfItems);
            return View(data);

        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return RedirectToAction("Dashboard", "SuperAdmin");
        }
    }

    //Fetches a partial view for the user list
    [HttpGet]
    [CustomAuthorise(new string[] {"SuperAdmin", "AccountManager" }, "User", "View")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> UserListPartialView(Pagination<UserListViewModel> userList)
    {
        ViewData["ActivePage"] = "User";
        try
        {
            var token = Request.Cookies["JwtToken"];
            var decodedToken = await _commonRepository.ValidateToken(token);
            var roleId = decodedToken.Role;
            var userEmail = decodedToken.UserEmail;
            var data = await _usersRepository.GetUserList(userList, userEmail);
            return PartialView("_PartialUserData", data);
        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return RedirectToAction("Dashboard", "SuperAdmin");
        }
    }

    //Displays the form to add a new user
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "User", "AddOrEdit")]
    public async Task<IActionResult> AddNewUser()
    {

        try
        {
            var token = Request.Cookies["JwtToken"];
            var decodedToken = await _commonRepository.ValidateToken(token);
            var roleId = decodedToken.Role;
            // if (!await _commonRepository.canAddOrEdit("User"))
            // {
            //     return RedirectToAction("AccessDenied", "Login");
            // }
            ViewData["ActivePage"] = "User";
            return View();
        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return RedirectToAction("Dashboard", "SuperAdmin");
        }
    }

    [HttpPost]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "User", "AddOrEdit")]
    public async Task<IActionResult> AddNewUserPost(AddNewUserViewModel addNewUserViewModel)
    {
        ViewData["ActivePage"] = "User";
        try
        {
            var token = Request.Cookies["JwtToken"];
            var decodedToken = await _commonRepository.ValidateToken(token);
            var roleId = decodedToken.Role;
            // if (!await _commonRepository.canAddOrEdit("User"))
            // {
            //     return RedirectToAction("AccessDenied", "Login");
            // }
            if (!ModelState.IsValid)
            {
                return View("AddNewUser", addNewUserViewModel);
            }
            string uploadFolder = Path.Combine(_webHostEnvironmnet.WebRootPath, "uploads");
            var result = await _usersRepository.AddNewUser(addNewUserViewModel, uploadFolder);
            switch (result)
            {
                case "UserName already taken":
                case "phone number is already registered":
                case "User with email already present":
                case "Only jpg,png,jpeg  images are allowed":
                case "Image must be less than 3MB":
                    TempData["ToasterMessage"] = result;
                    TempData["ToasterType"] = "error";
                    return Json(new { success = false, redirectUrl = "/Users/AddNewUser" });
                default:
                    TempData["ToasterMessage"] = result;
                    TempData["ToasterType"] = "success";
                    return Json(new { success = true, redirectUrl = "/Users/UserList" });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error from Controller : {ex.Message}");
            TempData["ToasterMessage"] = "An Unexpected error got occured,Please try again";
            TempData["ToasterType"] = "error";
            return Json(new { success = false, redirectUrl = "/SuperAdmin/UserList" });

        }
    }

    //Displays the form to edit a user
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "User", "AddOrEdit")]
    public async Task<IActionResult> EditUser(string email)
    {
        ViewData["ActivePage"] = "User";
        try
        {
            var token = Request.Cookies["JwtToken"];
            var decodedToken = await _commonRepository.ValidateToken(token);
            var roleId = decodedToken.Role;
            // if (!await _commonRepository.canAddOrEdit("User"))
            // {
            //     return RedirectToAction("AccessDenied", "Login");
            // }
            var userInfo = await _usersRepository.GetUserForEdit(email);
            return View(userInfo);
        }
        catch (Exception ex)
        {
            _notfy.Error("An Unexpected error got occured,Please try again");
            Console.WriteLine($"error from Controller : {ex.Message}");
            return RedirectToAction("EditUser", "SuperAdmin", new { email = email });
        }
    }

    //Handles form submission to update user details
    [HttpPost]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "User", "AddOrEdit")]
    public async Task<IActionResult> EditUserPost(EditUserViewModel editUserViewModel)
    {
        ViewData["ActivePage"] = "User";
        try
        {
            var token = Request.Cookies["JwtToken"];
            var decodedToken = await _commonRepository.ValidateToken(token);
            var roleId = decodedToken.Role;
            // if (!await _commonRepository.canAddOrEdit( "User"))
            // {
            //     return RedirectToAction("AccessDenied", "Login");
            // }
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, redirectUrl = "/Users/EditUser?email=" + editUserViewModel.Email });
            }
            string uploadFolder = Path.Combine(_webHostEnvironmnet.WebRootPath, "uploads");
            var result = await _usersRepository.EditUser(editUserViewModel, uploadFolder);
            switch (result)
            {
                case "UserName already taken":
                case "phone number is already registered":
                case "Only jpg, png,jpeg  images are allowed":
                case "Image must be less than 3MB":
                    TempData["ToasterMessage"] = result;
                    TempData["ToasterType"] = "error";
                    return Json(new { success = true, redirectUrl = "/Users/EditUser?email=" + editUserViewModel.Email });
                default:
                    TempData["ToasterMessage"] = result;
                    TempData["ToasterType"] = "success";
                    return Json(new { success = false, redirectUrl = "/Users/UserList" });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error from Controller : {ex.Message}");
            TempData["ToasterMessage"] = "An Unexpected error got occured,Please try again";
            TempData["ToasterType"] = "error";
            return Json(new { success = false, redirectUrl = "/Users/UserList" });
        }
    }

    //Deletes a user by email
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "User", "Delete")]
    public async Task<IActionResult> DeleteUser(string email)
    {
        ViewData["ActivePage"] = "User";
        try
        {
            var token = Request.Cookies["JwtToken"];
            var decodedToken = await _commonRepository.ValidateToken(token);
            var roleId = decodedToken.Role;
            var result = await _usersRepository.DeleteUser(email);
            if (result == "user deleted successfully")
            {
                _notfy.Success(result);
                return RedirectToAction("UserList", "Users");
            }
            _notfy.Error(result);
            return RedirectToAction("UserList", "Users");
        }
        catch (Exception ex)
        {
            _notfy.Error("An Unexpected error got occured,Please try again");
            Console.WriteLine($"error from Controller : {ex.Message}");
            return RedirectToAction("UserList", "Users");
        }
    }
}
