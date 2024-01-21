using Personal.Blog.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Personal.Blog.Services
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterViewModel model);
        Task<SignInResult> LoginUserAsync(LoginViewModel model, string returnUrl);
        Task LogoutUserAsync();
        Task<IdentityResult> ResetPasswordAsync(ResetPasswordViewModel model);
        Task<string> GenerateForgotPasswordTokenAsync(ForgotPasswordViewModel model);
        
    }
}
