


using PizzaShop.Domain.DataModels;

using PizzaShop.Domain.ViewModels;  
using Microsoft.AspNetCore.Mvc;
using PizzaShop.Repository.Interfaces;
using PizzaShop.Service.Interface;

namespace PizzaShopRMS.Controllers;

public class TableAndSectionController : Controller
{
    private readonly ITableAndSectionRepository _tableAndSectionRepository;

    private readonly ITableAndSectionService _tableAndSectionService;

    private readonly IUserServices _userServices;

    // private readonly ICommonRepository _commonRepository;
    public TableAndSectionController(ITableAndSectionRepository tableAndSectionRepository, ITableAndSectionService tableAndSectionService, IUserServices userServices)
    {
        _tableAndSectionRepository = tableAndSectionRepository;
        _tableAndSectionService= tableAndSectionService;
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

    public async Task<IActionResult> TableAndSection()
    {
        await SetUserProfileInViewBag();
        var token = Request.Cookies["AuthToken"];
        var email = _userServices.ExtractEmailFromToken(token);


        bool isFirstLogin = await _userServices.IsFirstLoginAsync(email);
        if (isFirstLogin)
        {
            return RedirectToAction("ChangePassword", "Login");
        }

        TableAndSectionViewModel tableAndSection = new TableAndSectionViewModel();
        tableAndSection.sectionList = await _tableAndSectionService.GetSectionList();
        return View(tableAndSection);
    }

    public async Task<IActionResult> GetTablesListForSection(Pagination<TableViewModel> tableList, string sectionId)
    {
        await SetUserProfileInViewBag();
        var token = Request.Cookies["AuthToken"];
        var email = _userServices.ExtractEmailFromToken(token);


        bool isFirstLogin = await _userServices.IsFirstLoginAsync(email);
        if (isFirstLogin)
        {
            return RedirectToAction("ChangePassword", "Login");
        }

        try
        {
            var data = await _tableAndSectionService.GetTablesListForSection(tableList, sectionId);
            return PartialView("_TableListPartialView", data);
        }
        catch (Exception ex)
        {

            TempData["error"] = "An Unexpected error got occured";
         
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/TableAndSection/TableAndSection" });
        }
    }
    [HttpPost]
    public async Task<IActionResult> AddSection(SectionViewModel AddSection)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Model state is not valid";
              
                return Json(new { success = false, redirectUrl = "/TableAndSection/TableAndSection" });
            }
            // var token = Request.Cookies["JwtToken"];
            // var decodedToken = await _commonRepository.ValidateToken(token);
            // var userId = decodedToken.UserId;
            // AddSection.CreatedBy = userId;
            var result = await _tableAndSectionService.AddSection(AddSection);
            if (result == "Section Added Successfully")
            {
                TempData["success"] = result;
                
                return Json(new { success = true, redirectUrl = "/TableAndSection/TableAndSection" });
            }
            TempData["error"] = result;
            
            return Json(new { success = true, redirectUrl = "/TableAndSection/TableAndSection" });
        }
        catch (Exception ex)
        {
            TempData["error"] = "An Unexpected error got occured";
            
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/TableAndSection/TableAndSection" });
        }
    }

    public async Task<IActionResult> GetSectionForEdit(string sectionId)
    {
        await SetUserProfileInViewBag();

        var token = Request.Cookies["AuthToken"];
        var email = _userServices.ExtractEmailFromToken(token);


        bool isFirstLogin = await _userServices.IsFirstLoginAsync(email);
        if (isFirstLogin)
        {
            return RedirectToAction("ChangePassword", "Login");
        }

        try
        {
            var section = await _tableAndSectionService.GetSectionForEdit(sectionId);
            return Json(section);
        }
        catch (Exception ex)
        {
            TempData["error"] = "An Unexpected error got occured";
           
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/TableAndSection/TableAndSection" });
        }
    }
    [HttpPost]
    public async Task<IActionResult> EditSection(SectionViewModel AddSection)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Model state is not valid";
                
                return Json(new { success = false, redirectUrl = "/TableAndSection/TableAndSection" });
            }
            // var token = Request.Cookies["JwtToken"];
            // var decodedToken = await _commonRepository.ValidateToken(token);
            // var userId = decodedToken.UserId;
            // AddSection.EditedBy = userId;
            var result = await _tableAndSectionService.EditSection(AddSection);
            switch (result)
            {
                case "Section with the name already exist":
                    TempData["error"] = result;
                    
                    return Json(new { success = false, redirectUrl = "/TableAndSection/TableAndSection" });
                default:
                    TempData["success"] = result;
                   
                    return Json(new { success = true, redirectUrl = "/TableAndSection/TableAndSection" });
            }
        }
        catch (Exception ex)
        {
            TempData["error"] = "An Unexpected error got occured";
           
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/TableAndSection/TableAndSection" });
        }
    }
    public async Task<IActionResult> DeleteSection(string sectionId)
    {
        try
        {
            var result = await _tableAndSectionService.DeleteSection(sectionId);
            if (result == "section Deleted Successfully")
            {
                TempData["success"] = result;
                
                return Json(new { success = true, redirectUrl = "/TableAndSection/TableAndSection" });   
            }
            TempData["error"] = result;
            
            return Json(new { success = false, redirectUrl = "/TableAndSection/TableAndSection" });
        }
        catch (Exception ex)
        {
            TempData["error"] = "An Unexpected error got occured";
           
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/TableAndSection/TableAndSection" });
        }
    }
    [HttpPost]
    public async Task<IActionResult> AddTable(TableViewModel AddTable){
       try
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Model state is not valid";
            
                return Json(new { success = false, redirectUrl = "/TableAndSection/TableAndSection" });
            }
            // var token = Request.Cookies["JwtToken"];
            // var decodedToken = await _commonRepository.ValidateToken(token);
            // var userId = decodedToken.UserId;
            // AddTable.CreatedBy = userId;
            var result = await _tableAndSectionService.AddTable(AddTable);
            if (result == "Table Added Successfully")
            {
                TempData["success"] = result;
              
                return Json(new { success = true, redirectUrl = "/TableAndSection/TableAndSection" });
            }
            TempData["error"] = result;
          
            return Json(new { success = false, redirectUrl = "/TableAndSection/TableAndSection" });
        }
        catch (Exception ex)
        {
            TempData["error"] = "An Unexpected error got occured";
           
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/TableAndSection/TableAndSection" });
        } 
    }

    public async Task<IActionResult> GetTableForEdit(string tableId){
        try
        {
            var table = await _tableAndSectionService.GetTableForEdit(tableId);
            return Json(table);
        }
        catch (Exception ex)
        {
            TempData["error"] = "An Unexpected error got occured";
     
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/TableAndSection/TableAndSection" });
        }
    }
    [HttpPost]
    public async Task<IActionResult> EditTable(TableViewModel AddTable){
        try
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Model state is not valid";
          
                return Json(new { success = false, redirectUrl = "/TableAndSection/TableAndSection" });
            }
            // var token = Request.Cookies["JwtToken"];
            // var decodedToken = await _commonRepository.ValidateToken(token);
            // var userId = decodedToken.UserId;
            // AddTable.EditedBy = userId;
            var result = await _tableAndSectionService.EditTable(AddTable);
            switch (result)
            {
                case "Table with the name already exist":
                    TempData["error"] = result;
                   
                    return Json(new { success = false, redirectUrl = "/TableAndSection/TableAndSection" });
                default:
                    TempData["success"] = result;
                    
                    return Json(new { success = true, redirectUrl = "/TableAndSection/TableAndSection" });
            }
        }
        catch (Exception ex)
        {
            TempData["error"] = "An Unexpected error got occured";
           
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/TableAndSection/TableAndSection" });
        }
    }
    [HttpPost]
    public async Task<IActionResult> DeleteTable(List<string> tableIds){
        try
        {
            var result = await _tableAndSectionService.DeleteTable(tableIds);
            TempData["success"] = result;
            
            return Json(new { success = true,redirectUrl = "/TableAndSection/TableAndSection" });
        }
        catch (Exception ex)
        {
            TempData["error"] = "An Unexpected error got occured";
            
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/TableAndSection/TableAndSection"});
        }
    }
}
