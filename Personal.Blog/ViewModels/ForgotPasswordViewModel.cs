using System.ComponentModel.DataAnnotations;

namespace Personal.Blog.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")] 
        public string Email { get; set; }

    }
}
