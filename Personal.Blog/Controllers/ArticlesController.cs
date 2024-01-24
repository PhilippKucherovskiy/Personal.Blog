using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Personal.Blog.Models;
using Personal.Blog.Services;
using System.Threading.Tasks;
using NLog;
using ILogger = NLog.ILogger;

namespace Personal.Blog.Controllers
{
    public class ArticlesController : Controller
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IArticleService _articleService;

        public ArticlesController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        public async Task<IActionResult> Index()
        {
            _logger.Info("Fetching all articles");
            var articles = await _articleService.GetAllArticlesAsync();
            return View(articles);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.Warn("Article Details: Id is null");
                return NotFound();
            }

            _logger.Info($"Fetching details for article with Id: {id}");
            var article = await _articleService.GetArticleByIdAsync(id.Value);
            if (article == null)
            {
                _logger.Warn($"Article with Id: {id} not found");
                return NotFound();
            }

            return View(article);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArticleId,Title,Content,CreatedAt,UserId")] Article article)
        {
            if (ModelState.IsValid)
            {
                _logger.Info("Attempting to create a new article");
                await _articleService.CreateArticleAsync(article);
                _logger.Info("Article created successfully");
                return RedirectToAction(nameof(Index));
            }

            _logger.Warn("Invalid model state for creating article");
            return View(article);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.Warn("Edit Article: Id is null");
                return NotFound();
            }

            var article = await _articleService.GetArticleByIdAsync(id.Value);
            if (article == null)
            {
                _logger.Warn($"Edit Article: Article with Id: {id} not found");
                return NotFound();
            }
            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArticleId,Title,Content,CreatedAt,UserId")] Article article)
        {
            if (id != article.ArticleId)
            {
                _logger.Warn($"Edit Article: Article Id mismatch, expected: {article.ArticleId}, got: {id}");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _logger.Info($"Attempting to update article with Id: {id}");
                    await _articleService.UpdateArticleAsync(article);
                    _logger.Info($"Article with Id: {id} updated successfully");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ArticleExists(article.ArticleId))
                    {
                        _logger.Warn($"Update Article: Article with Id: {id} not found");
                        return NotFound();
                    }
                    else
                    {
                        _logger.Error($"Concurrency exception while updating article with Id: {id}");
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            _logger.Warn("Invalid model state for editing article");
            return View(article);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.Warn("Delete Article: Id is null");
                return NotFound();
            }

            var article = await _articleService.GetArticleByIdAsync(id.Value);
            if (article == null)
            {
                _logger.Warn($"Delete Article: Article with Id: {id} not found");
                return NotFound();
            }

            return View(article);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.Info($"Attempting to delete article with Id: {id}");
            await _articleService.DeleteArticleAsync(id);
            _logger.Info($"Article with Id: {id} deleted successfully");
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ArticleExists(int id)
        {
            return await _articleService.GetArticleByIdAsync(id) != null;
        }

        public async Task<IActionResult> ArticlesByUser(int userId)
        {
            _logger.Info($"Fetching articles by user with Id: {userId}");
            var articles = await _articleService.GetArticlesByUserIdAsync(userId);
            return View("Index", articles);
        }
    }
}
