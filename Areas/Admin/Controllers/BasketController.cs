using Juan_Mvc_Project.Data;
using Juan_Mvc_Project.Models;
using JuanApp.Models;
using JuanApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace JuanApp.Controllers
{
	public class BasketController : Controller
    {
        private readonly JuanDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public BasketController(JuanDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Add(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _context.Products
                .Include(b => b.ProductSizes)
                .Include(b => b.ProductImages)
                .FirstOrDefault(b => b.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var basket = HttpContext.Request.Cookies["basket"];
            List<BasketItemVM> basketItemVms;

            if (basket == null)
            {
                basketItemVms = new List<BasketItemVM>();
            }
            else
            {
                basketItemVms = JsonSerializer.Deserialize<List<BasketItemVM>>(basket);
            }

            var basketItemVm = basketItemVms.FirstOrDefault(b => b.Id == id);

            if (basketItemVm == null)
            {
                BasketItemVM basketItem = new();
                basketItem.Id = id;
                basketItem.Name = product.Name;
                basketItem.Price = product.SalePrice;
                basketItem.Count = 1;
                basketItem.MainImage = product.ProductImages.FirstOrDefault(bi => bi.Status == true).Name;
                basketItemVms.Add(basketItem);
            }
            else
            {
                basketItemVm.Count++;
            }
            if(User.Identity.IsAuthenticated&& User.IsInRole("member"))
            {
                var user = _userManager.Users.Include(u=>u.BasketItems).FirstOrDefault(u=>u.UserName==User.Identity.Name); 
                var basketItem = user.BasketItems.FirstOrDefault(b=>b.ProductId==id);
                if (basketItem == null)
                {
                    BasketItem newBasketItem = new();
                    newBasketItem.ProductId = product.Id;
                    newBasketItem.Count = 1;
                    newBasketItem.AppUserId = user.Id;
                    newBasketItem.CreateDate = DateTime.Now;
                    user.BasketItems.Add(newBasketItem);
                }
                else
                {
                    basketItem.Count++;
                }
                _context.SaveChanges();
            }

            HttpContext.Response.Cookies.Append("basket", JsonSerializer.Serialize(basketItemVms));

            return PartialView("_BasketPartial", basketItemVms);
        }


        public IActionResult Index()
        {
            var basket = HttpContext.Request.Cookies["basket"];
            List<BasketItemVM> basketItemVms;

            if (basket == null)
            {
                basketItemVms = new List<BasketItemVM>();
            }
            else
            {
                basketItemVms = JsonSerializer.Deserialize<List<BasketItemVM>>(basket);
            }

            return View(basketItemVms);
        }

        [Authorize(Roles ="member")]
        public IActionResult Checkout()
        {
            var user = _userManager.Users
                .Include(u=>u.BasketItems)
                .ThenInclude(bi=>bi.Product).FirstOrDefault(u=>u.UserName == User.Identity.Name);
            CheckoutVM checkoutVM = new();
            checkoutVM.CheckoutItemVMs = user.BasketItems.Select(b=>new CheckoutItemVM
            { 
              Name =b.Product.Name,
              TotalItemPrice = b.Product.SalePrice*b.Count,
              Count = b.Count
            }).ToList();

            return View(checkoutVM);
        }

        [HttpPost]
        [Authorize(Roles = "member")]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout(OrderVM orderVM )
        {
            var user = _userManager.Users
                    .Include(u => u.BasketItems)
                    .ThenInclude(bi => bi.Product)
                    .FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (user == null)
                return RedirectToAction("login", "account");
            if (!ModelState.IsValid)
            {
                CheckoutVM checkoutVM = new();
                checkoutVM.CheckoutItemVMs = user.BasketItems.Select(b => new CheckoutItemVM
                {
                    Name = b.Product.Name,
                    TotalItemPrice = b.Product.SalePrice * b.Count,
                    Count = b.Count
                }).ToList();
                checkoutVM.OrderVM = orderVM;
            }
            Order order = new();
            order.AppUserId = user.Id;
            order.Country = orderVM.Country;
            order.ZipCode = orderVM.ZipCode;
            order.TownCity = orderVM.TownCity;
            order.Address = orderVM.Address;
            order.TotalPrice = user.BasketItems.Sum(b=>b.Product.SalePrice * b.Count);
            order.OrderStatus = OrderStatus.Pending;

            order.OrderItems = user.BasketItems.Select(b => new OrderItem
            {
                ProductId = b.ProductId,
                Count = b.Count
            }).ToList();

            _context.Orders.Add(order);
            _context.BasketItems.RemoveRange(user.BasketItems);
            _context.SaveChanges();
            HttpContext.Response.Cookies.Delete("basket");
            return RedirectToAction("Profile", "Account", new { tab = "order" });
        }

        [Authorize(Roles = "member")]
        public IActionResult GetOrderItems(int orderId)
        {
            var order = _context.Orders
                .Where(o => o.AppUserId==_userManager.GetUserId(User))
                .Include(o=>o.OrderItems)
                .ThenInclude(o => o.Product)
                .FirstOrDefault(o => o.Id == orderId);
            return PartialView("_OrderItemsPartial",order);
        }
    }
}
