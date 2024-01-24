using Microsoft.AspNetCore.Mvc;
using Personal.Blog.Models;
using System.Diagnostics;
using NLog;
using ILogger = NLog.ILogger;

namespace Personal.Blog.Controllers
{
    public class HomeController : Controller
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        // GET: /Home/Index
        public IActionResult Index()
        {
            _logger.Info("Accessed Home Index");
            return View();
        }

        // GET: /Home/About
        public IActionResult About()
        {
            _logger.Info("Accessed About Page");
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        // GET: /Home/Contact
        public IActionResult Contact()
        {
            _logger.Info("Accessed Contact Page");
            ViewData["Message"] = "Your contact page.";
            return View();
        }

        // GET: /Home/Error
        [Route("Error")]
        public IActionResult Error()
        {
            _logger.Error("General error occurred");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Обработка ошибки 404 - Не найдено
        [Route("Home/404")]
        public IActionResult Error404()
        {
            _logger.Warn("404 - Page Not Found");
            return View("NotFound");
        }

        // Обработка ошибки 403 - Доступ запрещен
        [Route("Home/403")]
        public IActionResult Error403()
        {
            _logger.Warn("403 - Forbidden");
            return View("Forbidden");
        }
    }
}
