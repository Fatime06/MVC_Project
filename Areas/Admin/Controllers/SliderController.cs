using Juan_Mvc_Project.Data;
using Juan_Mvc_Project.Helpers;
using Juan_Mvc_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Juan_Mvc_Project.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]
	public class SliderController : Controller
	{
		private readonly JuanDbContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public SliderController(JuanDbContext context, IWebHostEnvironment webHostEnvironment)
		{
			_context = context;
			_webHostEnvironment = webHostEnvironment;
		}

		public IActionResult Index()
		{
			return View(_context.Sliders.ToList());
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Slider slider)
		{
			if (slider.Image == null)
			{
				ModelState.AddModelError("Photo", "Photo is required");
			}

			if (!ModelState.IsValid)
				return View(slider);

			var file = slider.Photo;
			slider.Image = file.SaveImage(_webHostEnvironment.WebRootPath, "assets/img/slider");
			slider.CreateDate = DateTime.UtcNow;

			await _context.Sliders.AddAsync(slider);
			await _context.SaveChangesAsync();

			return RedirectToAction("Index");
		}

		public IActionResult Edit(int? id)
		{
			if (id is null)
				return NotFound();
			var slider = _context.Sliders.Find(id);
			if (slider is null)
				return NotFound();
			return View(slider);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(int id, Slider slider)
		{
			if (!ModelState.IsValid)
				return View(slider);

			var existingSlider = _context.Sliders.Find(id);
			if (existingSlider == null) return NotFound();

			string oldImage = existingSlider.Image;

			if (slider.Image != null)
			{
				if (!string.IsNullOrEmpty(oldImage))
				{
					var deletedImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img/slider", oldImage);
					FileManager.DeleteFile(deletedImagePath);
				}

				existingSlider.Image = slider.Photo.SaveImage(_webHostEnvironment.WebRootPath, "assets/img/slider");
			}

			existingSlider.Title = slider.Title;
			existingSlider.Description = slider.Description;
			existingSlider.ButtonText = slider.ButtonText;

			_context.Sliders.Update(existingSlider);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Delete(int id)
		{
			var slider = _context.Sliders.Find(id);
			if (slider == null)
				return NotFound();

			if (!string.IsNullOrEmpty(slider.Image))
			{
				string deletedImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img/slider", slider.Image);
				FileManager.DeleteFile(deletedImagePath);
			}

			_context.Sliders.Remove(slider);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}
	}
}
