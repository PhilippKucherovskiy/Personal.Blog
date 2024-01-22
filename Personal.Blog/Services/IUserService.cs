using Personal.Blog.Models;
using Personal.Blog.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Personal.Blog.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserWithRolesViewModel>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<bool> UserExistsAsync(int id);
    }
}
