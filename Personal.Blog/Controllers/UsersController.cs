using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using Personal.Blog.Models;
using Personal.Blog.Services;
using System.Threading.Tasks;
using ILogger = NLog.ILogger;

namespace Personal.Blog.Controllers
{
    public class UsersController : Controller
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            _logger.Info("Accessed Users Index");
            var usersWithRoles = await _userService.GetAllUsersAsync();
            return View(usersWithRoles);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.Warn("User Details accessed with null ID");
                return NotFound();
            }

            var user = await _userService.GetUserByIdAsync(id.Value);
            if (user == null)
            {
                _logger.Warn($"User Details not found for ID {id}");
                return NotFound();
            }

            _logger.Info($"Accessed Details for User ID {id}");
            return View(user);
        }

        public IActionResult Create()
        {
            _logger.Info("Accessed User Creation Page");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Username,Email,PasswordHash")] User user)
        {
            if (ModelState.IsValid)
            {
                await _userService.CreateUserAsync(user);
                _logger.Info($"User created: {user.UserName}");
                return RedirectToAction(nameof(Index));
            }

            _logger.Warn("User creation failed due to invalid model state");
            return View(user);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.Warn("User Edit accessed with null ID");
                return NotFound();
            }

            var user = await _userService.GetUserByIdAsync(id.Value);
            if (user == null)
            {
                _logger.Warn($"User Edit not found for ID {id}");
                return NotFound();
            }

            _logger.Info($"Accessed User Edit for ID {id}");
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Username,Email,PasswordHash")] User user)
        {
            if (id != user.Id)
            {
                _logger.Warn($"Mismatch in User ID during edit, expected {id}, got {user.Id}");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _userService.UpdateUserAsync(user);
                    _logger.Info($"User updated: {user.UserName}");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _userService.UserExistsAsync(user.Id))
                    {
                        _logger.Warn($"User Edit not found for ID {user.Id}");
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            _logger.Warn("User update failed due to invalid model state");
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.Warn("User Delete accessed with null ID");
                return NotFound();
            }

            var user = await _userService.GetUserByIdAsync(id.Value);
            if (user == null)
            {
                _logger.Warn($"User Delete not found for ID {id}");
                return NotFound();
            }

            _logger.Info($"Accessed User Delete for ID {id}");
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _userService.DeleteUserAsync(id);
            _logger.Info($"User deleted: ID {id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
