using Juan_Mvc_Project.Data;
using Juan_Mvc_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Juan_Mvc_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class SizeController : Controller
    {
        private readonly JuanDbContext _context;

        public SizeController(JuanDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var sizes = await _context.Sizes.ToListAsync();
            return View(sizes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Size size)
        {
            if (!ModelState.IsValid)
                return View(size);

            if (await _context.Sizes.AnyAsync(c => c.Name == size.Name))
            {
                ModelState.AddModelError("Name", "size with this name already exists.");
                return View(size);
            }

            size.CreateDate = DateTime.UtcNow;
            await _context.Sizes.AddAsync(size);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
                return NotFound();

            var size = await _context.Sizes.FindAsync(id);
            if (size == null)
                return NotFound();

            return View(size);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var size = await _context.Sizes.FindAsync(id);
            if (size == null)
                return NotFound();

            return View(size);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Size size)
        {
            if (!ModelState.IsValid)
                return View(size);

            var existingsize = await _context.Sizes.FindAsync(id);
            if (existingsize == null)
                return NotFound();

            if (await _context.Sizes.AnyAsync(c => c.Name == size.Name && c.Id != id))
            {
                ModelState.AddModelError("Name", "Another size with this name already exists.");
                return View(size);
            }

            existingsize.Name = size.Name;
            _context.Sizes.Update(existingsize);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var existingsize = await _context.Sizes.FindAsync(id);
            if (existingsize == null)
                return NotFound();

            _context.Sizes.Remove(existingsize);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
