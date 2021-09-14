using System;
using BedLinenStore.WEB.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BedLinenStore.WEB.Controllers
{
    public class ConfirmEmailController : Controller
    {
        private readonly IEmailSender emailSender;
        private readonly IUserService userService;

        public ConfirmEmailController(IEmailSender emailSender,
            IUserService userService)
        {
            this.emailSender = emailSender;
            this.userService = userService;
        }
        
        [HttpGet]
        public ActionResult ConfirmEmail(string email, int userId, DateTime date)
        {
            if (date < DateTime.Now)
            {
                return View("ExpiredTime");
            }
            
            if (email == null)
            {
                return RedirectToAction("Index", "Main");
            }

            var user = userService.GetById(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }

            var result = userService.ConfirmEmail(user, email);
            if (!result)
            {
                throw new InvalidOperationException($"Error confirming email for user with email '{email}':");
            }

            return View(user);
        }
    }
}