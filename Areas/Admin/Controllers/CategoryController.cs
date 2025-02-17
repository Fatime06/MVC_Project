using Humanizer;
using Juan_Mvc_Project.Data;
using Juan_Mvc_Project.Helpers;
using Juan_Mvc_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Juan_Mvc_Project.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class CategoryController : Controller
	{
		private readonly JuanDbContext _context;

		public CategoryController(JuanDbContext context)
		{
			_context = context;
		}

		public IActionResult Index(int page = 1,int take = 2)
		{
			var query = _context.Categories.Include(c=>c.Products).AsQueryable();
			PaginatedList<Category> paginatedList = PaginatedList<Category>.Create(query, take, page);
			return View(paginatedList);
		}
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
        public IActionResult Create(Category category)
        {
			if (!ModelState.IsValid) return View();
			if(_context.Categories.Any(c=>c.Name.ToLower() == category.Name.Trim().ToLower()))
			{
				ModelState.AddModelError("Name", "This category already exists");
				return View();
			}
			category.Name = category.Name.Trim();
			_context.Categories.Add(category);
			_context.SaveChanges();
            return RedirectToAction("Index");
        }
		public IActionResult Edit(int? id)
		{
            if (id is null)
            {
				return BadRequest();
            }
            var category = _context.Categories.FirstOrDefault(c=>c.Id == id);
			if(category is null)
			{
				return NotFound();
			}
			return View(category);
		}
		[HttpPost]
        public IActionResult Edit(Category category)
        {
            if (!ModelState.IsValid) return View();
            if (_context.Categories.Any(c => c.Name.ToLower() == category.Name.Trim().ToLower() && c.Id != category.Id))
            {
                ModelState.AddModelError("Name", "This category already exist");
                return View();
            }
            var existCategory = _context.Categories.FirstOrDefault(c => c.Id == category.Id);
            if (category is null)
            {
                return NotFound();
            }
			existCategory.Name = category.Name.Trim();
			_context.SaveChanges();
            return RedirectToAction("Index");
        }
		public IActionResult Detail(int? id)
		{
            if (id is null) return BadRequest();
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category is null) return NotFound();
            return View(category);
        }
        public IActionResult Delete(int? id)
        {
            if (id is null) return BadRequest();
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category is null) return NotFound();
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
