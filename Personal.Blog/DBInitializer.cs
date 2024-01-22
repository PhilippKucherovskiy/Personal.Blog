
using Microsoft.AspNetCore.Identity;
using Personal.Blog.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Personal.Blog
{
    public static class DbInitializer
    {
        public static async Task InitializeRoles(RoleManager<Role> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new Role { Name = "Admin" });
            }
            if (!await roleManager.RoleExistsAsync("Moderator"))
            {
                await roleManager.CreateAsync(new Role { Name = "Moderator" });
            }
            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new Role { Name = "User" });
            }
        }

        public static async Task InitializeUsers(UserManager<User> userManager)
        {
            if (userManager.Users.All(u => u.UserName != "admin@example.com"))
            {
                var adminUser = new User { UserName = "admin@example.com", Email = "admin@example.com" };
                await userManager.CreateAsync(adminUser, "Admin123!");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            if (userManager.Users.All(u => u.UserName != "moderator@example.com"))
            {
                var moderatorUser = new User { UserName = "moderator@example.com", Email = "moderator@example.com" };
                await userManager.CreateAsync(moderatorUser, "Moderator123!");
                await userManager.AddToRoleAsync(moderatorUser, "Moderator");
            }

            
            if (userManager.Users.All(u => u.UserName != "user@example.com"))
            {
                var userUser = new User { UserName = "user@example.com", Email = "user@example.com" };
                await userManager.CreateAsync(userUser, "User123!");
                await userManager.AddToRoleAsync(userUser, "User");
            }
        }
    }

}
