using Microsoft.AspNetCore.Mvc;
using Personal.Blog.Models;
using Personal.Blog.Services;
using System.Threading.Tasks;

public class CommentsController : Controller
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _commentService.GetAllCommentsAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var comment = await _commentService.GetCommentByIdAsync(id.Value);
        if (comment == null)
        {
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
            await _commentService.CreateCommentAsync(comment);
            return RedirectToAction(nameof(Index));
        }
        return View(comment);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var comment = await _commentService.GetCommentByIdAsync(id.Value);
        if (comment == null)
        {
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
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            await _commentService.UpdateCommentAsync(comment);
            return RedirectToAction(nameof(Index));
        }
        return View(comment);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var comment = await _commentService.GetCommentByIdAsync(id.Value);
        if (comment == null)
        {
            return NotFound();
        }

        return View(comment);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _commentService.DeleteCommentAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
