using Juan_Mvc_Project.Data;
using Juan_Mvc_Project.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JuanApp.Controllers
{
	public class HomeController : Controller
    {
        private readonly JuanDbContext _juanAppDbContext;

        public HomeController(JuanDbContext juanAppDbContext)
        {
            _juanAppDbContext = juanAppDbContext;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new();
            homeVM.Services = _juanAppDbContext.Services.ToList();
            homeVM.Sliders = _juanAppDbContext.Sliders.ToList();
            homeVM.Products = _juanAppDbContext.Products
               .Include(b => b.Category)
               .Include(b => b.ProductSizes)
               .Include(b => b.ProductImages.Where(x => x.Status != null))
               .ToList();
            homeVM.NewProducts = _juanAppDbContext.Products
               .Include(b => b.Category)
               .Include(b => b.ProductSizes)
               .Include(b => b.ProductImages.Where(x => x.Status != null))
               .OrderByDescending(p => p.Id)
               .Take(4)
               .ToList();
            return View(homeVM);
        }

    }
}
