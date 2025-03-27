

using Microsoft.AspNetCore.Mvc;
using PizzaShop.Services.Interfaces;
using PizzaShop.Domain.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using PizzaShop.Service.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PizzaShop.Web.Controllers
{
    public class PermissionController : Controller
    {
        private readonly IRolePermissionService _rolePermissionService;
        private readonly IRoleService _roleService;
        private readonly IUserServices _userServices;

        public PermissionController(IRolePermissionService rolePermissionService, IRoleService roleService, IUserServices userServices)
        {
            _rolePermissionService = rolePermissionService;
            _roleService = roleService;
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


        public async Task<IActionResult> Role()
        {
            await SetUserProfileInViewBag();

            var token = Request.Cookies["AuthToken"];
            var email = _userServices.ExtractEmailFromToken(token);


            bool isFirstLogin = await _userServices.IsFirstLoginAsync(email);
            if (isFirstLogin)
            {
                return RedirectToAction("ChangePassword", "Login");
            }

            var roles = await _roleService.GetRolesAsync();
            return View(roles);
        }

        // public async Task<IActionResult> RolePermission(int roleId)
        // {
        //     await SetUserProfileInViewBag();
            
        //     ViewBag.Roles = await _rolePermissionService.GetRolesAsync();
        //     ViewBag.Permissions = await _rolePermissionService.GetPermissionsAsync();
        //     ViewBag.RolePermissions = await _rolePermissionService.GetRolePermissionsAsync(roleId);
        //     return View();
        // }

        public async Task<IActionResult> RolePermission(int roleId)
        {
            await SetUserProfileInViewBag();

            var token = Request.Cookies["AuthToken"];
            var email = _userServices.ExtractEmailFromToken(token);


            bool isFirstLogin = await _userServices.IsFirstLoginAsync(email);
            if (isFirstLogin)
            {
                return RedirectToAction("ChangePassword", "Login");
            }


            ViewBag.Roles = await _rolePermissionService.GetRolesAsync();
            ViewBag.Permissions = await _rolePermissionService.GetPermissionsAsync();
            ViewBag.RolePermissions = await _rolePermissionService.GetRolePermissionsAsync(roleId);
            ViewBag.SelectedRoleId = roleId; //  selected role ID 
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetRolePermissions(int roleId)
        {
            await SetUserProfileInViewBag();

            var token = Request.Cookies["AuthToken"];
            var email = _userServices.ExtractEmailFromToken(token);


            bool isFirstLogin = await _userServices.IsFirstLoginAsync(email);
            if (isFirstLogin)
            {
                return RedirectToAction("ChangePassword", "Login");
            }


            Console.WriteLine($"Fetching permissions for Role ID: {roleId}");

            var permissions = await _rolePermissionService.GetPermissionsAsync();
            var rolePermissions = await _rolePermissionService.GetRolePermissionsAsync(roleId);

            var result = permissions.Select(p => new
            {
                PermissionId = p.Id,
                Name = p.Name,
                CanView = rolePermissions.Any(rp => rp.PermissionId == p.Id && (rp.CanView ?? false)),
                CanAddEdit = rolePermissions.Any(rp => rp.PermissionId == p.Id && (rp.CanAddEdit ?? false)),
                CanDelete = rolePermissions.Any(rp => rp.PermissionId == p.Id && (rp.CanDelete ?? false))
            }).ToList();

            Console.WriteLine($"Returning {result.Count} permissions");
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> SaveRolePermissions([FromBody] List<RolePermissionViewModel> rolePermissions)
        {
            Console.WriteLine($"Received {rolePermissions.Count} permissions for Role ID: {rolePermissions.FirstOrDefault()?.RoleId}");

            if (rolePermissions == null || !rolePermissions.Any())
            {
                return BadRequest("Invalid data received.");
            }

            bool success = await _rolePermissionService.UpdateRolePermissionsAsync(rolePermissions);

            if (!success)
            {
                return StatusCode(500, "Failed to save permissions.");
            }

            TempData["success"] = "permissions saved successfully";

            return Json(new { success });
        }

        // private string ExtractEmailFromToken(string token)
        // {
        //     var handler = new JwtSecurityTokenHandler();
        //     var jwtToken = handler.ReadJwtToken(token);
        //     var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

        //     return emailClaim?.Value;
        // }
    }
}