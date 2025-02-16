using Juan_Mvc_Project.Data;
using Microsoft.AspNetCore.Mvc;

namespace Juan_Mvc_Project.Controllers
{
    public class BlogController : Controller
    {
        private readonly JuanDbContext _context;

        public BlogController(JuanDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Detail(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }
            var data = _context.Blogs.FirstOrDefault(b => b.Id == id);
            if(data is null)
            {
                return NotFound();
            }
            return View(data);
        }
    }
}
