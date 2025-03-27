using Microsoft.AspNetCore.Mvc;
using PizzaShop.Domain.DataModels;

namespace PizzaShop.Service.Interface;

public interface IAuthTokenService
    {
         string GenerateJwtToken(User user, bool rememberMe);
        int ExtractRoleFromToken(string token);
        Task<IActionResult> RedirectUser(int userId);
    }