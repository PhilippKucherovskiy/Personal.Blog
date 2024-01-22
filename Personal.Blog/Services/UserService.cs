using global::Personal.Blog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Personal.Blog.ViewModels;
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
            private readonly UserManager<User> _userManager;

            public UserService(ApplicationDbContext context, UserManager<User> userManager)
            {
                _context = context;
                _userManager = userManager;
            }

            public async Task<IEnumerable<UserWithRolesViewModel>> GetAllUsersAsync()
            {
                var users = await _context.Users.ToListAsync();
                var userViewModels = new List<UserWithRolesViewModel>();

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    userViewModels.Add(new UserWithRolesViewModel
                    {
                        Id = user.Id,
                        Username = user.UserName,
                        Email = user.Email,
                        Roles = roles
                    });
                }

                return userViewModels;
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
                return await _context.Users.AnyAsync(e => e.Id == id);
            }
        }
    }

}
