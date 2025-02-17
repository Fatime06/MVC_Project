using Juan_Mvc_Project.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Juan_Mvc_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> CreateAdminUser()
        {
            AppUser user = new AppUser
            {
                UserName = "_admin",
                Email = "admin@gmail.com",
                FullName = "Fatima Admin"
            };

            IdentityResult result = await _userManager.CreateAsync(user, "_Admin123");
            await _userManager.AddToRoleAsync(user, "admin");

            return Json(result);
        }
        public async Task<IActionResult> CreateRole()
        {
            await _roleManager.CreateAsync(new IdentityRole("admin"));
            await _roleManager.CreateAsync(new IdentityRole("user"));
            await _roleManager.CreateAsync(new IdentityRole("member"));
            await _roleManager.CreateAsync(new IdentityRole("superAdmin"));
            return Content("addedd");

        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AdminLoginVM adminLoginVM)
        {
            if (!ModelState.IsValid)
                return View(adminLoginVM);

            var admin = _userManager.FindByNameAsync(adminLoginVM.UserName).Result;
            if (admin == null || !_userManager.CheckPasswordAsync(admin, adminLoginVM.Password).Result)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View(adminLoginVM);
            }

            await _signInManager.SignInAsync(admin, true);

            return View();
        }

        public async Task<IActionResult> GetUser()
        {
            var user = await _userManager.GetUserAsync(User);
            return Json(User.Identity);
        }
    }
}
