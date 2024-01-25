using Microsoft.AspNetCore.Mvc;
using Personal.Blog.ViewModels;
using Personal.Blog.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Personal.Blog.Models;
using NLog;
using ILogger = NLog.ILogger;

namespace Personal.Blog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountApiController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<User> _userManager;
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public AccountApiController(IAccountService accountService, UserManager<User> userManager)
        {
            _accountService = accountService;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            _logger.Info("Register attempt for email: {0}", model.Email);

            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    _logger.Info("User registered successfully: {0}", model.Email);
                    await _userManager.AddToRoleAsync(user, "User");
                    return Ok(new { Message = "Registration successful" });
                }

                return BadRequest(result.Errors);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.LoginUserAsync(model, returnUrl);
                if (result.Succeeded)
                {
                    _logger.Info("Login successful for email: {0}", model.Email);
                    return Ok(new { Message = "Login successful" });
                }
                return BadRequest(new { Message = "Invalid login attempt" });
            }
            return BadRequest(ModelState);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            _logger.Info("User is attempting to logout");

            await _accountService.LogoutUserAsync();

            _logger.Info("User logged out successfully");

            return Ok(new { Message = "Logout successful" });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var token = await _accountService.GenerateForgotPasswordTokenAsync(model);
                if (token != null)
                {
                    
                    return Ok(new { Message = "Password reset email sent." });
                }
                return BadRequest(new { Message = "Error generating password reset token" });
            }
            return BadRequest(ModelState);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            _logger.Info("Attempting to reset password");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _accountService.ResetPasswordAsync(model);
            if (result.Succeeded)
            {
                return Ok(new { Message = "Password reset successfully" });
            }

            return BadRequest(result.Errors);
        }
    }
}
