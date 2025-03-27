
using Microsoft.AspNetCore.Mvc;
using PizzaShop.Domain.ViewModels;
using PizzaShop.Repository.Interface;
using PizzaShop.Service.Interface;

namespace PizzaShop.Web.Controllers;

public class TaxAndFeeController : Controller
{
    private readonly ITaxAndFeeRepository _taxAndFeeRepository;

    private readonly ITaxAndFeeService _taxAndService;

    private readonly IUserServices _userServices;

    public TaxAndFeeController(ITaxAndFeeRepository taxAndFeeRepository, ITaxAndFeeService taxAndFeeService, IUserServices userServices)
    {
        _taxAndFeeRepository = taxAndFeeRepository;
        _taxAndService = taxAndFeeService;
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

    public async Task<IActionResult> TaxAndFeeView()
    {
        await SetUserProfileInViewBag();
        return View();
    }
    // for fetching the taxList
    public async Task<IActionResult> TaxList(Pagination<TaxAndFeeViewModel> taxList)
    {
        try
        {
            // var data = await _taxAndFeeRepository.GetTaxList(taxList);
            var data = await _taxAndService.GetTaxList(taxList);
            return PartialView("_TaxPartialView", data);
        }
        catch (Exception ex)
        {

            TempData["error"] = ex.Message;

            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/TaxAndFee/TaxAndFeeView" });
        }
    }
    // for add tax
    [HttpPost]
    public async Task<IActionResult> AddTax(TaxAndFeeViewModel addTax)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                TempData["errror"] = "modal state is not valid";

                return Json(new { success = false, redirectUrl = "/TaxAndFee/TaxAndFeeView" });
            }
            // var token = Request.Cookies["JwtToken"];

            // var userId = decodedToken.UserId;
            // addTax.CreatedBy = userId;
            // var result = await _taxAndFeeRepository.AddTax(addTax);
            var result = await _taxAndService.AddTax(addTax);
            if (result == "Tax Added Successfully")
            {
                TempData["success"] = result;

                return Json(new { success = true, redirectUrl = "/TaxAndFee/TaxAndFeeView" });
            }
            TempData["error"] = result;

            return Json(new { success = false, redirectUrl = "/TaxAndFee/TaxAndFeeView" });
        }
        catch (Exception ex)
        {
            TempData["error"] = "An Unexpected error got occured";

            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/TaxAndFee/TaxAndFeeView" });
        }
    }
    //fetch tax for edit
    public async Task<IActionResult> GetTaxForEdit(string taxId)
    {
        try
        {
            // var tax = await _taxAndFeeRepository.GetTaxForEdit(taxId);
            var tax = await _taxAndService.GetTaxForEdit(taxId);
            return Json(tax);
        }
        catch (Exception ex)
        {
            TempData["error"] = "An Unexpected error got occured";

            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/TaxAndFee/TaxAndFeeView" });
        }
    }
    [HttpPost]
    public async Task<IActionResult> EditTax(TaxAndFeeViewModel editTax)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Model state is not valid";

                return Json(new { success = false, redirectUrl = "/TaxAndFee/TaxAndFeeView" });
            }
            // var token = Request.Cookies["JwtToken"];

            // var userId = decodedToken.UserId;
            // editTax.EditedBy = userId;
            // var result = await _taxAndFeeRepository.EditTax(editTax);
            var result = await _taxAndService.EditTax(editTax);
            switch (result)
            {
                case "tax with the name already exist":
                    TempData["error"] = result;

                    return Json(new { success = false, redirectUrl = "/TaxAndFee/TaxAndFeeView" });
                default:
                    TempData["success"] = result;

                    return Json(new { success = true, redirectUrl = "/TaxAndFee/TaxAndFeeView" });
            }
        }
        catch (Exception ex)
        {
            TempData["error"] = "An Unexpected error got occured";

            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/TaxAndFee/TaxAndFeeView" });
        }
    }

    public async Task<IActionResult> DeleteTax(string taxId)
    {
        try
        {
            // var result = await _taxAndFeeRepository.DeleteTax(taxId);
            var result = await _taxAndService.DeleteTax(taxId);
            if (result == "tax Deleted Successfully")
            {
                TempData["success"] = result;

                return Json(new { success = true, redirectUrl = "/TaxAndFee/TaxAndFeeView" });
            }
            TempData["error"] = result;

            return Json(new { success = false, redirectUrl = "/TaxAndFee/TaxAndFeeView" });
        }
        catch (Exception ex)
        {
            TempData["error"] = "An Unexpected error got occured";

            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/TaxAndFee/TaxAndFeeView" });
        }
    }
}