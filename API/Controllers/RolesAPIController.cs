using Microsoft.AspNetCore.Mvc;
using Personal.Blog.Models;
using Personal.Blog.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesAPIController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesAPIController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRole(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return Ok(role);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(int id, Role role)
        {
            if (id != role.Id)
            {
                return BadRequest();
            }

            await _roleService.UpdateRoleAsync(role);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Role>> PostRole(Role role)
        {
            await _roleService.CreateRoleAsync(role);
            return CreatedAtAction("GetRole", new { id = role.Id }, role);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            await _roleService.DeleteRoleAsync(id);
            return NoContent();
        }
    }
}
