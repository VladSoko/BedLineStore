using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using BedLinenStore.WEB.Models;
using BedLinenStore.WEB.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BedLinenStore.WEB.Controllers
{
    public class ResetPasswordController : Controller
    {
        private readonly IUserService userService;
        private readonly IEmailSender emailSender;

        public ResetPasswordController(IUserService userService,
            IEmailSender emailSender)
        {
            this.userService = userService;
            this.emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = userService.GetByEmail(model.Email);
            if (user == null)
            {
                return Json(new
                {
                    status = "validationerror",
                    message = "Пользователь с такой почтой не существует"
                });
            }

            var result = userService.ResetPassword(user, model.Password);
            if (result)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            ModelState.AddModelError("", "Что-то пошло не так");
            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, DateTime date)
        {
            if (email == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }

            if (date < DateTime.Now)
            {
                return View("ExpiredTime");
            }

            LoginModel loginModel = new LoginModel()
            {
                Email = email
            };
            return View(loginModel);
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (ModelState.IsValid)
            {
                var user = userService.GetByEmail(email);

                if (user == null)
                {
                    return Json(new
                    {
                        status = "validationerror",
                        message = "Пользователь с такой почтой не существует"
                    });
                }
                else
                {
                    return await SendEmailToResetPassword(user.Email);
                }
            }

            return View(email);
        }
        
        public async Task<IActionResult> SendEmailToResetPassword(string email)
        {
            var callbackUrl = Url.Action(
                "ResetPassword",
                "ResetPassword",
                values: new {email = email, date = DateTime.Now.AddDays(1)},
                protocol: Request.Scheme);

            StringBuilder emailMessage = new StringBuilder
            (
                $"«Здравствуйте! Вы получили это письмо, так как " +
                $"Вами было запрошено восстановление пароля. Если это были не Вы, " +
                $"пожалуйста, свяжитесь с нашей службой поддержки по номеру +375 29 7444 009.<br/>" +
                $"Если восстановление пароля для Вас актуально, пожалуйста, перейдите по ссылке " +
                $"«<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Восстановить пароль</a>» для того, чтобы сбросить старый пароль и задать новый.<br/><br/>" +
                $"С уважением, команда The Lines»"
            );

            await emailSender.SendEmailAsync(email, "Восстановление парол",
                emailMessage.ToString());
            return PartialView("SendEmailSuccess");
        }
    }
}