using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext context { get; set; }

        public IActionResult Index()
        {
            //return Content(context.TodoItemsDb.Count().ToString());
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}