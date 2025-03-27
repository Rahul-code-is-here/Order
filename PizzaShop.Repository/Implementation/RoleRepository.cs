using Microsoft.EntityFrameworkCore;
using PizzaShop.Domain.DataContext;
using PizzaShop.Domain.DataModels;
using PizzaShop.Repository.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PizzaShop.Repository.Implementations
{
    public class RoleRepository : IRoleRepository
    {
        private readonly PizzaShemaContext _context;

        public RoleRepository(PizzaShemaContext context)
        {
            _context = context;
        }

        public async Task<List<Role>> GetRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

          public async Task<List<RolePermission>> GetRolePermissionsAsync(int roleId)
        {
            return await _context.RolePermissions.Where(rp => rp.RoleId == roleId).ToListAsync();
        }

        public async Task UpdateRolePermissionsAsync(List<RolePermission> rolePermissions)
        {
            _context.RolePermissions.UpdateRange(rolePermissions);
            await _context.SaveChangesAsync();
        }
    }
}