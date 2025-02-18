using Juan_Mvc_Project.Data;
using Microsoft.AspNetCore.Mvc;

namespace JuanApp.Controllers
{
	public class BlogController : Controller
    {
        private readonly JuanDbContext _juanAppDbContext;

        public BlogController(JuanDbContext juanAppDbContext)
        {
            _juanAppDbContext = juanAppDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Detail()
        {
            return View();
        }
    }
}
