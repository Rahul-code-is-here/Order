using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using BussinessLogicLayer.Interface;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.OutputCaching;
using Org.BouncyCastle.Asn1;

namespace PizzaShopRMS.Controllers;

[OutputCache(NoStore = true, Duration = 0)]
public class LoginController : Controller
{
    private readonly INotyfService _notyf;
    private readonly ILoginRepository _loginRepository;
    private readonly ICommonRepository _commonRepository;
    private readonly IJwtRepository _jwtRepository;

    public LoginController(INotyfService notyf, ILoginRepository loginRepository, ICommonRepository commonRepository, IJwtRepository jwtRepository)
    {
        _notyf = notyf;
        _loginRepository = loginRepository;
        _commonRepository = commonRepository;
        _jwtRepository = jwtRepository;
    }

    // Displays the login page. If the user is already authenticated via JWT token, redirects to the dashboard.
    public async Task<IActionResult> Login()
    {
        var token = Request.Cookies["JwtToken"];
        if (!string.IsNullOrEmpty(token))
        {
            var valid = await _commonRepository.ValidateToken(token);
            HttpContext.Session.SetString("UserProfileImage", valid.Profilepic);
            HttpContext.Session.SetString("UserName", valid.UserName);
            if ((bool)valid.Valid)
            {
                return RedirectToAction("DashBoard", "SuperAdmin");
            }
        }
        ViewBag.Title = "Login";
        return View();
    }

    // Authenticates the user based on login credentials and sets a JWT token in cookies.
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }
            var expiryTime = loginViewModel.RememberMe ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddDays(1);
            var token = await _loginRepository.AuthenticateUserAsync(loginViewModel);
            if (token == "Invalid Credentials")
            {
                _notyf.Error(token);
                return View(loginViewModel);
            }
            if (token == "user not found")
            {
                _notyf.Error(token);
                return View(loginViewModel);
            }
            Response.Cookies.Append("JwtToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = expiryTime
            });
            var decodedToken = await _commonRepository.ValidateToken(token);
            HttpContext.Session.SetString("UserProfileImage", decodedToken.Profilepic);
            HttpContext.Session.SetString("UserName", decodedToken.UserName);
            Console.WriteLine(decodedToken.Role);
            if (decodedToken.Role == "AccountManager")
            {
                _notyf.Success("Login successful");
                return RedirectToAction("Index", "Home");
            }
            if (decodedToken.Role == "Chef")
            {
                _notyf.Success("Login successful");
                return RedirectToAction("Privacy", "Home");
            }
            _notyf.Success("Login successful");
            return RedirectToAction("Dashboard", "SuperAdmin");
        }
        catch (Exception ex)
        {
            _notyf.Error("An unexpected error occurred.");
            Console.WriteLine($"[Controller Error]: {ex.Message}");
            return View(loginViewModel);
        }
    }

    // Displays the forgot password page, allowing users to enter their email for password reset.
    public IActionResult ForgotPassword(string email)
    {
        if (!string.IsNullOrEmpty(email))
        {
            ViewData["enteredEmail"] = email;
        }
        else
        {
            ViewData["enteredEmail"] = "";
        }
        return View();
    }

    //Processes the forgot password request by generating a reset token and sending an email with a reset link.
    [HttpPost]
    public async Task<IActionResult> ForgetPasswordPost(ForgotPasswordViewModel forgotPaswordViewModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View("ForgotPassword", forgotPaswordViewModel);
            }
            var token = _jwtRepository.generateJWTToken(forgotPaswordViewModel.Email, "", "", DateTime.UtcNow.AddDays(1), "", "");
            var resetPasswordLink = Url.Action("ResetPassword", "Login", new { token = token }, Request.Scheme);

            var result = await _loginRepository.ForgotPasswordSendEmailAsync(forgotPaswordViewModel, resetPasswordLink);
            if (!result)
            {
                _notyf.Error(" User not found");
                return RedirectToAction("ForgotPassword");
            }
            _notyf.Success("email sent successfully");
            return RedirectToAction("Login", "Login");

        }
        catch (Exception ex)
        {
            _notyf.Error("An unexpected error occurred");
            Console.WriteLine($"[exception from controller] : {ex.Message}");
            return View("ForgotPassword", forgotPaswordViewModel);
        }
    }

    // Displays the reset password page where users can enter a new password using a token.
    public IActionResult ResetPassword(string token)
    {
        ViewBag.Title = "Reset_password";
        ViewData["ResetPasswordEmail"] = token;
        return View();
    }

    //Handles the password reset process by validating the reset token and updating the user's password.
    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View("ResetPassword", resetPasswordViewModel);

            }
            var validate = await _commonRepository.ValidateToken(resetPasswordViewModel.Token);
            if (!(bool)validate.Valid)
            {
                _notyf.Error("Link got expired generate new link");
                return RedirectToAction("ForgotPassword", "Login");
            }
            resetPasswordViewModel.Email = validate.UserEmail;
            var result = await _loginRepository.ResetPassword(resetPasswordViewModel);
            if (result)
            {
                _notyf.Success("password changed successfully");
                return View("ResetPassword");
            }
            _notyf.Error("error occured try again later");
            return View("ResetPassword", resetPasswordViewModel);
        }
        catch (Exception ex)
        {
            _notyf.Error("An Unexpected error occurred");
            Console.WriteLine($"[exception from controller] : {ex.Message}");
            return View("ResetPassword", resetPasswordViewModel);
        }
    }

    //Logs out the user by deleting the JWT token from cookies and redirecting to the login page.
    public IActionResult LogOut()
    {
        Response.Cookies.Delete("JwtToken");
        return RedirectToAction("Login", "Login");
    }

    // Displays the access denied page for unauthorized users.
    public IActionResult AccessDenied()
    {
        return View();
    }
}
