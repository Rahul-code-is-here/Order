using System.Security.Claims;
using PizzaShop.Repository.Interface;

namespace PizzaShop.Web.Middlewares;

public class RedirectIfFirstLoginMiddleware
{
    private readonly RequestDelegate _next;

    public RedirectIfFirstLoginMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        //  Check if user is authenticated
        if (context.User.Identity.IsAuthenticated)
        {
            var userRepository = context.RequestServices.GetRequiredService<IUserRepository>();

            // Extract user email from JWT Token
            var email = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (!string.IsNullOrEmpty(email))
            {
                var isFirstLogin = await userRepository.IsFirstLoginAsync(email);

                //  If first login, force redirect to change password
                if (isFirstLogin)
                {
                    if (!context.Request.Path.StartsWithSegments("/Login/ChangePassword"))
                    {
                        context.Response.Redirect("/Login/ChangePassword");
                        return;
                    }
                }
            }
        }

        await _next(context);
    }
}


