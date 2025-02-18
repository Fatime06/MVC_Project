using Juan_Mvc_Project.Data;
using Juan_Mvc_Project.Models;
using Juan_Mvc_Project.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JuanApp.Controllers
{
	public class ProductController : Controller
    {
        private readonly JuanDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ProductController(JuanDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Detail(int? id)
        {
            if (id is null)
                return NotFound();
            var user = _userManager.GetUserAsync(User).Result;
            if (user == null || !_userManager.IsInRoleAsync(user, "member").Result)
            {
                return RedirectToAction("login", "account", new { returnUrl = Url.Action("detail", "product", id = (int)id) });

            }
            var vm = getProductDetailVM((int)id, user.Id);

            if (vm.Product == null)
                return NotFound();

            return View(vm);

        }

        [HttpPost]
        public async Task<IActionResult> AddReview(ProductReview productReview)
        {
            if (!_context.Products.Any(b => b.Id == productReview.ProductId))
            {
                return RedirectToAction("NotFound", "Error");
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null || !await _userManager.IsInRoleAsync(user, "member"))
               return RedirectToAction("login", "account", new { returnUrl = Url.Action("detail", "product", productReview.ProductId) });
            if (_context.ProductReviews.Any(x => x.ProductId == productReview.ProductId && x.AppUserId == user.Id))
                return NotFound();

            if (!ModelState.IsValid) return View("detail", getProductDetailVM(productReview.ProductId, user.Id));

            productReview.AppUserId = user.Id;
            productReview.CreateDate = DateTime.Now;
            _context.ProductReviews.Add(productReview);
            await _context.SaveChangesAsync();

            return RedirectToAction("Detail", new { id = productReview.ProductId });
        }
        private ProductDetailVM getProductDetailVM(int productId, string userId)
        {
            var existProduct = _context.Products
                .Include(b => b.Category)
                .Include(b => b.ProductSizes).ThenInclude(bc => bc.Size)
                .Include(b => b.ProductImages)
                .Include(b => b.ProductReviews)
                .ThenInclude(bc => bc.AppUser)
                .FirstOrDefault(x => x.Id == productId);

            ProductDetailVM productDetailVm = new ProductDetailVM
            {
                Product = existProduct,
                RelatedProducts = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductSizes).ThenInclude(ps => ps.Size)
                .Include(p => p.ProductImages)
                .Where(p => p.CategoryId == existProduct.CategoryId && p.Id != existProduct.Id) 
                .Take(5) 
                .ToList(),
                HasCommentUser = _context.ProductReviews.Any(x=> x.Id == productId && x.AppUserId == userId && x.Status != ReviewStatus.Rejected),
            };
            productDetailVm.TotalReviews = _context.ProductReviews.Count(x => x.ProductId == existProduct.Id);
            productDetailVm.AvgRate = productDetailVm.TotalReviews > 0 ? (int)_context.ProductReviews.Where(x => x.ProductId == existProduct.Id).Average(x => x.Rate) : 0;
            return productDetailVm;
        }
    }
}
