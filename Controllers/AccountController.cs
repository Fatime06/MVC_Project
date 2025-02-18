using Juan_Mvc_Project.Models;
using Juan_Mvc_Project.Services;
using Juan_Mvc_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JuanApp.Controllers
{
	public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly EmailService _emailService;


        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, EmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public IActionResult Register()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterVM userRegisterVm)
        {
            if (!ModelState.IsValid)
                return View();

            AppUser user = await _userManager.FindByNameAsync(userRegisterVm.UserName);
            if (user != null)
            {
                ModelState.AddModelError("UserName", "This username already exists");
                return View();
            }

            user = new AppUser
            {
                UserName = userRegisterVm.UserName,
                FullName = userRegisterVm.FullName,
                Email = userRegisterVm.Email
            };

            var result = await _userManager.CreateAsync(user, userRegisterVm.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }

            await _userManager.AddToRoleAsync(user, "member");
            //send email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var url = Url.Action("VerifyEmail", "Account", new { email = user.Email, token = token }, Request.Scheme);
            // create email

            using StreamReader reader = new StreamReader("wwwroot/templates/emailconfirmation.html");
            var body = reader.ReadToEnd();
            body = body.Replace("{{url}}", url);
            body = body.Replace("{{username}}", user.UserName);

            _emailService.SendEmail(user.Email, "Confirm Email", body);

            //TempData["Success"] = "Email sended to " + user.Email;

            return RedirectToAction("Login");
        }


        public async Task<IActionResult> VerifyEmail(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !_userManager.IsInRoleAsync(user, "member").Result) return RedirectToAction("notfound", "error");
            await _userManager.ConfirmEmailAsync(user, token);
            return RedirectToAction("login");
        }
        
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginVM userLoginVm)
        {
            if (!ModelState.IsValid)
                return View();

            AppUser user = await _userManager.FindByNameAsync(userLoginVm.UserNameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(userLoginVm.UserNameOrEmail);
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid username or email");
                    return View();
                }
            }
            var result = await _signInManager.PasswordSignInAsync(user, userLoginVm.Password, userLoginVm.RememberMe, true);
            if(!user.EmailConfirmed)
            {
                ModelState.AddModelError("", "Invalid username or email");
                return View();
            }
            
            if(result.IsLockedOut)
            {
                ModelState.AddModelError("", "account is lockout...");
                return View();
            }
            if(!result.Succeeded)
            {
                ModelState.AddModelError("", "Invalid username or email");
                return View();
            }

            return RedirectToAction("Index","Home");

        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var userProfileVM = new UserProfileVM
            {
                UserProfileUpdateVM = new UserProfileUpdateVM
                {
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email
                }
            };

            return View(userProfileVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Profile(UserProfileUpdateVM userProfileUpdateVm)
        {
            UserProfileVM userProfileVM = new();
            userProfileVM.UserProfileUpdateVM = userProfileUpdateVm;
            if (!ModelState.IsValid)
                return View(userProfileUpdateVm);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login");

            user.FullName = userProfileUpdateVm.FullName;
            user.Email = userProfileUpdateVm.Email;
            user.UserName = userProfileUpdateVm.UserName;

            if(userProfileUpdateVm.CurrentPassword != null && userProfileUpdateVm.NewPassword != null)
            {
                var respomse = await _userManager.ChangePasswordAsync(user, userProfileUpdateVm.CurrentPassword, userProfileUpdateVm.NewPassword);
                if(!respomse.Succeeded)
                {
                    foreach (var error in respomse.Errors)
                        ModelState.AddModelError("", error.Description);
                    return View(userProfileVM);
                }
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(userProfileVM);
            }
            await _signInManager.SignInAsync(user, true);

            return RedirectToAction("Index", "Home");
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM forgotPasswordVm)
        {
            if (!ModelState.IsValid)
                return View();

            var user = await _userManager.FindByEmailAsync(forgotPasswordVm.Email);
            if (user == null)
                return RedirectToAction("NotFound", "Error");
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var url = Url.Action("VerifyPassword", "Account", new { email = user.Email, token = token }, Request.Scheme);

            // create email

            using StreamReader reader = new StreamReader("wwwroot/templates/resetpassword.html");
            var body = reader.ReadToEnd();
            body = body.Replace("{{url}}", url);
            body = body.Replace("{{username}}", user.UserName);

            _emailService.SendEmail(user.Email, "Reset Password", body);

            TempData["Success"] = "Email sended to " + user.Email;
            return View();
        }


        public async Task<IActionResult> VerifyPassword(string token, string email)
        {
            TempData["token"] = token;
            TempData["email"] = email;

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null || !await _userManager.IsInRoleAsync(user, "member")) return RedirectToAction("notfound", "error");
            if (!await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", token))
                return RedirectToAction("notfound", "error");
            return RedirectToAction("resetpassword");
        }
        
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(PasswordResetVM request)
        {
            TempData["token"] = request.Token;
            TempData["email"] = request.Email;

            if (!ModelState.IsValid) return View();

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.IsInRoleAsync(user,"member")) return View();
            if (!await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", request.Token))
                return RedirectToAction("notfound", "error");
            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);
                }
                return View();

            }
            return RedirectToAction("Login");

        }


        public IActionResult FacebookLogin(string ReturnUrl)
        {
            string RedirectUrl = Url.Action("ExternalResponse", "Home", new { ReturnUrl = ReturnUrl });

            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", RedirectUrl);

            return new ChallengeResult("Facebook", properties);

        }

        public async Task<IActionResult> ExternalResponse(string ReturnUrl = "/")
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true);

                if (result.Succeeded)
                {
                    return Redirect(ReturnUrl);
                }
                else
                {
                    AppUser user = new AppUser();

                    user.Email = info.Principal.FindFirst(ClaimTypes.Email).Value;
                    string ExternalUserId = info.Principal.FindFirst(ClaimTypes.NameIdentifier).Value;

                    if (info.Principal.HasClaim(x => x.Type == ClaimTypes.Name))
                    {
                        string userName = info.Principal.FindFirst(ClaimTypes.Name).Value;

                        userName = userName.Replace(' ', '-').ToLower() + ExternalUserId.Substring(0, 5).ToString();

                        user.UserName = userName;
                    }
                    else
                    {
                        user.UserName = info.Principal.FindFirst(ClaimTypes.Email).Value;
                    }

                    IdentityResult createResult = await _userManager.CreateAsync(user);
                    if (createResult.Succeeded)
                    {
                        IdentityResult loginResult = await _userManager.AddLoginAsync(user, info);

                        if (loginResult.Succeeded)
                        {
                            await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true);
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Error");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Error");
                    }
                }
            }

            return RedirectToAction("notfound", "error");
        }

    }
}
