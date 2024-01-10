namespace Personal.Blog.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        // Связи
        public ICollection<Article> Articles { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }

}
