


using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PizzaShop.Domain.DataModels;
using PizzaShop.Domain.ViewModels;
using PizzaShop.Repository.Interface;
using PizzaShop.Service.Interface;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PizzaShop.Service.Implementation
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailSender _emailSender;

        private readonly IWebHostEnvironment env;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserServices(IUserRepository userRepository, IEmailSender emailSender, IWebHostEnvironment webHostEnvironment, IWebHostEnvironment env)
        {
            _userRepository = userRepository;
            _emailSender = emailSender;
            _webHostEnvironment = webHostEnvironment;
            this.env = env;
        }


        public async Task<UpdateProfileModel> GetUserProfileAsync(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null) return null;
            var status1 = user.Status;
            if (status1 == "1")
            {
                status1 = "Active";
            }
            else
            {
                status1 = "Inactive";
            }
            var model = new UpdateProfileModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.Username,
                Phone = user.Phone,
                CountryID = user.CountryId,
                StateID = user.StateId,
                CityID = user.CityId,
                Address = user.Adress,
                Zipcode = user.Zipcode,
                status = status1,
                // ProfileImageUrl = user.ProfileImage,
                PathOfProfilePicture = user.ProfileImage,

                // Populate dropdowns
                Countries = await _userRepository.GetCountries().ToListAsync(),
                States = await _userRepository.GetStates(user.CountryId).ToListAsync(),
                Cities = await _userRepository.GetCities(user.StateId).ToListAsync()
            };

            return model;
        }

        public async Task<bool> UpdateUserProfileAsync(UpdateProfileModel model, string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null) return false;

            // Updating user details
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Username = model.UserName;
            user.Phone = model.Phone;
            user.CountryId = model.CountryID;
            user.StateId = model.StateID;
            user.CityId = model.CityID;
            user.Adress = model.Address;
            user.Zipcode = model.Zipcode;
            // user.ProfileImage = model.ProfileImageUrl;
            if (model.ProfilePicture != null && model.ProfilePicture.Length > 0)
            {
                string Location = "uploads\\ProfileImages\\";
                string path = UploadFile(env, model.ProfilePicture, Location, model.UserName);
                user.ProfileImage = path;
            }




            return await _userRepository.UpdateUserAsync(user);
        }

        public string UploadFile(IWebHostEnvironment env, IFormFile file, string location, string file_name)
        {
            string path = Path.Combine(env.WebRootPath, location);
            string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + file_name + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(path, fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            string finalPath = "/" + location + fileName;
            return finalPath;
        }


        public async Task<List<StateViewModel>> GetStatesAsync(int countryId)
        {
            var states = await _userRepository.GetStates(countryId).ToListAsync();
            return states.Select(s => new StateViewModel
            {
                StateId = s.StateId,
                StateName = s.StateName
            }).ToList();
        }

        public async Task<List<CityViewModel>> GetCitiesAsync(int stateId)
        {
            var cities = await _userRepository.GetCities(stateId).ToListAsync();
            return cities.Select(c => new CityViewModel
            {
                CityId = c.CityId,
                CityName = c.CityName
            }).ToList();
        }

        // public async Task<(List<UserListModel> Users, int TotalCount)> GetUserListAsync(string searchQuery, int pageNumber, int pageSize, string sortBy, string sortOrder)
        // {
        //     Expression<Func<User, bool>> predicate = string.IsNullOrEmpty(searchQuery)
        //         ? (u => !u.IsDeleted)
        //         : (u => !u.IsDeleted && (u.FirstName.Contains(searchQuery) || u.LastName.Contains(searchQuery) || u.Email.Contains(searchQuery) || u.Phone.Contains(searchQuery)));

        //     var users = await _userRepository.GetUsersAsync(predicate, pageNumber, pageSize, sortBy, sortOrder);
        //     var totalCount = await _userRepository.GetTotalUsersAsync(predicate);

        //     return (users, totalCount);
        // }

        public async Task<(List<UserListModel> Users, int TotalCount)> GetUserListAsync(string searchQuery, int pageNumber, int pageSize, string sortBy, string sortOrder)
        {
            // Normalize the search query by trimming whitespace
            var trimmedSearchQuery = string.IsNullOrWhiteSpace(searchQuery) ? string.Empty : searchQuery.Trim();

            // Define the predicate based on the search query
            Expression<Func<User, bool>> predicate = string.IsNullOrEmpty(trimmedSearchQuery)
                ? (u => !u.IsDeleted)
                : (u => !u.IsDeleted &&
                         (u.FirstName.ToLower().Contains(trimmedSearchQuery.ToLower()) ||
                          u.LastName.ToLower().Contains(trimmedSearchQuery.ToLower()) ||
                          String.Concat(u.FirstName, " ", u.LastName).ToLower().Contains(trimmedSearchQuery.ToLower()) ||
                          u.Email.ToLower().Contains(trimmedSearchQuery.ToLower()) ||
                          u.Phone.ToLower().Contains(trimmedSearchQuery.ToLower())));

            // Fetch the users based on the predicate, pagination, and sorting
            var users = await _userRepository.GetUsersAsync(predicate, pageNumber, pageSize, sortBy, sortOrder);


            // Get the total count of users that match the predicate
            var totalCount = await _userRepository.GetTotalUsersAsync(predicate);

            return (users, totalCount);
        }


        public async Task<bool> SoftDeleteUserAsync(int userId)
        {
            return await _userRepository.SoftDeleteUserAsync(userId);
        }


        public async Task<EditUserModel> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }



        public async Task<List<Country>> GetCountriesAsync()
        {
            return await _userRepository.GetCountriesAsync();
        }



        public async Task<bool> UpdateUserAsync(EditUserModel model)
        {


            var user = await _userRepository.GetUserByIdAsync(model.Id);
            if (user == null) return false;

            var status1 = model.status;
            if (status1 == "1")
            {
                status1 = "Active";
            }
            if (status1 == "2")
            {
                status1 = "InActive";
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.UserName=model.UserName;
            user.Email = model.Email;
            user.Phone = model.Phone;
            user.RoleId = model.RoleId;
            user.Password = model.Password;
            user.CountryID = model.CountryID;
            user.StateID = model.StateID;
            user.CityID = model.CityID;
            user.Address = model.Address;
            user.CityID = model.CityID;
            user.Zipcode = model.Zipcode;
            user.status = status1;
            // user.IsDeleted = model.IsDeleted;
            if (model.ProfileImage != null && model.ProfileImage.Length > 0)
            {
                string Location = "uploads\\ProfileImages\\";
                string path = UploadFile(env, model.ProfileImage, Location, model.UserName);
                user.ProfileImagePath = path;
            }

            return await _userRepository.UpdateUserAsync(user);
        }

        public async void AddUser(UserModel model)
        {

            var newUser = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Username = model.UserName,
                Email = model.Email,
                Password = model.Password,
                RoleId = model.RoleId,
                CountryId = model.CountryID,
                StateId = model.StateID,
                CityId = model.CityID,
                Phone = model.PhoneNumber,
                Adress = model.Address,
                Zipcode = model.Zipcode,
                ProfileImage = model.ProfileImagePath,
                // ProfileImage = model.ProfilePicture

            };

            if (model.ProfileImage != null)
            {
                string uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/users");


                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfileImage.FileName;
                string filePath = Path.Combine(uploads, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfileImage.CopyToAsync(fileStream);
                }

                newUser.ProfileImage = "/images/users/" + uniqueFileName;
            }

            _userRepository.AddUser(newUser);
        }

        public async Task<bool> SendWelcomeEmailAsync(string email, string password)
        {

            var subject = "Reset Password";

            var templatePath = Path.Combine(_webHostEnvironment.WebRootPath, "EmailTemplate", "NewUserEmail.html");
            if (!System.IO.File.Exists(templatePath))
            {
                throw new FileNotFoundException("Template file not found", templatePath);
            }
            var templateContent = await System.IO.File.ReadAllTextAsync(templatePath);
            string message = templateContent.Replace("{{pass}}", password).Replace("{{username}}", email);
            // var htmlMessage = $"<p>Dear User,</p><p>Your account has been created successfully.</p><p>Email: {email}</p><p>Password: {password}</p><p>Thank you for joining us!</p>";
            return await _emailSender.SendEmailAsync(email, subject, message);
        }

        public async Task<User> GetCurrentUserAsync(string email)
        {
            return await _userRepository.GetUserByEmail(email);
        }

          public async Task<User> GetCurrentUserAsync(string email, int userId)
        {
            return await _userRepository.GetUserByEmail(email, userId);
        }

        // public string ExtractEmailFromToken(string token)
        // {
        //     if (string.IsNullOrEmpty(token))
        //     {
        //         return null;
        //     }
        //     var handler = new JwtSecurityTokenHandler();
        //     var jwtToken = handler.ReadJwtToken(token);
        //     var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

        //     return emailClaim?.Value;
        // }
        public string ExtractEmailFromToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null; // Instead of throwing an exception, return null
            }

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                return emailClaim?.Value;
            }
            catch (Exception)
            {
                return null; // If token is invalid, return null instead of throwing an error
            }
        }



        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.GetUserByUsernameAsync(username);
        }

        public async Task<User> GetUserByUsernameAsync(string username, int userId)
        {
            return await _userRepository.GetUserByUsernameAsync(username, userId);
        }

        public async Task<int> GetTotalUsersCountAsync()
        {
            return await _userRepository.GetTotalUsersCountAsync();
        }
        public async Task<List<RolePermissionViewModel>> GetPermissionsByRoleAsync(string roleName)
        {
            return await _userRepository.GetPermissionsByRoleAsync(roleName);
        }

        public async Task<bool> IsFirstLoginAsync(string email)
        {
            return await _userRepository.IsFirstLoginAsync(email);
        }

        public async Task UpdateFirstLoginStatusAsync(string email)
        {
            await _userRepository.UpdateFirstLoginStatusAsync(email);
        }

    }
}