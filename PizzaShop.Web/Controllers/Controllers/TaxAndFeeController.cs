using BussinessLogicLayer.Interface;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using PizzaShopRMS.Helpers;

namespace PizzaShopRMS.Controllers;

public class TaxAndFeeController :Controller
{
    private readonly ITaxAndFeeRepository _taxAndFeeRepository;
    private readonly ICommonRepository _commonRepository;
    public TaxAndFeeController(ITaxAndFeeRepository taxAndFeeRepository,ICommonRepository commonRepository)
    {
        _taxAndFeeRepository = taxAndFeeRepository;
        _commonRepository = commonRepository;
    }
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "Tax and Fee", "View")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> TaxAndFeeView(){
        return View();
    }
    // for fetching the taxList
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "Tax and Fee", "View")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> TaxList(Pagination<TaxAndFeeViewModel> taxList){
        try
        {
            var data = await _taxAndFeeRepository.GetTaxList(taxList);
            return PartialView("_TaxPartialView", data);
        }
        catch (Exception ex)
        {

            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/TaxAndFee/TaxAndFeeView"});
        }
    }
    // for add tax
    [HttpPost]
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "Tax and Fee", "AddOrEdit")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> AddTax(TaxAndFeeViewModel addTax){
        try
        {
            if (!ModelState.IsValid)
            {
                TempData["ToasterMessage"] = "Model state is not valid";
                TempData["ToasterType"] = "error";
                return Json(new { success = false,  redirectUrl = "/TaxAndFee/TaxAndFeeView" });
            }
            var token = Request.Cookies["JwtToken"];
            var decodedToken = await _commonRepository.ValidateToken(token);
            var userId = decodedToken.UserId;
            addTax.CreatedBy = userId;
            var result = await _taxAndFeeRepository.AddTax(addTax);
            if (result == "Tax Added Successfully")
            {
                TempData["ToasterMessage"] = result;
                TempData["ToasterType"] = "success";
                return Json(new { success = true, redirectUrl = "/TaxAndFee/TaxAndFeeView"  });
            }
            TempData["ToasterMessage"] = result;
            TempData["ToasterType"] = "error";
            return Json(new { success = false, redirectUrl = "/TaxAndFee/TaxAndFeeView"  });
        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/TaxAndFee/TaxAndFeeView"});
        } 
    }
    //fetch tax for edit
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "Tax and Fee", "AddOrEdit")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> GetTaxForEdit(string taxId){
         try
        {
            var tax = await _taxAndFeeRepository.GetTaxForEdit(taxId);
            return Json(tax);
        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/TaxAndFee/TaxAndFeeView"});
        }
    }
   
    [HttpPost]
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "Tax and Fee", "AddOrEdit")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> EditTax(TaxAndFeeViewModel editTax){
        try
        {
            if (!ModelState.IsValid)
            {
                TempData["ToasterMessage"] = "Model state is not valid";
                TempData["ToasterType"] = "error";
                return Json(new { success = false,  redirectUrl = "/TaxAndFee/TaxAndFeeView"});
            }
            var token = Request.Cookies["JwtToken"];
            var decodedToken = await _commonRepository.ValidateToken(token);
            var userId = decodedToken.UserId;
            editTax.EditedBy = userId;
            var result = await _taxAndFeeRepository.EditTax(editTax);
            switch (result)
            {
                case "tax with the name already exist":
                    TempData["ToasterMessage"] = result;
                    TempData["ToasterType"] = "error";
                    return Json(new { success = false, redirectUrl = "/TaxAndFee/TaxAndFeeView"});
                default:
                    TempData["ToasterMessage"] = result;
                    TempData["ToasterType"] = "success";
                    return Json(new { success = true,redirectUrl = "/TaxAndFee/TaxAndFeeView"});
            }
        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/TaxAndFee/TaxAndFeeView" });
        }
    }
    
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "Tax and Fee", "Delete")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> DeleteTax(string taxId){
        try
        {
            var result = await _taxAndFeeRepository.DeleteTax(taxId);
            if (result == "tax Deleted Successfully")
            {
                TempData["ToasterMessage"] = result;
                TempData["ToasterType"] = "success";
                return Json(new { success = true, redirectUrl = "/TaxAndFee/TaxAndFeeView" });   
            }
            TempData["ToasterMessage"] = result;
            TempData["ToasterType"] = "error";
            return Json(new { success = false, redirectUrl = "/TaxAndFee/TaxAndFeeView" });
        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/TaxAndFee/TaxAndFeeView"});
        }
    }
}
