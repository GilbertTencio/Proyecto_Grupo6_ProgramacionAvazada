using Microsoft.AspNetCore.Mvc;

namespace WebApplicationAPP.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}