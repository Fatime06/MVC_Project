using Juan_Mvc_Project.Data;
using Juan_Mvc_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Juan_Mvc_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class SettingController : Controller
    {
        private readonly JuanDbContext _context;

        public SettingController(JuanDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var settings = _context.Settings.ToList();
            return View(settings);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Setting setting)
        {
            if (!ModelState.IsValid)
                return View(setting);

            _context.Settings.Add(setting);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Detail(string key)
        {
            if (string.IsNullOrEmpty(key))
                return NotFound();

            var setting = await _context.Settings.FindAsync(key);
            if (setting == null)
                return NotFound();

            return View(setting);
        }

        public async Task<IActionResult> Edit(string key)
        {
            if (string.IsNullOrEmpty(key))
                return NotFound();

            var setting = await _context.Settings.FindAsync(key);
            if (setting == null)
                return NotFound();

            return View(setting);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string key, Setting setting)
        {
            if (key != setting.Key)
                return NotFound();

            if (!ModelState.IsValid)
                return View(setting);

            _context.Settings.Update(setting);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string key)
        {
            var setting = await _context.Settings.FindAsync(key);
            if (setting == null)
                return NotFound();

            _context.Settings.Remove(setting);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
