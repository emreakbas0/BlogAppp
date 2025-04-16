using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BlogAppp.Models;
using BlogAppp.ViewModels;
using BlogAppp.IServices;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BlogAppp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IBlogService _blogService;

        public AccountController(IAccountService accountService, UserManager<AppUser> userManager, IBlogService blogService)
        {
            _userManager = userManager;
            _blogService = blogService;
        
            _accountService = accountService;
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _accountService.RegisterAsync(model);

            if (result.Succeeded)
            {
                var loginModel = new LoginViewModel
                {
                    Username = model.Username!,
                    Password = model.Password!,
                    RememberMe = false
                };

                await _accountService.LoginAsync(loginModel);
                return RedirectToAction("Index", "Blog");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _accountService.LoginAsync(model);

            if (result.Succeeded)
                return RedirectToAction("Index", "Blog");

            ModelState.AddModelError("", "Geçersiz kullanıcı adı veya şifre");
            model.Username = string.Empty;
            model.Password = string.Empty;
            return View(model);
        }
        
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();

            var blogs = await _blogService.GetAllBlogsAsync(null);
            var userBlogs = blogs.Where(b => b.AppUserId == userId).ToList();

            var model = new ProfileViewModel
            {
                Username = user.UserName!,
                Email = user.Email!,
                Blogs = userBlogs
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {   
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(model.CurrentPassword) &&
            !string.IsNullOrWhiteSpace(model.NewPassword) &&
            !string.IsNullOrWhiteSpace(model.ConfirmPassword))
        {
            var result = await _accountService.ChangePasswordAsync(userId, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            else
            {
                ViewBag.PasswordChanged = true;
            }
        }

        var blogs = await _blogService.GetAllBlogsAsync(null);
        model.Blogs = blogs.Where(b => b.AppUserId == userId).ToList();
        model.Username = user.UserName ?? "";
        model.Email = user.Email ?? "";

        return View(model);
    }

        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
