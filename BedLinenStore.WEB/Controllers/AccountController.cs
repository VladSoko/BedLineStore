using BedLinenStore.WEB.Data;
using BedLinenStore.WEB.Enums;
using BedLinenStore.WEB.Models;
using BedLinenStore.WEB.Models.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BedLinenStore.WEB.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext context;

        public AccountController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = context.Users.FirstOrDefault(item => item.Email == model.Email);

                if (user != null && user.Password == model.Password)
                {                    
                    await Authenticate(user);
                    return RedirectToAction("Index", "Main");
                }
                else
                {
                    ModelState.AddModelError("", "Неверный логин и(или) пароль");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                if (context.Users.FirstOrDefault(item => item.Email == model.Email) == null)
                {
                    User user = new User
                    {
                        Email = model.Email,
                        Password = model.Password,
                        Role = Role.AuthorizedUser,
                    };

                    CartLine newCartLine = new CartLine
                    {
                        User = user
                    };

                    context.CartLines.Add(newCartLine);
                    context.Users.Add(user);
                    context.SaveChanges();

                    await Authenticate(user);

                    return RedirectToAction("Index", "Main");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с этим логином уже существует");
                }

            }
            return View(model);
        }

        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
