
using Personal.Blog.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Personal.Blog.Services
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetAllCommentsAsync();
        Task<Comment> GetCommentByIdAsync(int commentId);
        Task CreateCommentAsync(Comment comment);
        Task UpdateCommentAsync(Comment comment);
        Task DeleteCommentAsync(int commentId);
        Task<bool> CommentExistsAsync(int commentId);
    }

}
