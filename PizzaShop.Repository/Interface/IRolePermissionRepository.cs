using PizzaShop.Domain.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PizzaShop.Repository.Interfaces;

public interface IRolePermissionRepository
{
    Task<List<RoleViewModel>> GetRolesAsync();
    Task<List<PermissionViewModel>> GetPermissionsAsync();
    Task<List<RolePermissionViewModel>> GetRolePermissionsAsync(int roleId);
    Task<bool> UpdateRolePermissionsAsync(List<RolePermissionViewModel> rolePermissions);
}
