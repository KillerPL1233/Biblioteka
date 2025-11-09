using Microsoft.AspNetCore.Mvc;

namespace Biblioteka.Controllers.Home
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
