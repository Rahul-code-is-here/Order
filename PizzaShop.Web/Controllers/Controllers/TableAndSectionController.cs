using BussinessLogicLayer.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using PizzaShopRMS.Helpers;

namespace PizzaShopRMS.Controllers;

public class TableAndSectionController : Controller
{
    private readonly ITableAndSectionRepository _tableAndSectionRepository;

    private readonly ICommonRepository _commonRepository;
    public TableAndSectionController(ITableAndSectionRepository tableAndSectionRepository, ICommonRepository commonRepository)
    {
        _tableAndSectionRepository = tableAndSectionRepository;
        _commonRepository = commonRepository;
    }
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "Tables and Sections", "View")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> TableAndSection()
    {
        TableAndSectionViewModel tableAndSection = new TableAndSectionViewModel();
        tableAndSection.sectionList = await _tableAndSectionRepository.GetSectionList();
        return View(tableAndSection);
    }
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager"}, "Tables and Sections", "View")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> GetTablesListForSection(Pagination<TableViewModel> tableList, string sectionId)
    {
        try
        {
            var data = await _tableAndSectionRepository.GetTablesListForSection(tableList, sectionId);
            return PartialView("_TableListPartialView", data);
        }
        catch (Exception ex)
        {

            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/TableAndSection/TableAndSection" });
        }
    }
    [HttpPost]
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "Tables and Sections", "AddOrEdit")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> AddSection(SectionViewModel AddSection)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                TempData["ToasterMessage"] = "Model state is not valid";
                TempData["ToasterType"] = "error";
                return Json(new { success = false, redirectUrl = "/TableAndSection/TableAndSection" });
            }
            var token = Request.Cookies["JwtToken"];
            var decodedToken = await _commonRepository.ValidateToken(token);
            var userId = decodedToken.UserId;
            AddSection.CreatedBy = userId;
            var result = await _tableAndSectionRepository.AddSection(AddSection);
            if (result == "Section Added Successfully")
            {
                TempData["ToasterMessage"] = result;
                TempData["ToasterType"] = "success";
                return Json(new { success = true, redirectUrl = "/TableAndSection/TableAndSection" });
            }
            TempData["ToasterMessage"] = result;
            TempData["ToasterType"] = "error";
            return Json(new { success = true, redirectUrl = "/TableAndSection/TableAndSection" });
        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/TableAndSection/TableAndSection" });
        }
    }
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "Tables and Sections", "AddOrEdit")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> GetSectionForEdit(string sectionId)
    {
        try
        {
            var section = await _tableAndSectionRepository.GetSectionForEdit(sectionId);
            return Json(section);
        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/TableAndSection/TableAndSection" });
        }
    }
    [HttpPost]
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "Tables and Sections", "AddOrEdit")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> EditSection(SectionViewModel AddSection)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                TempData["ToasterMessage"] = "Model state is not valid";
                TempData["ToasterType"] = "error";
                return Json(new { success = false, redirectUrl = "/TableAndSection/TableAndSection" });
            }
            var token = Request.Cookies["JwtToken"];
            var decodedToken = await _commonRepository.ValidateToken(token);
            var userId = decodedToken.UserId;
            AddSection.EditedBy = userId;
            var result = await _tableAndSectionRepository.EditSection(AddSection);
            switch (result)
            {
                case "Section with the name already exist":
                    TempData["ToasterMessage"] = result;
                    TempData["ToasterType"] = "error";
                    return Json(new { success = false, redirectUrl = "/TableAndSection/TableAndSection" });
                default:
                    TempData["ToasterMessage"] = result;
                    TempData["ToasterType"] = "success";
                    return Json(new { success = true, redirectUrl = "/TableAndSection/TableAndSection" });
            }
        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/TableAndSection/TableAndSection" });
        }
    }
    [CustomAuthorise(new string[] { "SuperAdmin","AccountManager"},"Tables and Sections", "Delete")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> DeleteSection(string sectionId)
    {
        try
        {
            var result = await _tableAndSectionRepository.DeleteSection(sectionId);
            if (result == "section Deleted Successfully")
            {
                TempData["ToasterMessage"] = result;
                TempData["ToasterType"] = "success";
                return Json(new { success = true, redirectUrl = "/TableAndSection/TableAndSection" });   
            }
            TempData["ToasterMessage"] = result;
            TempData["ToasterType"] = "error";
            return Json(new { success = false, redirectUrl = "/TableAndSection/TableAndSection" });
        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/TableAndSection/TableAndSection" });
        }
    }
    [HttpPost]
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "Tables and Sections", "AddOrEdit")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> AddTable(TableViewModel AddTable){
       try
        {
            if (!ModelState.IsValid)
            {
                TempData["ToasterMessage"] = "Model state is not valid";
                TempData["ToasterType"] = "error";
                return Json(new { success = false, redirectUrl = "/TableAndSection/TableAndSection" });
            }
            var token = Request.Cookies["JwtToken"];
            var decodedToken = await _commonRepository.ValidateToken(token);
            var userId = decodedToken.UserId;
            AddTable.CreatedBy = userId;
            var result = await _tableAndSectionRepository.AddTable(AddTable);
            if (result == "Table Added Successfully")
            {
                TempData["ToasterMessage"] = result;
                TempData["ToasterType"] = "success";
                return Json(new { success = true, redirectUrl = "/TableAndSection/TableAndSection" });
            }
            TempData["ToasterMessage"] = result;
            TempData["ToasterType"] = "error";
            return Json(new { success = false, redirectUrl = "/TableAndSection/TableAndSection" });
        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/TableAndSection/TableAndSection" });
        } 
    }

    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "Tables and Sections", "AddOrEdit")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> GetTableForEdit(string tableId){
        try
        {
            var table = await _tableAndSectionRepository.GetTableForEdit(tableId);
            return Json(table);
        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/TableAndSection/TableAndSection" });
        }
    }
    [HttpPost]
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "Tables and Sections", "AddOrEdit")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> EditTable(TableViewModel AddTable){
        try
        {
            if (!ModelState.IsValid)
            {
                TempData["ToasterMessage"] = "Model state is not valid";
                TempData["ToasterType"] = "error";
                return Json(new { success = false, redirectUrl = "/TableAndSection/TableAndSection" });
            }
            var token = Request.Cookies["JwtToken"];
            var decodedToken = await _commonRepository.ValidateToken(token);
            var userId = decodedToken.UserId;
            AddTable.EditedBy = userId;
            var result = await _tableAndSectionRepository.EditTable(AddTable);
            switch (result)
            {
                case "Table with the name already exist":
                    TempData["ToasterMessage"] = result;
                    TempData["ToasterType"] = "error";
                    return Json(new { success = false, redirectUrl = "/TableAndSection/TableAndSection" });
                default:
                    TempData["ToasterMessage"] = result;
                    TempData["ToasterType"] = "success";
                    return Json(new { success = true, redirectUrl = "/TableAndSection/TableAndSection" });
            }
        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/TableAndSection/TableAndSection" });
        }
    }
    [HttpPost]
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "Tables and Sections", "Delete")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> DeleteTable(List<string> tableIds){
        try
        {
            var result = await _tableAndSectionRepository.DeleteTable(tableIds);
            TempData["ToasterMessage"] = result;
            TempData["ToasterType"] = "success";
            return Json(new { success = true,redirectUrl = "/TableAndSection/TableAndSection" });
        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/TableAndSection/TableAndSection"});
        }
    }
}
