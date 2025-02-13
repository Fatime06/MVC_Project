using Juan_Mvc_Project.Data;
using Juan_Mvc_Project.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Juan_Mvc_Project.Controllers
{
    public class HomeController : Controller
    {

        private readonly JuanDbContext _context;

        public HomeController(JuanDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            HomeVm homeVm = new HomeVm();
            homeVm.sliders= _context.Sliders.ToList();
            return View(sliders);
        }
    }
}
