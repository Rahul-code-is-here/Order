using PizzaShop.Domain.DataModels;
using PizzaShop.Repository.Interfaces;
using PizzaShop.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PizzaShop.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<List<Role>> GetRolesAsync()
        {
            return await _roleRepository.GetRolesAsync();
        }
    }
}