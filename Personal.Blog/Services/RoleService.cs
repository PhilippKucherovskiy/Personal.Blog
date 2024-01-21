using Microsoft.EntityFrameworkCore;
using Personal.Blog.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Personal.Blog.Services
{
    public class RoleService : IRoleService
    {
        private readonly ApplicationDbContext _context;

        public RoleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role> GetRoleByIdAsync(int roleId)
        {
            return await _context.Roles.FindAsync(roleId);
        }

        public async Task CreateRoleAsync(Role role)
        {
            _context.Add(role);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateRoleAsync(Role role)
        {
            try
            {
                _context.Update(role);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await RoleExistsAsync(role.Id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<bool> DeleteRoleAsync(int roleId)
        {
            var role = await _context.Roles.FindAsync(roleId);
            if (role != null)
            {
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> RoleExistsAsync(int roleId)
        {
            return await _context.Roles.AnyAsync(e => e.Id == roleId);
        }
    }
}
