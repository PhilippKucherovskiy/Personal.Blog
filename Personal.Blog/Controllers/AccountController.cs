using Microsoft.AspNetCore.Mvc;
using Personal.Blog.ViewModels;
using Personal.Blog.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Personal.Blog.Models;
using NLog;
using ILogger = NLog.ILogger;

namespace Personal.Blog.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<User> _userManager;
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public AccountController(IAccountService accountService, UserManager<User> userManager)
        {
            _accountService = accountService;
            _userManager = userManager; 
        }

        [HttpGet]
        public IActionResult Register()
        {

            _logger.Info("Displaying Register view");

            return View();

        }

        [HttpPost]
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
                    return RedirectToAction("index", "home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    _logger.Error("Error during registration for email {0}: {1}", model.Email, error.Description);
                }
            }
            else
            {
                _logger.Warn("Registration attempt failed due to invalid model state for email: {0}", model.Email);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            _logger.Info("Displaying Login view");
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.LoginUserAsync(model, returnUrl);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    var roles = await _userManager.GetRolesAsync(user);

                    var claims = new List<Claim>();
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var userIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                    await HttpContext.SignInAsync(principal);

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("index", "home");
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            _logger.Info("User is attempting to logout");

            await _accountService.LogoutUserAsync();

            _logger.Info("User logged out successfully");

            return RedirectToAction("index", "home");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            _logger.Info("Displaying ForgotPassword view");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var token = await _accountService.GenerateForgotPasswordTokenAsync(model);
                if (token != null)
                {
                    // Send email logic
                }
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }
            return View(model);
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            _logger.Info("Displaying ForgotPasswordConfirmation view");
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordViewModel { Token = token, Email = email };
            _logger.Info("Displaying ResetPassword view");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {

            _logger.Info("Attempting to reset password");

            if (!ModelState.IsValid)
            {
                _logger.Warn("ResetPassword model state is invalid");
                return View(model);
            }
            var result = await _accountService.ResetPasswordAsync(model);
            if (result.Succeeded)
            {
                _logger.Info("Password reset successfully");
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
                _logger.Error($"Error during password reset: {error.Description}");
            }
            return View(model);
        }

        public IActionResult ResetPasswordConfirmation()
        {
            _logger.Info("Displaying ResetPasswordConfirmation view");
            return View();

        }
    }
}
