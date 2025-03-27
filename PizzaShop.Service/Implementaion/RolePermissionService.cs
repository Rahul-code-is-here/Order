using PizzaShop.Domain.ViewModels;
using PizzaShop.Repository.Interfaces;
using PizzaShop.Service.Interface;
using PizzaShop.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PIZZASHOP.Services.Implementations;

public class RolePermissionService : IRolePermissionService
{
    private readonly IRolePermissionRepository _rolePermissionRepository;

    public RolePermissionService(IRolePermissionRepository rolePermissionRepository)
    {
        _rolePermissionRepository = rolePermissionRepository;
    }

    public async Task<List<RoleViewModel>> GetRolesAsync()
    {
        return await _rolePermissionRepository.GetRolesAsync();
    }

    public async Task<List<PermissionViewModel>> GetPermissionsAsync()
    {
        return await _rolePermissionRepository.GetPermissionsAsync();
    }

    public async Task<List<RolePermissionViewModel>> GetRolePermissionsAsync(int roleId)
    {
        return await _rolePermissionRepository.GetRolePermissionsAsync(roleId);
    }

    public async Task<bool> UpdateRolePermissionsAsync(List<RolePermissionViewModel> rolePermissions)
    {
        return await _rolePermissionRepository.UpdateRolePermissionsAsync(rolePermissions);
    }
}

