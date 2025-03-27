using PizzaShop.Domain.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PizzaShop.Services.Interfaces
{
    public interface IRoleService
    {
        Task<List<Role>> GetRolesAsync();
    }
}