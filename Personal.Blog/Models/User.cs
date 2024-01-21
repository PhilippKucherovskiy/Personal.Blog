

using Microsoft.AspNetCore.Identity;

namespace Personal.Blog.Models
{
    public class User : IdentityUser<int>
    {
        
        // Связи
        public ICollection<Article> Articles { get; set; }
        public ICollection<Comment> Comments { get; set; }
        
    }

}
