using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using BedLinenStore.WEB.Enums;
using BedLinenStore.WEB.Models;
using BedLinenStore.WEB.Models.Entities;
using BedLinenStore.WEB.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace BedLinenStore.WEB.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService userService;
        private readonly IEmailSender emailSender;

        public AccountController(IUserService userService,
            IEmailSender emailSender)
        {
            this.userService = userService;
            this.emailSender = emailSender;
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
                var user = userService.GetByEmail(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Пользователь с такой почтой не существует");
                    return View(model);
                }

                if (user.Password != model.Password)
                {
                    ModelState.AddModelError("", "Неверный пароль");
                    return View(model);
                }

                if (!user.ConfirmedEmail)
                {
                    var callbackUrl = Url.Action(
                        "SendEmailToConfirmEmail",
                        "Account",
                        values: new {email = user.Email, userId = user.Id},
                        protocol: Request.Scheme);
                    
                    ModelState.AddModelError("", 
                        $"Ваша почта не подтверждена. <a class='sendEmail' " +
                        $"href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Отправить письмо?</a>");
                    return View(model);
                }
                
                await Authenticate(user);
                return RedirectToAction("Index", "Main");
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
                if (userService.GetByEmail(model.Email) == null)
                {
                    var user = new User
                    {
                        Email = model.Email,
                        Password = model.Password,
                        Role = Role.AuthorizedUser,
                        CartLine = new CartLine()
                    };

                    var createdUser = userService.Create(user);

                    if (createdUser != null)
                    {
                        return await SendEmailToConfirmEmail(user.Email, createdUser.Id);
                    }
                    
                    return RedirectToAction("Index", "Main");
                }

                return Json(new
                {
                    status = "validationerror",
                    message = "Пользователь с этим логином уже существует"
                });
            }

            return View(model);
        }

        public async Task<IActionResult> SendEmailToConfirmEmail(string email, int id)
        {
            var callbackUrl = Url.Action(
                "ConfirmEmail",
                "ConfirmEmail",
                values: new {email = email, userId = id, date = DateTime.Now.AddDays(1)},
                protocol: Request.Scheme);
            
            StringBuilder emailMessage = new StringBuilder
            (
                $"Здравствуйте! Это письмо на подтверждение вашей электронной почты.<br/><br/>" +
                $"Перейдите по ссылке «<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Подтвердить адрес электронной " +
                $"почты</a>» чтобы закончить регистрацию"
            );
            
            await emailSender.SendEmailAsync(email, "Подтверждение электронной почты",
                emailMessage.ToString());
            return PartialView("SendEmailSuccess");
        }
        
        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
            };

            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
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