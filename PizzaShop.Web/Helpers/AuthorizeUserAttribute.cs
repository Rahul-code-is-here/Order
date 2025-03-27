using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PizzaShop.Service.Implementation;

namespace PizzaShop.Web.Helpers;


    public class AuthorizeUserAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var token = context.HttpContext.Request.Cookies["AuthToken"];

            // If token is missing, redirect to login
            if (string.IsNullOrEmpty(token))
            {
                context.Result = new RedirectToActionResult("Login", "Login", null);
                return;
            }

            // Optional: Check token validity (if needed)
            var userService = context.HttpContext.RequestServices.GetService(typeof(UserServices)) as UserServices;
            try
            {
                var email = userService?.ExtractEmailFromToken(token);
                if (string.IsNullOrEmpty(email))
                {
                    context.Result = new RedirectToActionResult("Login", "Login", null);
                }
            }
            catch (Exception)
            {
                context.Result = new RedirectToActionResult("Login", "Login", null);
            }
        }
    }
    