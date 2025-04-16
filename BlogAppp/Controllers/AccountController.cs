using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BlogAppp.Models;
using BlogAppp.ViewModels;
using BlogAppp.IServices;

namespace BlogAppp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
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

        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
