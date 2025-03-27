

using Microsoft.EntityFrameworkCore;
using PizzaShop.Domain.DataContext;
using PizzaShop.Domain.DataModels;
using PizzaShop.Domain.ViewModels;
using PizzaShop.Repository.Interface;
using System.Linq;
using System.Linq.Expressions;

namespace PizzaShop.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly PizzaShemaContext _context;

        public UserRepository(PizzaShemaContext context)
        {
            _context = context;
        }



        public User Get(Expression<Func<User, bool>> predicate)
        {
            return _context.Users.FirstOrDefault(predicate);
        }

        // public async Task<User> GetEmailAndPassword(string email, string password)
        // {
        //     return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password && u.IsDeleted == false && u.Status == "Active");
        // }

        public async Task<User> GetEmailAndPassword(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.IsDeleted == false && u.Status == "Active");
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return user;
            }
            return null;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByEmail(string email, int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Id != userId);
        }
        public async Task<bool> UpdateUserAsync(User user)
        {
            try
            {
                user.Isfirstlogin = false;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public IQueryable<User> GetUsers()
        {
            return _context.Users.AsQueryable();
        }

        public IQueryable<Country> GetCountries()
        {
            return _context.Countries.OrderBy(c => c.CountryName).AsQueryable();
        }

        public IQueryable<State> GetStates(int countryId)
        {
            return _context.States.Where(s => s.CountryId == countryId).OrderBy(s => s.StateName).AsQueryable();
        }

        public IQueryable<City> GetCities(int stateId)
        {
            return _context.Cities.Where(c => c.StateId == stateId).OrderBy(c => c.CityName).AsQueryable();
        }


        public async Task<List<UserListModel>> GetUsersAsync(Expression<Func<User, bool>> predicate, int pageNumber, int pageSize, string sortBy, string sortOrder)
        {
            var query = _context.Users
                .Where(predicate)
                .Select(u => new UserListModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Phone = u.Phone,
                    RoleId = u.RoleId,
                    ProfileImagePath = u.ProfileImage,
                    IsDeleted = u.IsDeleted,
                    status = u.Status
                });

            // Sorting Logic
            query = sortBy switch
            {
                "Email" => (sortOrder == "asc") ? query.OrderBy(u => u.Email) : query.OrderByDescending(u => u.Email),
                "Phone" => (sortOrder == "asc") ? query.OrderBy(u => u.Phone) : query.OrderByDescending(u => u.Phone),
                "Role" => (sortOrder == "asc") ? query.OrderBy(u => u.RoleId) : query.OrderByDescending(u => u.RoleId),
                _ => (sortOrder == "asc") ? query.OrderBy(u => u.FirstName).ThenBy(u => u.LastName) : query.OrderByDescending(u => u.FirstName).ThenByDescending(u => u.LastName)
            };

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalUsersAsync(Expression<Func<User, bool>> predicate)
        {
            return await _context.Users.CountAsync(predicate);
        }

        public async Task<bool> SoftDeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<EditUserModel> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return null;
            // var status1 = user.Status;
            // if (status1 == "1" || status1 == "Active")
            // {
            //     status1 = "Active";
            // }
            // if(status1 == "2" || status1 == "Inactive")
            // {
            //     status1 = "Inactive";
            // }
            return new EditUserModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.Username,
                Email = user.Email,
                Password = user.Password,
                RoleId = user.RoleId,
                CountryID = user.CountryId,
                StateID = user.StateId,
                CityID = user.CityId,
                Address = user.Adress,
                Phone = user.Phone,
                Zipcode = user.Zipcode,
                status = user.Status,
                /////
                ProfileImagePath = user.ProfileImage,
                Countries = await _context.Countries.OrderBy(c => c.CountryName).ToListAsync(),
                States = await _context.States.Where(s => s.CountryId == user.CountryId).OrderBy(s => s.StateName).ToListAsync(),
                Cities = await _context.Cities.Where(c => c.StateId == user.StateId).OrderBy(c => c.CityName).ToListAsync()
            };
        }

        public async Task<bool> UpdateUserAsync(EditUserModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == model.Id);
            if (user == null) return false;

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Username = model.UserName;
            user.Email = model.Email;

            if (!string.IsNullOrEmpty(model.Password))
            {
                user.Password = model.Password;
            }

            user.RoleId = model.RoleId;
            user.CountryId = model.CountryID;
            user.StateId = model.StateID;
            user.CityId = model.CityID;
            user.Adress = model.Address;
            // user.Password=model.Password;
            user.Phone = model.Phone;
            user.Zipcode = model.Zipcode;
            user.Status = model.status;
            user.ProfileImage = model.ProfileImagePath;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Country>> GetCountriesAsync()
        {
            return await _context.Countries.OrderBy(c => c.CountryName).ToListAsync();
        }

        public async Task<List<State>> GetStatesByCountryIdAsync(int countryId)
        {
            return await _context.States.Where(s => s.CountryId == countryId).OrderBy(s => s.StateName).ToListAsync();
        }

        public async Task<List<City>> GetCitiesByStateIdAsync(int stateId)
        {
            return await _context.Cities.Where(c => c.StateId == stateId).OrderBy(c => c.CityName).ToListAsync();
        }



        public async Task<User> GetUserByIdAsyncs(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.IsDeleted == false && u.Status == "Active");
        }

        public async Task<User> GetUserByUsernameAsync(string username, int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.IsDeleted == false && u.Status == "Active" && u.Id != userId);
        }

        public async Task<bool> isFirstLogin(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Isfirstlogin == true);
            if (user == null) return false;

            // var result= _context.Users.Where(u => u.Email == email && u.IsFirstLogin == true).FirstOrDefault();
            // user.Isfirstlogin = false;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<int> GetTotalUsersCountAsync()
        {
            return await _context.Users.Where(u => !u.IsDeleted).CountAsync();
        }

        public async Task<List<RolePermissionViewModel>> GetPermissionsByRoleAsync(string roleName)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role == null) return new List<RolePermissionViewModel>();

            var rolePermissions = await _context.RolePermissions
                .Where(rp => rp.RoleId == role.Id)
                .Select(rp => new RolePermissionViewModel
                {
                    PermissionId = rp.PermissionId,
                    RoleId = rp.RoleId,
                    CanView = rp.CanView,
                    CanAddEdit = rp.CanAddEdit,
                    CanDelete = rp.CanDelete
                }).ToListAsync();

            return rolePermissions;
        }

        //  Check if user is logging in for the first time
        public async Task<bool> IsFirstLoginAsync(string email)
        {
            if (email == null)
            {
                return false;
            }
            var user = await _context.Users
                .Where(u => u.Email == email)
                .Select(u => u.Isfirstlogin)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return false;
            }

            return (bool)user;
        }

        //  Update the first login status after password change
        public async Task UpdateFirstLoginStatusAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null)
            {
                user.Isfirstlogin = false;
                await _context.SaveChangesAsync();
            }
        }

    }
}