using Personal.Blog.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Personal.Blog.Services
{
    public interface IRoleService
    {
        Task<List<Role>> GetAllRolesAsync();
        Task<Role> GetRoleByIdAsync(int roleId);
        Task CreateRoleAsync(Role role);
        Task<bool> UpdateRoleAsync(Role role);
        Task<bool> DeleteRoleAsync(int roleId);
        Task<bool> RoleExistsAsync(int roleId);
    }
}
