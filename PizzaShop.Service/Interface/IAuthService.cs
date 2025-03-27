using PizzaShop.Domain.DataModels;
using PizzaShop.Domain.ViewModels;

namespace PizzaShop.Service.Interface;

public interface IAuthService
{
    Task<User> Login(string email, string password);
    Task<User> GetUserAsync(string email);
    // string GetEmailBody(string link);

    //  Task<bool> ProcessPasswordResetAsync(User user, string resetToken, string resetLink);
    Task<bool> SendResetPasswordEmailAsync(string email, string resetLink);

     Task<bool> ChangePasswordAsync(string email, ChangePasswordModel model);

      Task<bool> ResetPasswordAsync(string token, string newPassword);

     string GenerateJwtToken(User user, bool rememberMe);

     Task<bool> isFirstLogin(string email);

     
    //    Task<bool> ResetPasswordAsync(string token, string newPassword);
}

