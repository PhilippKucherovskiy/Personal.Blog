using System.ComponentModel.DataAnnotations;

namespace Personal.Blog.ViewModels
{
    public class UserWithRolesViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }


}
