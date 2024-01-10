using Personal.Blog.Models;

namespace Personal.Blog.Services
{
    public interface IArticleService
    {
        Task CreateArticleAsync(Article article);
        Task<List<Article>> GetAllArticlesAsync();
        Task<Article> GetArticleByIdAsync(int id);
        Task UpdateArticleAsync(Article article);
        Task DeleteArticleAsync(int id);
    }

}
