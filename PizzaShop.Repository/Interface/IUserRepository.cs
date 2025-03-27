
using PizzaShop.Domain.DataModels;
using PizzaShop.Domain.ViewModels;
using System.Linq.Expressions;

namespace PizzaShop.Repository.Interface
{
    public interface IUserRepository
    {
        // Task<List<UserListModel>> GetUsersAsync(Expression<Func<UserListModel, bool>> predicate, int pageNumber, int pageSize, string sortBy, string sortOrder);
        // Task<int> GetTotalUsersAsync(Expression<Func<UserListModel, bool>> predicate);

          void AddUser(User user);
        
        User Get(Expression<Func<User, bool>> predicate);
        Task<User> GetEmailAndPassword(string email, string password);
        Task<User> GetUserByEmail(string email);

        Task<User> GetUserByEmail(string email, int userId);
        Task<bool> UpdateUserAsync(User user);
        Task<User> GetUserById(int userId);
        IQueryable<User> GetUsers();
        IQueryable<Country> GetCountries();   
        IQueryable<State> GetStates(int countryId);
        IQueryable<City> GetCities(int stateId);

         Task<List<UserListModel>> GetUsersAsync(Expression<Func<User, bool>> predicate, int pageNumber, int pageSize, string sortBy, string sortOrder);
        Task<int> GetTotalUsersAsync(Expression<Func<User, bool>> predicate);

        Task<bool> SoftDeleteUserAsync(int userId);

        Task<EditUserModel> GetUserByIdAsync(int id);
        Task<bool> UpdateUserAsync(EditUserModel model);
        Task<List<Country>> GetCountriesAsync();
        Task<List<State>> GetStatesByCountryIdAsync(int countryId);
        Task<List<City>> GetCitiesByStateIdAsync(int stateId);

        public  Task<User> GetUserByIdAsyncs(int id);

        public Task<User> GetUserByUsernameAsync(string username);

           public Task<User> GetUserByUsernameAsync(string username, int userId);

        public Task<bool> isFirstLogin(string email);

    Task<int> GetTotalUsersCountAsync();

    Task<List<RolePermissionViewModel>> GetPermissionsByRoleAsync(string roleName);

    Task<bool> IsFirstLoginAsync(string email);

    Task UpdateFirstLoginStatusAsync(string email);
  }
}