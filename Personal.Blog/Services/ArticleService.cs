using Microsoft.EntityFrameworkCore;
using Personal.Blog.Models;

namespace Personal.Blog.Services
{
    public class ArticleService
    {
        private readonly ApplicationDbContext _context;

        public ArticleService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Метод для создания новой статьи
        public async Task CreateArticleAsync(Article article)
        {
            if (article == null)
            {
                throw new ArgumentNullException(nameof(article));
            }

            _context.Articles.Add(article);
            await _context.SaveChangesAsync();
        }

        // Метод для получения всех статей
        public async Task<List<Article>> GetAllArticlesAsync()
        {
            return await _context.Articles.ToListAsync();
        }

        // Метод для получения статьи по ID
        public async Task<Article> GetArticleByIdAsync(int id)
        {
            return await _context.Articles.FindAsync(id);
        }

        // Метод для обновления статьи
        public async Task UpdateArticleAsync(Article article)
        {
            if (article == null)
            {
                throw new ArgumentNullException(nameof(article));
            }

            _context.Entry(article).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // Метод для удаления статьи
        public async Task DeleteArticleAsync(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article != null)
            {
                _context.Articles.Remove(article);
                await _context.SaveChangesAsync();
            }
        }
    }
}
