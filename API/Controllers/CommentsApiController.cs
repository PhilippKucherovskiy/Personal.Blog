using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Personal.Blog.Models;
using Personal.Blog.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsApiController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsApiController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        // GET: api/CommentsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            var comments = await _commentService.GetAllCommentsAsync();
            return Ok(comments);
        }

        // GET: api/CommentsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            var comment = await _commentService.GetCommentByIdAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return comment;
        }

        // PUT: api/CommentsApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, Comment comment)
        {
            if (id != comment.CommentId)
            {
                return BadRequest();
            }

            try
            {
                await _commentService.UpdateCommentAsync(comment);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _commentService.CommentExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CommentsApi
        [HttpPost]
        public async Task<ActionResult<Comment>> PostComment(Comment comment)
        {
            await _commentService.CreateCommentAsync(comment);
            return CreatedAtAction("GetComment", new { id = comment.CommentId }, comment);
        }

        // DELETE: api/CommentsApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            if (!await _commentService.CommentExistsAsync(id))
            {
                return NotFound();
            }

            await _commentService.DeleteCommentAsync(id);
            return NoContent();
        }
    }
}
