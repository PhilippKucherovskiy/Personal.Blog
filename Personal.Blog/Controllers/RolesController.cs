using Microsoft.AspNetCore.Mvc;
using Personal.Blog.Models;
using Personal.Blog.Services;
using System.Threading.Tasks;
using NLog;
using ILogger = NLog.ILogger;

namespace Personal.Blog.Controllers
{
    public class RolesController : Controller
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<IActionResult> Index()
        {
            _logger.Info("Accessed Roles Index");
            return View(await _roleService.GetAllRolesAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.Warn("Role Details accessed with null ID");
                return NotFound();
            }

            var role = await _roleService.GetRoleByIdAsync(id.Value);
            if (role == null)
            {
                _logger.Warn($"Role Details not found for ID {id}");
                return NotFound();
            }

            _logger.Info($"Accessed Details for Role ID {id}");
            return View(role);
        }

        public IActionResult Create()
        {
            _logger.Info("Accessed Role Creation Page");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Role role)
        {
            if (ModelState.IsValid)
            {
                await _roleService.CreateRoleAsync(role);
                _logger.Info($"Role created: {role.Name}");
                return RedirectToAction(nameof(Index));
            }

            _logger.Warn("Role creation failed due to invalid model state");
            return View(role);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.Warn("Role Edit accessed with null ID");
                return NotFound();
            }

            var role = await _roleService.GetRoleByIdAsync(id.Value);
            if (role == null)
            {
                _logger.Warn($"Role Edit not found for ID {id}");
                return NotFound();
            }

            _logger.Info($"Accessed Role Edit for ID {id}");
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Role role)
        {
            if (id != role.Id)
            {
                _logger.Warn($"Mismatch in Role ID during edit, expected {id}, got {role.Id}");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _roleService.UpdateRoleAsync(role);
                if (!result)
                {
                    _logger.Warn($"Role update failed for ID {id}");
                    return NotFound();
                }

                _logger.Info($"Role updated: {role.Name}");
                return RedirectToAction(nameof(Index));
            }

            _logger.Warn("Role update failed due to invalid model state");
            return View(role);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.Warn("Role Delete accessed with null ID");
                return NotFound();
            }

            var role = await _roleService.GetRoleByIdAsync(id.Value);
            if (role == null)
            {
                _logger.Warn($"Role Delete not found for ID {id}");
                return NotFound();
            }

            _logger.Info($"Accessed Role Delete for ID {id}");
            return View(role);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _roleService.DeleteRoleAsync(id);
            if (!result)
            {
                _logger.Warn($"Failed to delete Role ID {id}");
                return NotFound();
            }

            _logger.Info($"Role deleted: ID {id}");
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> RoleExists(int id)
        {
            var exists = await _roleService.RoleExistsAsync(id);
            _logger.Info($"Role existence check for ID {id}: {exists}");
            return exists;
        }
    }
}
