using Microsoft.AspNetCore.Mvc;

namespace Juan_Mvc_Project.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
