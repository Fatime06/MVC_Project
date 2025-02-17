using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Juan_Mvc_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class DashboardController : Controller
    {
        public IActionResult CreateAdmin()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
