

using Microsoft.EntityFrameworkCore;
using Personal.Blog.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Personal.Blog.Services
{
 
    public class TagService : ITagService
    {
        private readonly ApplicationDbContext _context;

        public TagService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<Tag> GetTagByIdAsync(int tagId)
        {
            return await _context.Tags.FindAsync(tagId);
        }

        public async Task CreateTagAsync(Tag tag)
        {
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTagAsync(Tag tag)
        {
            _context.Entry(tag).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTagAsync(int tagId)
        {
            var tag = await _context.Tags.FindAsync(tagId);
            if (tag != null)
            {
                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> TagExistsAsync(int tagId)
        {
            return await _context.Tags.AnyAsync(e => e.TagId == tagId);
        }

        public async Task<IEnumerable<ArticleTag>> GetAllArticleTagsAsync()
        {
            return await _context.ArticleTags.Include(a => a.Article).Include(a => a.Tag).ToListAsync();
        }

        public async Task CreateArticleTagAsync(ArticleTag articleTag)
        {
            _context.ArticleTags.Add(articleTag);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteArticleTagAsync(int articleId, int tagId)
        {
            var articleTag = await _context.ArticleTags
                .FirstOrDefaultAsync(at => at.ArticleId == articleId && at.TagId == tagId);
            if (articleTag != null)
            {
                _context.ArticleTags.Remove(articleTag);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ArticleTagExistsAsync(int articleId, int tagId)
        {
            return await _context.ArticleTags
                .AnyAsync(at => at.ArticleId == articleId && at.TagId == tagId);
        }
    }

}
