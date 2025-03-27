using PizzaShop.Domain.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PizzaShop.Repository.Interfaces
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetRolesAsync();
         Task<List<RolePermission>> GetRolePermissionsAsync(int roleId);
        Task UpdateRolePermissionsAsync(List<RolePermission> rolePermissions);
    }
}