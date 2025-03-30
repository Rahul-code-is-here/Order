using BussinessLogicLayer.Interface;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using PizzaShopRMS.Helpers;

namespace PizzaShopRMS.Controllers;

public class RolesAndPermissionController : Controller
{
    private readonly IRolesAndPermissionRepository _rolesAndPermissionRepository;

    private readonly ICommonRepository _commonRepository;

    public RolesAndPermissionController(IRolesAndPermissionRepository rolesAndPermissionRepository, ICommonRepository commonRepository)
    {
        _rolesAndPermissionRepository = rolesAndPermissionRepository;
        _commonRepository = commonRepository;
    }
    //  Displays the roles page
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [CustomAuthorise(new string[] {"SuperAdmin"}, "Roles and Permissions", "View")]
    public IActionResult Roles()
    {
        ViewData["ActivePage"] = "Role";
        return View();
    }

    // Fetches permissions for a role
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [CustomAuthorise(new string[] {"SuperAdmin"}, "Roles and Permissions", "View")]
    public async Task<IActionResult> Permissions(string role)
    {
        ViewData["ActivePage"] = "Role";
        ViewBag.role = role;
        try
        {
            var permissions = await _rolesAndPermissionRepository.GetPermissions(role);
            return View(permissions);
        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return RedirectToAction("Permissions", "RolesAndPermission", role);
        }
    }

    //Handles permission updates for a role
    [HttpPost]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [CustomAuthorise(new string[] {"SuperAdmin"}, "Roles and Permissions", "AddOrEdit")]
    public async Task<IActionResult> PermissionPost(RolePermissionViewModel rolePermissionViewModel)
    {
        try
        {
            var token = Request.Cookies["JwtToken"];
            var decodedToken = await _commonRepository.ValidateToken(token);
            var userId = decodedToken.UserId;
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Permissions", "RolesAndPermission", new { role = rolePermissionViewModel.Role });
            }
            rolePermissionViewModel.EditedBy = userId;
            var result = await _rolesAndPermissionRepository.UpdatePermissions(rolePermissionViewModel);
            if (result == "Permissions Updated Sucessfully")
            {
                TempData["ToasterMessage"] = result;
                TempData["ToasterType"] = "success";
                return RedirectToAction("Permissions", "RolesAndPermission", new { role = rolePermissionViewModel.Role });
            }
            return RedirectToAction("Permissions", "RolesAndPermission", new { role = rolePermissionViewModel.Role });
        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return RedirectToAction("Permissions", "RolesAndPermission", new { role = rolePermissionViewModel.Role });
        }
    }
}
