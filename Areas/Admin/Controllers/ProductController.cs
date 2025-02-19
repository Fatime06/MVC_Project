using Juan_Mvc_Project.Data;
using Juan_Mvc_Project.Helpers;
using Juan_Mvc_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Juan_Mvc_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly JuanDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(JuanDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index(int page = 1, int take = 4)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductSizes).ThenInclude(ps => ps.Size)
                .Include(p => p.ProductReviews)
                .AsQueryable();

            return View(PaginatedList<Product>.Create(query, take, page));
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Sizes = _context.Sizes.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Sizes = _context.Sizes.ToList();

            if (!ModelState.IsValid)
                return View(product);

            if (!_context.Categories.Any(c => c.Id == product.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Selected category does not exist.");
                return View(product);
            }

            foreach (var sizeId in product.SizeIds)
            {
                if (!_context.Sizes.Any(s => s.Id == sizeId))
                {
                    ModelState.AddModelError("SizeIds", "One or more selected sizes are invalid.");
                    return View(product);
                }
                ProductSize productSize = new();
                productSize.SizeId = sizeId;
                productSize.ProductId = product.Id;
                product.ProductSizes.Add(productSize);
            }

            var files = product.Photos;
            if (files.Count > 0)
            {
                foreach (var file in files)
                {
                    ProductImage productImage = new();
                    productImage.Name = file.SaveImage(_env.WebRootPath, "assets/img/product");
                    if (files[0] == file)
                    {
                        productImage.Status = true;
                    }
                    product.ProductImages.Add(productImage);
                }
            }

            _context.Products.Add(product);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Detail(int? id)
        {
            if (id is null)
                return NotFound();
            var product = _context.Products
                .Include(b => b.Category)
                .Include(b => b.ProductSizes)
                .ThenInclude(bt => bt.Size)
                .Include(b => b.ProductImages)
                .FirstOrDefault(b => b.Id == id);
            if (product is null)
                return NotFound();
            return View(product);
        }

        public IActionResult DeleteBookImage(int? id)
        {
            if (id is null)
                return NotFound();
            var productImage = _context.ProductImages.Find(id);
            if (productImage is null)
                return NotFound();
            if (productImage.Status == true)
            {
                return BadRequest();
            }

            _context.ProductImages.Remove(productImage);
            _context.SaveChanges();
            return RedirectToAction("Detail", new { id = productImage.ProductId });
        }

        public IActionResult SetMainImage(int? id)
        {
            if (id is null)
                return NotFound();
            var productImage = _context.ProductImages.Find(id);
            if (productImage is null)
                return NotFound();

            var mainImage = _context.ProductImages.FirstOrDefault(bi => bi.Status == true && bi.ProductId == productImage.ProductId);
            mainImage.Status = false;

            productImage.Status = true;

            _context.SaveChanges();
            return RedirectToAction("Edit", new { id = productImage.ProductId });

        }

        public IActionResult Edit(int? id)
        {
            if (id is null)
                return NotFound();
            var book = _context.Products
                .Include(b => b.Category)
                .Include(b => b.ProductSizes)
                .ThenInclude(bt => bt.Size)
                .Include(b => b.ProductImages)
                .FirstOrDefault(b => b.Id == id);
            if (book is null)
                return NotFound();
            book.SizeIds = book.ProductSizes.Select(b => b.Id).ToList();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Sizes = _context.Sizes.ToList();
            return View(book);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Sizes = _context.Sizes.ToList();

            if (!ModelState.IsValid)
                return View(product);

            var existProduct = _context.Products.Include(b => b.ProductSizes).Include(b => b.ProductImages).FirstOrDefault(b => b.Id == product.Id);
            if (existProduct == null)
                return NotFound();

            if (!_context.Categories.Any(a => a.Id == product.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Category not found");
                return View(product);
            }

            existProduct.Name = product.Name;
            existProduct.Description = product.Description;
            existProduct.SalePrice = product.SalePrice;
            existProduct.CostPrice = product.CostPrice;
            existProduct.InStock = product.InStock;
            existProduct.Rate = product.Rate;
            existProduct.CategoryId = product.CategoryId;

            existProduct.ProductSizes.Clear();
            foreach (var sizeId in product.SizeIds)
            {
                if (!_context.Sizes.Any(t => t.Id == sizeId))
                {
                    ModelState.AddModelError("SizeIds", "SizeId not found");
                    return View(product);
                }
                existProduct.ProductSizes.Add(new ProductSize { SizeId = sizeId, ProductId = existProduct.Id });
            }

            if (product.Photos != null && product.Photos.Count > 0)
            {
                foreach (var file in product.Photos)
                {
                    ProductImage productImage = new()
                    {
                        Name = file.SaveImage(_env.WebRootPath, "assets/img/product"),
                        Status = (existProduct.ProductImages.Count == 0)
                    };
                    existProduct.ProductImages.Add(productImage);
                }
            }

            _context.Products.Update(existProduct);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
