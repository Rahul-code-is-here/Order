using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BussinessLogicLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PizzaShopRMS.Controllers;

public class BaseController : Controller
{
    protected string userRole { get; private set; } = "Guest";
    protected string userEmail { get; private set; } = "unknown";
    protected string userName { get; private set; } = "Guest";
    protected string userdata { get; private set; } = "";
    protected string userId { get; private set; } = "";

    protected string userImg {get;private set;} ="";

    private readonly ICommonRepository _commonRepository;

    public BaseController(ICommonRepository commonRepository)
    {
        _commonRepository =commonRepository;
    }

    public override void  OnActionExecuting(ActionExecutingContext context)
    {
        try
        {
            var token = Request.Cookies["JwtToken"];

            if (string.IsNullOrEmpty(token))
            {
                RedirectToLogin(context);
                return;
            }
            var handler = new JwtSecurityTokenHandler();
            var JwtToken = handler.ReadJwtToken(token);

            Console.WriteLine(JwtToken.ValidTo);
            if (JwtToken.ValidTo <DateTime.UtcNow)
            {
                RedirectToLogin(context);
                return;
            }

            userRole = JwtToken.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Role)?.Value ?? "Guest";
            userEmail = JwtToken.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Email)?.Value ?? "unknown";
            userName = JwtToken.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Name)?.Value ?? "Guest";
            // userdata = JwtToken.Claims.FirstOrDefault(p=>p.Type==ClaimTypes.UserData)?.Value??"";
            userImg =  _commonRepository.getCurrentUserImage(userEmail);
            userId = JwtToken.Claims.FirstOrDefault(p=>p.Type==ClaimTypes.Sid)?.Value??"";
            ViewBag.role = userRole;
            ViewBag.email = userEmail;
            ViewBag.UName = userName;
            ViewBag.userImg = userImg;
            ViewBag.userId = userId;
            Console.WriteLine(userRole);
        }
        catch (Exception ex)
        {
            Console.WriteLine("error: while validating try again later");
            RedirectToLogin(context);
        }
    }
    private void RedirectToLogin(ActionExecutingContext context)
    {
        context.Result = new RedirectToRouteResult(new { controller = "Login", action = "Login" });
    }
}
