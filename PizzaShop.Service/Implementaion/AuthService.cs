

using Microsoft.IdentityModel.Tokens;
using PizzaShop.Domain.DataModels;
using PizzaShop.Domain.ViewModels;
using PizzaShop.Repository.Interface;
using PizzaShop.Service.Interface;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PizzaShop.Service.Implementaion
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailSender _emailSender;

        private readonly IAuthTokenService _authTokenService;

        public AuthService(IUserRepository userRepository, IEmailSender emailSender, IAuthTokenService authTokenService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _authTokenService = authTokenService;
        }

        public async Task<User> Login(string email, string password)
        {
            return await _userRepository.GetEmailAndPassword(email, password);
        }

        public async Task<User> GetUserAsync(string email)
        {
            return await _userRepository.GetUserByEmail(email);
        }

        public string GetEmailBody(string link)
        {
            return $@"
        <html>
        <head>
            <title>Email Title</title>
        </head>
        <body>
            <div style='background-color: #0073C6; text-align:center; padding: 20px;'>
                <h1>PIZZA SHOP</h1>
            </div>
            <div>Welcome to pizza shop</div>
            <div>Please click <a href='{link}' style='color=blue'>here</a> to reset your account password.</div>
            <div>If you encounter any issue or have any questions, please do not hesitate to contact our support team.</div>
            <div><span style='color:yellow'>Important Note:</span> For security reasons, the link will expire in 24 hours. If you did not request a password reset, please ignore this email or contact our support team.</div>
        </body>
        </html>";
        }

        public async Task<bool> ProcessPasswordResetAsync(User user, string resetToken, string resetLink)
        {
            try
            {
                // Update user with reset token
                user.Resettoken = resetToken;
                user.Resettokenexpiry = DateTime.UtcNow.AddHours(24);

                // Save changes to database
                bool updated = await _userRepository.UpdateUserAsync(user);

                if (!updated)
                    return false;

                // Generate email body
                string emailBody = GetEmailBody(resetLink);

                // Send email
                return await _emailSender.SendEmailAsync(user.Email, "Reset Password", emailBody);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing password reset: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> SendResetPasswordEmailAsync(string email, string resetLink)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null)
            {
                return false;
            }

            // user.Resettoken = Guid.NewGuid().ToString();
            // user.Resettokenexpiry = DateTime.UtcNow.ToLocalTime().AddHours(1);
            // await _userRepository.UpdateUserAsync(user);

            var token = _authTokenService.GenerateJwtToken(user, rememberMe: false);
            // Store the token and its expiry in the database
            user.Resettoken = token;
            user.Resettokenexpiry = DateTime.UtcNow.ToLocalTime().AddHours(1); // Token valid for 1 hour
            await _userRepository.UpdateUserAsync(user);

            string subject = "Reset Password";
            string message = $"Click on this link to reset your password: <a href='{resetLink}'>Reset Password</a>";
            await _emailSender.SendEmailAsync(email, subject, message);

            return true;
        }

        public async Task<bool> ChangePasswordAsync(string email, ChangePasswordModel model)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (!BCrypt.Net.BCrypt.Verify(model.CurrentPassword, user.Password))
            // if (user == null || user.Password != model.CurrentPassword)
            {
                return false;
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            // user.Password = model.NewPassword;
            return await _userRepository.UpdateUserAsync(user);
        }
        // for blaklist token
        public static class TokenBlacklist
        {
            private static HashSet<string> blacklistedTokens = new HashSet<string>();

            public static void Add(string token)
            {
                blacklistedTokens.Add(token);
            }

            public static bool Contains(string token)
            {
                return blacklistedTokens.Contains(token);
            }
        }


        // public async Task<bool> ResetPasswordAsync(string token, string newPassword)
        // {
        //     var handler = new JwtSecurityTokenHandler();
        //     ClaimsPrincipal claimsPrincipal;

        //     try
        //     {
        //         claimsPrincipal = handler.ValidateToken(token, new TokenValidationParameters
        //         {
        //             ValidateIssuer = false,
        //             ValidateAudience = false,
        //             ValidateLifetime = true,
        //             ValidateIssuerSigningKey = true,
        //             IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("dhfdjnfbvdkjcndskfhjuiejedhk#4243hdbdjcnsdsfregrefeds3rewsdsfdssdfsdfsskoik"))
        //         }, out SecurityToken validatedToken);
        //     }
        //     catch (Exception)
        //     {
        //         return false; // Invalid token
        //     }

        //     var emailClaim = claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);
        //     if (emailClaim == null)
        //     {
        //         return false; // Email claim not found
        //     }

        //     var email = emailClaim.Value;
        //     var user = await _userRepository.GetUserByEmail(email);
        //     if (user == null)
        //     {
        //         return false; // User not found
        //     }

        //     // user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
        //     user.Password = newPassword;

        //     var updateResult = await _userRepository.UpdateUserAsync(user);

        //     if (updateResult)
        //     {
        //         // Optionally log or handle the token invalidation
        //         await InvalidateTokenAsync(token);
        //     }

        //     return updateResult;

        //     // return await _userRepository.UpdateUserAsync(user);
        // }
        public async Task<bool> ResetPasswordAsync(string token, string newPassword)
        {
            var handler = new JwtSecurityTokenHandler();
            ClaimsPrincipal claimsPrincipal;

            try
            {
                claimsPrincipal = handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes("dhfdjnfbvdkjcndskfhjuiejedhk#4243hdbdjcnsdsfregrefeds3rewsdsfdssdfsdfsskoik"))
                }, out SecurityToken validatedToken);
            }
            catch (Exception)
            {
                return false; // Invalid token
            }

            var emailClaim = claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);
            if (emailClaim == null)
            {
                return false; // Email claim not found
            }

            var email = emailClaim.Value;
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null)
            {
                return false; // User not found
            }

            // Update user password (ensure to hash the password)
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            // user.Password = newPassword;  
            var updateResult = await _userRepository.UpdateUserAsync(user);

            if (updateResult)
            {
                // Invalidate the token
                TokenBlacklist.Add(token);
            }

            return updateResult;
        }

        // private Task InvalidateTokenAsync(string token)
        // {
        //     // Implement your logic to invalidate the token (e.g., store it in a database or cache)
        //     // For example, add it to a blacklist of invalid tokens.
        //     return Task.CompletedTask;
        // }


        public string GenerateJwtToken(User user, bool rememberMe)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("dhfdjnfbvdkjcndskfhjuiejedhk#4243hdbdjcnsdsfregrefeds3rewsdsfdssdfsdfsskoik");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = rememberMe ? DateTime.UtcNow.AddDays(7) : DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> isFirstLogin(string email)
        {
            bool user = await _userRepository.isFirstLogin(email);
            // if (user == null)
            // {
            //     return false;
            // }

            return user;
    }

    }
}
