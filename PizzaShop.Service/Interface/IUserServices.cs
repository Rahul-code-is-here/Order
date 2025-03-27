
using PizzaShop.Domain.DataModels;
using PizzaShop.Domain.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PizzaShop.Service.Interface
{
    public interface IUserServices
    {
        Task<UpdateProfileModel> GetUserProfileAsync(string email);
        Task<bool> UpdateUserProfileAsync(UpdateProfileModel model, string email);
        Task<List<StateViewModel>> GetStatesAsync(int countryId);
        Task<List<CityViewModel>> GetCitiesAsync(int stateId); 
      
         Task<(List<UserListModel> Users, int TotalCount)> GetUserListAsync(string searchQuery, int pageNumber, int pageSize, string sortBy, string sortOrder);

         Task<bool> SoftDeleteUserAsync(int userId);

         Task<EditUserModel> GetUserByIdAsync(int id);
        Task<bool> UpdateUserAsync(EditUserModel model);
        Task<List<Country>> GetCountriesAsync();

         void AddUser(UserModel model);

          Task<bool> SendWelcomeEmailAsync(string email, string password); 
          Task<User> GetCurrentUserAsync(string email);

          Task<User> GetCurrentUserAsync(string email, int userId);

          string ExtractEmailFromToken(string token);

          Task<User> GetUserByUsernameAsync(string username);

          Task<User> GetUserByUsernameAsync(string username, int userId);

        Task<int> GetTotalUsersCountAsync();

        Task<List<RolePermissionViewModel>> GetPermissionsByRoleAsync(string roleName);


        Task<bool> IsFirstLoginAsync(string email);
        Task UpdateFirstLoginStatusAsync(string email);
    }
}