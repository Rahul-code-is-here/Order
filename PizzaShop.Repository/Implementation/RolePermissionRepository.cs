using Microsoft.EntityFrameworkCore;
using PizzaShop.Domain.DataContext;
using PizzaShop.Domain.DataModels;
using PizzaShop.Domain.ViewModels;
using PizzaShop.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaShop.Repository.Implementations;

public class RolePermissionRepository : IRolePermissionRepository
{
    private readonly PizzaShemaContext _context;

    public RolePermissionRepository(PizzaShemaContext context)
    {
        _context = context;
    }

    public async Task<List<RoleViewModel>> GetRolesAsync()
    {
        return await _context.Roles
            .Select(r => new RoleViewModel { Id = r.Id, Name = r.Name })
            .ToListAsync();
    }

    public async Task<List<PermissionViewModel>> GetPermissionsAsync()
    {
        return await _context.Permissions
            .Select(p => new PermissionViewModel { Id = p.Id, Name = p.Name })
            .ToListAsync();
    }

    public async Task<List<RolePermissionViewModel>> GetRolePermissionsAsync(int roleId)
    {
        var rolePermissions = await _context.RolePermissions
            .Where(rp => rp.RoleId == roleId)
            .ToListAsync();

        return rolePermissions.Select(rp => new RolePermissionViewModel
        {
            PermissionId = rp.PermissionId,
            RoleId = rp.RoleId,
            CanView = rp.CanView,
            CanAddEdit = rp.CanAddEdit,
            CanDelete = rp.CanDelete
        }).ToList();
    }

    public async Task<bool> UpdateRolePermissionsAsync(List<RolePermissionViewModel> rolePermissions)
    {
        foreach (var permission in rolePermissions)
        {
            var existingPermission = await _context.RolePermissions
                .FirstOrDefaultAsync(rp => rp.RoleId == permission.RoleId && rp.PermissionId == permission.PermissionId);

            if (existingPermission != null)
            {
                existingPermission.CanView = (bool)permission.CanView;
                existingPermission.CanAddEdit = (bool)permission.CanAddEdit;
                existingPermission.CanDelete = (bool)permission.CanDelete;
                _context.RolePermissions.Update(existingPermission);
            }
            else
            {
                await _context.RolePermissions.AddAsync(new RolePermission
                {
                    RoleId = permission.RoleId,
                    PermissionId = (int)permission.PermissionId,
                    CanView = (bool)permission.CanView,
                    CanAddEdit = (bool)permission.CanAddEdit,
                    CanDelete = (bool)permission.CanDelete
                });
            }
        }

        await _context.SaveChangesAsync();
        return true;
    }
}
