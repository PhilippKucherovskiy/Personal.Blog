using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Personal.Blog;
using Personal.Blog.Models;
using Personal.Blog.Services;
using System.Threading.Tasks;

public class TagsController : Controller
{
    private readonly ITagService _tagService;
    private readonly ApplicationDbContext _context;

    public TagsController(ITagService tagService, ApplicationDbContext context)
    {
        _tagService = tagService;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _tagService.GetAllTagsAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var tag = await _tagService.GetTagByIdAsync(id.Value);
        if (tag == null)
        {
            return NotFound();
        }
        return View(tag);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TagId,Name")] Tag tag)
    {
        if (ModelState.IsValid)
        {
            await _tagService.CreateTagAsync(tag);
            return RedirectToAction(nameof(Index));
        }
        return View(tag);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var tag = await _tagService.GetTagByIdAsync(id.Value);
        if (tag == null)
        {
            return NotFound();
        }
        return View(tag);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("TagId,Name")] Tag tag)
    {
        if (id != tag.TagId)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            await _tagService.UpdateTagAsync(tag);
            return RedirectToAction(nameof(Index));
        }
        return View(tag);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var tag = await _tagService.GetTagByIdAsync(id.Value);
        if (tag == null)
        {
            return NotFound();
        }
        return View(tag);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _tagService.DeleteTagAsync(id);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> ArticleTagsIndex()
    {
        var applicationDbContext = _context.ArticleTags.Include(a => a.Article).Include(a => a.Tag);
        return View(await applicationDbContext.ToListAsync());
    }

    public IActionResult CreateArticleTag()
    {
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
            return RedirectToAction(nameof(ArticleTagsIndex));
        }
        ViewData["ArticleId"] = new SelectList(_context.Articles, "ArticleId", "Title", articleTag.ArticleId);
        ViewData["TagId"] = new SelectList(_context.Tags, "TagId", "Name", articleTag.TagId);
        return View(articleTag);
    }
}
