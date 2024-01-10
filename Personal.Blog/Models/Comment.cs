namespace Personal.Blog.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public int ArticleId { get; set; }

        // Связи
        public User User { get; set; }
        public Article Article { get; set; }
    }

}
