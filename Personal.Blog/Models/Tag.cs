using Personal.Blog.Models;

namespace Personal.Blog.Models
{
    public class Tag
    {
        public int TagId { get; set; }
        public string Name { get; set; }

        // Связь
        public ICollection<ArticleTag> ArticleTags { get; set; }
    }

}
