using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personal.Blog.Models;
using Personal.Blog.Services;
using System.Threading.Tasks;
using NLog;
using ILogger = NLog.ILogger;

public class CommentsController : Controller
{
    private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    public async Task<IActionResult> Index()
    {
        _logger.Info("Fetching all comments");
        var comments = await _commentService.GetAllCommentsAsync();
        return View(comments);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            _logger.Warn("Comment Details: Id is null");
            return NotFound();
        }

        _logger.Info($"Fetching details for comment with Id: {id}");
        var comment = await _commentService.GetCommentByIdAsync(id.Value);
        if (comment == null)
        {
            _logger.Warn($"Comment with Id: {id} not found");
            return NotFound();
        }

        return View(comment);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CommentId,Content,CreatedAt,UserId,ArticleId")] Comment comment)
    {
        if (ModelState.IsValid)
        {
            _logger.Info("Attempting to create a new comment");
            await _commentService.CreateCommentAsync(comment);
            _logger.Info("Comment created successfully");
            return RedirectToAction(nameof(Index));
        }

        _logger.Warn("Invalid model state for creating comment");
        return View(comment);
    }

    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            _logger.Warn("Edit Comment: Id is null");
            return NotFound();
        }

        var comment = await _commentService.GetCommentByIdAsync(id.Value);
        if (comment == null)
        {
            _logger.Warn($"Edit Comment: Comment with Id: {id} not found");
            return NotFound();
        }
        return View(comment);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("CommentId,Content,CreatedAt,UserId,ArticleId")] Comment comment)
    {
        if (id != comment.CommentId)
        {
            _logger.Warn($"Edit Comment: Comment Id mismatch, expected: {comment.CommentId}, got: {id}");
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _logger.Info($"Attempting to update comment with Id: {id}");
            await _commentService.UpdateCommentAsync(comment);
            _logger.Info($"Comment with Id: {id} updated successfully");
            return RedirectToAction(nameof(Index));
        }

        _logger.Warn("Invalid model state for editing comment");
        return View(comment);
    }

    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            _logger.Warn("Delete Comment: Id is null");
            return NotFound();
        }

        var comment = await _commentService.GetCommentByIdAsync(id.Value);
        if (comment == null)
        {
            _logger.Warn($"Delete Comment: Comment with Id: {id} not found");
            return NotFound();
        }

        return View(comment);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        _logger.Info($"Attempting to delete comment with Id: {id}");
        await _commentService.DeleteCommentAsync(id);
        _logger.Info($"Comment with Id: {id} deleted successfully");
        return RedirectToAction(nameof(Index));
    }
}
