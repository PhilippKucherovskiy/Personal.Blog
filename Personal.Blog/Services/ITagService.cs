

using Personal.Blog.Models;
using System.Collections.Generic;
using System.Threading.Tasks;



namespace Personal.Blog.Services
{
    public interface ITagService
    {
        Task<IEnumerable<Tag>> GetAllTagsAsync();
        Task<Tag> GetTagByIdAsync(int tagId);
        Task CreateTagAsync(Tag tag);
        Task UpdateTagAsync(Tag tag);
        Task DeleteTagAsync(int tagId);
        Task<bool> TagExistsAsync(int tagId);

        Task<IEnumerable<ArticleTag>> GetAllArticleTagsAsync();
        Task CreateArticleTagAsync(ArticleTag articleTag);
        Task DeleteArticleTagAsync(int articleId, int tagId);
        Task<bool> ArticleTagExistsAsync(int articleId, int tagId);
    }

}
