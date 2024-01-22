namespace Personal.Blog.ViewModels
{
    public class UserWithRolesViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }


}
