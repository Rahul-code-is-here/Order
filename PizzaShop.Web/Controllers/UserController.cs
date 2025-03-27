

using Microsoft.AspNetCore.Mvc;
using PizzaShop.Service.Interface;
using PizzaShop.Domain.ViewModels;
using System;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using PizzaShop.Domain.DataModels;
using PizzaShop.Service.Implementation;
using PizzaShop.Web.Helpers;
using PizzaShop.Service.Implementaion;

namespace PizzaShop.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserServices _userServices;

        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        // private async Task SetUserProfileInViewBag()
        // {
        //     var token = Request.Cookies["AuthToken"];
        //     var email = _userServices.ExtractEmailFromToken(token);



        //     if (!string.IsNullOrEmpty(email))
        //     {
        //         var userProfile = await _userServices.GetUserProfileAsync(email);
        //         if (userProfile != null)
        //         {
        //             ViewBag.UserName = userProfile.UserName;
        //             ViewBag.UserImage = userProfile.PathOfProfilePicture;
        //         }
        //     }
        // }
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


        [HttpGet]
        public async Task<IActionResult> UpdateProfile()
        {
            await SetUserProfileInViewBag();

            var token = Request.Cookies["AuthToken"];
            var email = _userServices.ExtractEmailFromToken(token);


            bool isFirstLogin = await _userServices.IsFirstLoginAsync(email);
            if (isFirstLogin)
            {
                return RedirectToAction("ChangePassword", "Login");
            }

            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Login");

            var model = await _userServices.GetUserProfileAsync(email);
            if (model == null)
                return RedirectToAction("Login", "Login");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateProfileModel model)
        {
            await SetUserProfileInViewBag();

            var token = Request.Cookies["AuthToken"];
            var email = _userServices.ExtractEmailFromToken(token);

            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Auth");

            var success = await _userServices.UpdateUserProfileAsync(model, email);
            if (!success)
                return View(model);

            TempData["success"] = "Profile updated successfully!";
            return RedirectToAction("UpdateProfile");
        }



        [HttpGet]
        public async Task<IActionResult> GetStates(int countryId)
        {
            var states = await _userServices.GetStatesAsync(countryId);
            return Json(states);
        }

        [HttpGet]
        public async Task<IActionResult> GetCities(int stateId)
        {
            var cities = await _userServices.GetCitiesAsync(stateId);
            return Json(cities);
        }


        [HttpGet]
        public async Task<IActionResult> UserList(string searchQuery = "", int pageNumber = 1, int pageSize = 10, string sortBy = "Name", string sortOrder = "asc")
        {
            await SetUserProfileInViewBag();

            var token = Request.Cookies["AuthToken"];
            var email = _userServices.ExtractEmailFromToken(token);


            bool isFirstLogin = await _userServices.IsFirstLoginAsync(email);
            if (isFirstLogin)
            {
                return RedirectToAction("ChangePassword", "Login");
            }


            var (users, totalCount) = await _userServices.GetUserListAsync(searchQuery, pageNumber, pageSize, sortBy, sortOrder);

            int userTotal = await _userServices.GetTotalUsersCountAsync();

            ViewBag.TotalCount = userTotal;


            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;
            ViewBag.SearchQuery = searchQuery;
            ViewBag.SortBy = sortBy;
            ViewBag.SortOrder = sortOrder;

            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> SoftDeleteUser(int id)
        {
            await SetUserProfileInViewBag();

            var success = await _userServices.SoftDeleteUserAsync(id);
            if (!success)
            {
                TempData["error"] = "Failed to delete user.";
            }

            TempData["success"] = "user deleted successfully!";
            return RedirectToAction("UserList");
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            await SetUserProfileInViewBag();

            var token = Request.Cookies["AuthToken"];
            var email = _userServices.ExtractEmailFromToken(token);


            bool isFirstLogin = await _userServices.IsFirstLoginAsync(email);
            if (isFirstLogin)
            {
                return RedirectToAction("ChangePassword", "Login");
            }

            var model = await _userServices.GetUserByIdAsync(id);
            if (model == null) return NotFound();

            // Populate countries for the dropdown
            model.Countries = await _userServices.GetCountriesAsync();

            return View(model);
        }

        // [HttpPost]
        // public async Task<IActionResult> EditUser(EditUserModel model)
        // {
        //     await SetUserProfileInViewBag();

        //     var existingUser = await _userServices.GetUserByUsernameAsync(model.UserName, model.Id);
        //     if (existingUser != null)
        //     {
        //         TempData["error"] = "Username already exists.";
        //         return RedirectToAction("EditUser", new { id = model.Id });

        //     }

        //     var existUser = await _userServices.GetCurrentUserAsync(model.Email, model.Id);
        //     if (existUser != null)
        //     {
        //         ModelState.AddModelError(string.Empty, "Email already exists.");
        //         TempData["error"] = "Email already exists.";
        //         model.Countries = await _userServices.GetCountriesAsync(); // to populate the dropdown
        //         return View(model);
        //     }

        //     var success = await _userServices.UpdateUserAsync(model);
        //     if (!success) return NotFound();

        //     TempData["success"] = "User updated successfully!";
        //     return RedirectToAction("UserList");
        // }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserModel model)
        {
            await SetUserProfileInViewBag();

            // Check if the username already exists (excluding the current user being edited)
            var existingUser = await _userServices.GetUserByUsernameAsync(model.UserName, model.Id);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "Username already exists.");
                TempData["error"] = "Username already exists.";
                model.Countries = await _userServices.GetCountriesAsync(); // Reload countries
                return View(model);
            }

            // Check if the email already exists (excluding the current user being edited)
            var existUser = await _userServices.GetCurrentUserAsync(model.Email, model.Id);
            if (existUser != null)
            {
                ModelState.AddModelError(string.Empty, "Email already exists.");
                TempData["error"] = "Email already exists.";
                model.Countries = await _userServices.GetCountriesAsync(); // Reload countries
                return View(model);
            }

            var success = await _userServices.UpdateUserAsync(model);
            if (!success) return NotFound();

            TempData["success"] = "User updated successfully!";
            return RedirectToAction("UserList");
        }

        [HttpGet]
        public async Task<IActionResult> AddUserAsync()
        {
            await SetUserProfileInViewBag();

            var token = Request.Cookies["AuthToken"];
            var email = _userServices.ExtractEmailFromToken(token);


            bool isFirstLogin = await _userServices.IsFirstLoginAsync(email);
            if (isFirstLogin)
            {
                return RedirectToAction("ChangePassword", "Login");
            }


            var model = new UserModel
            {
                Countries = _userServices.GetCountriesAsync().Result,
                States = new List<State>(),
                Cities = new List<City>()
            };
            return View(model);
        }

        // [HttpPost]
        // public async Task<IActionResult> AddUserAsync(UserModel model)
        // {
        //     await SetUserProfileInViewBag();

        //     // Check if the username already exists
        //     var existingUser = await _userServices.GetUserByUsernameAsync(model.UserName);
        //     if (existingUser != null)
        //     {
        //         // TempData["error"] = "Username already exists.";
        //         // return RedirectToAction("AddUser");
        //         // // return View(model);
        //         // // throw new Exception("Username already exists.");
        //          ModelState.AddModelError(string.Empty, "Username already exists.");
        //          TempData["error"] = "Username already exists.";
        //          return View(model);
        //     }

        //     var existUser = await _userServices.GetCurrentUserAsync(model.Email);
        //     if (existUser != null)
        //     {
        //         TempData["error"] = "Email already exists.";
        //         // return RedirectToAction("AddUser");
        //         return View(model);
        //         // throw new Exception("Email already exists.");
        //     }

        //     _userServices.AddUser(model);

        //     // Send welcome email
        //     await _userServices.SendWelcomeEmailAsync(model.Email, model.Password);

        //     TempData["success"] = "User added successfully!";
        //     return RedirectToAction("UserList", "User");
        // }

        [HttpPost]
        public async Task<IActionResult> AddUserAsync(UserModel model)
        {
            await SetUserProfileInViewBag();

            // Check if the username already exists
            var existingUser = await _userServices.GetUserByUsernameAsync(model.UserName);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "Username already exists.");
                TempData["error"] = "Username already exists.";
                model.Countries = await _userServices.GetCountriesAsync(); // to populate the dropdown
                return View(model);
            }

            var existUser = await _userServices.GetCurrentUserAsync(model.Email);
            if (existUser != null)
            {
                ModelState.AddModelError(string.Empty, "Email already exists.");
                TempData["error"] = "Email already exists.";
                model.Countries = await _userServices.GetCountriesAsync(); // to populate the dropdown
                return View(model);
            }

            _userServices.AddUser(model);

            // Send welcome email
            await _userServices.SendWelcomeEmailAsync(model.Email, model.Password);

            TempData["success"] = "User added successfully!";
            return RedirectToAction("UserList", "User");
        }
    }
}