namespace Personal.Blog.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        public string Name { get; set; }

        // Связь
        public ICollection<UserRole> UserRoles { get; set; }
    }

}
