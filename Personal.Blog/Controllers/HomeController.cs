using Microsoft.AspNetCore.Mvc;
using Personal.Blog.Models;
using System.Diagnostics;

namespace Personal.Blog.Controllers
{
    public class HomeController : Controller
    {
        // GET: /Home/Index
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Home/About
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        // GET: /Home/Contact
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        // GET: /Home/Error
        [Route("Error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Обработка ошибки 404 - Не найдено
        [Route("Home/404")]
        public IActionResult Error404()
        {
            return View("NotFound");
        }

        // Обработка ошибки 403 - Доступ запрещен
        [Route("Home/403")]
        public IActionResult Error403()
        {
            return View("Forbidden");
        }
    }
}
