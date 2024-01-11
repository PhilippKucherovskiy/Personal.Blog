using global::Personal.Blog.Models;
using Microsoft.EntityFrameworkCore;
using Personal.Blog.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Personal.Blog.Services
{
  
    namespace Personal.Blog.Services
    {
        public class UserService : IUserService
        {
            private readonly ApplicationDbContext _context;

            public UserService(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<User>> GetAllUsersAsync()
            {
                return await _context.Users.ToListAsync();
            }

            public async Task<User> GetUserByIdAsync(int id)
            {
                return await _context.Users.FindAsync(id);
            }

            public async Task CreateUserAsync(User user)
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            public async Task UpdateUserAsync(User user)
            {
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            public async Task DeleteUserAsync(int id)
            {
                var user = await _context.Users.FindAsync(id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                }
            }

            public async Task<bool> UserExistsAsync(int id)
            {
                return await _context.Users.AnyAsync(e => e.UserId == id);
            }
        }
    }

}
