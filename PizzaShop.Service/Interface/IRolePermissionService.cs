using PizzaShop.Domain.ViewModels;

namespace PizzaShop.Service.Interface;

public interface IRolePermissionService
{
      Task<List<RoleViewModel>> GetRolesAsync();
    Task<List<PermissionViewModel>> GetPermissionsAsync();
    Task<List<RolePermissionViewModel>> GetRolePermissionsAsync(int roleId);
    Task<bool> UpdateRolePermissionsAsync(List<RolePermissionViewModel> rolePermissions);
}
