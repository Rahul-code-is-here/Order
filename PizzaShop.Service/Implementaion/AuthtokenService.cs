using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PizzaShop.Domain.DataModels;
using PizzaShop.Repository.Interface;
using PizzaShop.Service.Interface;

namespace PizzaShop.Service.Implementaion;

public class AuthTokenService : IAuthTokenService
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;

    public AuthTokenService(IConfiguration configuration, IUserRepository userRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
    }



    public string GenerateJwtToken(User user, bool rememberMe)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.RoleId.ToString())
            };

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Issuer"],
            claims,
            expires: rememberMe ? DateTime.UtcNow.AddDays(7) : DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    public async Task<IActionResult> RedirectUser(int userId)
    {

        var user = await _userRepository.GetUserById(userId);
        if (user == null)
        {
            return new RedirectToActionResult("Login", "Home", null);
        }

        return user.RoleId switch
        {
            1 => new RedirectToActionResult("UserList", "Home", null), // Super Admin
            2 => new RedirectToActionResult("Dashboard", "Home", null), // Admin
            3 => new RedirectToActionResult("Dashboard", "Home", null), // Account Manager
            4 => new RedirectToActionResult("Dashboard", "Home", null), // Chef
            _ => new RedirectToActionResult("Login", "Home", null) // Default
        };
    }

    public int ExtractRoleFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

        return roleClaim != null ? int.Parse(roleClaim.Value) : 0;
    }
}