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

        [HttpGet]
        public IActionResult Index()
        {
            _logger.Info("Accessed Home Index");
            return View();
        }

        [HttpGet]
        public IActionResult About()
        {
            _logger.Info("Accessed About Page");
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            _logger.Info("Accessed Contact Page");
            ViewData["Message"] = "Your contact page.";
            return View();
        }

        [HttpGet]
        [Route("Error")]
        public IActionResult Error()
        {
            _logger.Error("General error occurred");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [Route("Home/404")]
        public IActionResult Error404()
        {
            _logger.Warn("404 - Page Not Found");
            return View("NotFound");
        }

        [HttpGet]
        [Route("Home/403")]
        public IActionResult Error403()
        {
            _logger.Warn("403 - Forbidden");
            return View("Forbidden");
        }
    }
}
