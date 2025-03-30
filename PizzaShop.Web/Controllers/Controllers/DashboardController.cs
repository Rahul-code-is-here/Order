using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using BussinessLogicLayer.Interface;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using PizzaShopRMS.Helpers;

namespace PizzaShopRMS.Controllers;
// [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager", "Chef" })]
public class DashboardController : Controller
{
    private readonly INotyfService _notyf;
    private readonly IDashboardRepository _dashboardRepository;
    private readonly ICommonRepository _commonRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public DashboardController(IDashboardRepository dashboardRepository, INotyfService notyf, ICommonRepository commonRepository, IWebHostEnvironment webHostEnvironment)
    {
        _notyf = notyf;
        _dashboardRepository = dashboardRepository;
        _commonRepository = commonRepository;
        _webHostEnvironment = webHostEnvironment;
    }
    //Retrieves the list of roles asynchronously and returns them as JSON.
    [HttpGet]
    
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> GetRoles()
    {
        var countries = await _commonRepository.GetRolesAsync();
        return Json(countries);
    }

    //Retrieves the list of countries asynchronously and returns them as JSON.
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> GetCountries()
    {
        var countries = await _commonRepository.GetCountriesAsync();
        return Json(countries);
    }

    //Retrieves the list of states based on the provided country ID and returns them as JSON.
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> GetStates(int countryId)
    {
        var states = await _commonRepository.GetStatesAsync(countryId);
        return Json(states);
    }

    //Retrieves the list of cities based on the provided state ID and returns them as JSON.
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> GetCities(int StateId)
    {
        var cities = await _commonRepository.GetCitiesAsync(StateId);
        return Json(cities);
    }

    //Fetches and displays the user profile based on the provided email.
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> MyProfile()
    {
        ViewData["ActivePage"] = "DashBoard";
        try
        {
            var token = Request.Cookies["JwtToken"];
            var decodedToken = await _commonRepository.ValidateToken(token);
            var userProfile = await _dashboardRepository.GetUserProfileAsync(decodedToken.UserEmail);
            return View(userProfile);
        }
        catch (Exception ex)
        {
            _notyf.Error("An Unexcpected error occured,try again");
            Console.WriteLine($"error from controller :{ex.Message}");
            return RedirectToAction("Myprofile", "Dashboard");
        }
    }

    //Handles updating the user profile, including image uploads.
    [HttpPost]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> MyProfilePost(MyProfileViewModel myProfileViewModel)
    {
        ViewData["ActivePage"] = "DashBoard";
        try
        {
            var token = Request.Cookies["JwtToken"];
            var decodedToken = await _commonRepository.ValidateToken(token);
            var userEmail = decodedToken.UserEmail;

            if (!ModelState.IsValid)
            {
                _notyf.Error("Model State is not valid");
                return RedirectToAction("MyProfile", "Dashboard", new { email = userEmail });
                // return View("MyProfile",myProfileViewModel);
            }

            var uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            var result = await _dashboardRepository.UpdateMyProfileAsync(myProfileViewModel, userEmail, uploadFolder);
            switch (result[0])
            {
                case "username already exist":
                    _notyf.Error(result[0]);
                    return RedirectToAction("MyProfile", "Dashboard", new { email = userEmail });
                case "phone number is already registered":
                    _notyf.Error(result[0]);
                    return RedirectToAction("MyProfile", "Dashboard", new { email = userEmail });
                case "user not found":
                    _notyf.Error(result[0]);
                    return RedirectToAction("MyProfile", "Dashboard", new { email = userEmail });
                case "Only jpg, png,jpeg  images are allowed":
                    _notyf.Error(result[0]);
                    return RedirectToAction("MyProfile", "Dashboard", new { email = userEmail });
                case "Image must be less than 3MB":
                    _notyf.Error(result[0]);
                    return RedirectToAction("MyProfile", "Dashboard", new { email = userEmail });
                default:
                    _notyf.Success(result[0]);
                     HttpContext.Session.SetString("UserProfileImage",result[1]);
                    return RedirectToAction("MyProfile", "Dashboard", new { email = userEmail });
            }
        }
        catch (Exception ex)
        {
            _notyf.Error("An Unexpected error  occured,Please try again");
            Console.WriteLine($"Error from controller : {ex.Message}");
            return RedirectToAction("DashBoard", "SuperAdmin");
        }
    }

    //Displays the change password page.
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult ChangePassword()
    {
        ViewData["ActivePage"] = "DashBoard";
        return View();
    }

    //Handles the password change request and logs the user out upon success.
    [HttpPost]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
    {
        ViewData["ActivePage"] = "DashBoard";
        try
        {
            var token = Request.Cookies["JwtToken"];
            var decodedToken = await _commonRepository.ValidateToken(token);
            var userEmail = decodedToken.UserEmail;
            if (!ModelState.IsValid)
                return View(changePasswordViewModel);
            var result = await _dashboardRepository.ChangePasswordAsync(changePasswordViewModel, userEmail);
            if (result == "Password changed successfully,login again")
            {
                _notyf.Success("Password changes successfully,login again");
                Response.Cookies.Delete("JwtToken");
                return RedirectToAction("Login", "Login");
            }
            _notyf.Error("Incorrect Password");
            return View(changePasswordViewModel);
        }
        catch (Exception ex)
        {
            _notyf.Error("An unexpected error occured,try again");
            Console.Write($"Error from controller : {ex.Message}");
            return RedirectToAction("DashBoard", "SuperAdmin");
        }
    }
}
