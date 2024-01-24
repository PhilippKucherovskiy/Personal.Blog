using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Personal.Blog.Models;
using Personal.Blog.Services;
using System.Threading.Tasks;
using NLog;
using Personal.Blog;
using ILogger = NLog.ILogger;

public class TagsController : Controller
{
    private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
    private readonly ITagService _tagService;
    private readonly ApplicationDbContext _context;

    public TagsController(ITagService tagService, ApplicationDbContext context)
    {
        _tagService = tagService;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        _logger.Info("Accessed Tags Index");
        return View(await _tagService.GetAllTagsAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            _logger.Warn("Tag Details accessed with null ID");
            return NotFound();
        }

        var tag = await _tagService.GetTagByIdAsync(id.Value);
        if (tag == null)
        {
            _logger.Warn($"Tag Details not found for ID {id}");
            return NotFound();
        }

        _logger.Info($"Accessed Details for Tag ID {id}");
        return View(tag);
    }

    public IActionResult Create()
    {
        _logger.Info("Accessed Tag Creation Page");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TagId,Name")] Tag tag)
    {
        if (ModelState.IsValid)
        {
            await _tagService.CreateTagAsync(tag);
            _logger.Info($"Tag created: {tag.Name}");
            return RedirectToAction(nameof(Index));
        }

        _logger.Warn("Tag creation failed due to invalid model state");
        return View(tag);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            _logger.Warn("Tag Edit accessed with null ID");
            return NotFound();
        }

        var tag = await _tagService.GetTagByIdAsync(id.Value);
        if (tag == null)
        {
            _logger.Warn($"Tag Edit not found for ID {id}");
            return NotFound();
        }

        _logger.Info($"Accessed Tag Edit for ID {id}");
        return View(tag);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("TagId,Name")] Tag tag)
    {
        if (id != tag.TagId)
        {
            _logger.Warn($"Mismatch in Tag ID during edit, expected {id}, got {tag.TagId}");
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            await _tagService.UpdateTagAsync(tag);
            _logger.Info($"Tag updated: {tag.Name}");
            return RedirectToAction(nameof(Index));
        }

        _logger.Warn("Tag update failed due to invalid model state");
        return View(tag);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            _logger.Warn("Tag Delete accessed with null ID");
            return NotFound();
        }

        var tag = await _tagService.GetTagByIdAsync(id.Value);
        if (tag == null)
        {
            _logger.Warn($"Tag Delete not found for ID {id}");
            return NotFound();
        }

        _logger.Info($"Accessed Tag Delete for ID {id}");
        return View(tag);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _tagService.DeleteTagAsync(id);
        _logger.Info($"Tag deleted: ID {id}");
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> ArticleTagsIndex()
    {
        _logger.Info("Accessed Article Tags Index");
        var applicationDbContext = _context.ArticleTags.Include(a => a.Article).Include(a => a.Tag);
        return View(await applicationDbContext.ToListAsync());
    }

    public IActionResult CreateArticleTag()
    {
        _logger.Info("Accessed Create Article Tag Page");
        ViewData["ArticleId"] = new SelectList(_context.Articles, "ArticleId", "Title");
        ViewData["TagId"] = new SelectList(_context.Tags, "TagId", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateArticleTag([Bind("ArticleId,TagId")] ArticleTag articleTag)
    {
        if (ModelState.IsValid)
        {
            _context.Add(articleTag);
            await _context.SaveChangesAsync();
            _logger.Info($"Article Tag created: Article ID {articleTag.ArticleId}, Tag ID {articleTag.TagId}");
            return RedirectToAction(nameof(ArticleTagsIndex));
        }

        _logger.Warn("Creation of Article Tag failed due to invalid model state");
        ViewData["ArticleId"] = new SelectList(_context.Articles, "ArticleId", "Title", articleTag.ArticleId);
        ViewData["TagId"] = new SelectList(_context.Tags, "TagId", "Name", articleTag.TagId);
        return View(articleTag);
    }
}
