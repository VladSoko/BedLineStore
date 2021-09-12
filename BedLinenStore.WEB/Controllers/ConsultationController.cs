using System;
using System.Text;
using System.Threading.Tasks;
using BedLinenStore.WEB.Models;
using BedLinenStore.WEB.Models.Entities;
using BedLinenStore.WEB.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BedLinenStore.WEB.Controllers
{
    public class ConsultationController : Controller
    {
        private readonly IConsultationInfoService consultationInfoService;
        private readonly IConsultationDateService consultationDateService;
        private readonly IEmailSender emailSender;

        public ConsultationController(IConsultationInfoService consultationInfoService,
            IConsultationDateService consultationDateService,
            IEmailSender emailSender)
        {
            this.consultationInfoService = consultationInfoService;
            this.consultationDateService = consultationDateService;
            this.emailSender = emailSender;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateConsultationDate()
        {
            var consultationDate = new ConsultationDateViewModel
            {
                Date = DateTime.Now.Date
            };
            return View(consultationDate);
        }

        [HttpGet]
        public IActionResult GetNumberConsultationByDate(DateTime date)
        {
            var consultationDate = consultationDateService.GetByDate(date);

            if (consultationDate == null)
            {
                return NoContent();
            }

            ConsultationNumberViewModel consultationNumber = new ConsultationNumberViewModel
            {
                MaxConsultationNumber = consultationDate.ConsultationsNumber,
                ConsultationNumber = consultationDate.ConsultationInfos.Count
            };

            return Ok(consultationNumber);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateConsultationDate(ConsultationDateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var consultationDate = consultationDateService.GetByDate(model.Date);

                if (consultationDate != null)
                {
                    ModelState.AddModelError("", "Эта дата уже добавлена в базу");
                    return View(model);
                }

                var consultationDateForCreate = new ConsultationDate
                {
                    Date = model.Date,
                    ConsultationsNumber = model.ConsultationsNumber
                };
                consultationDateService.CreateConsultation(consultationDateForCreate);

                return PartialView("SuccessModelPage", "Дата добавлена");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult CreateUserConsultation()
        {
            var consultationInfo = new ConsultationInfoViewModel
            {
                Email = User?.Identity.Name,
                Date = DateTime.Now.Date
            };
            return View(consultationInfo);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserConsultation(ConsultationInfoViewModel info)
        {
            if (ModelState.IsValid)
            {
                var consultationInfo = consultationInfoService.GetInfoByEmail(info.Email);

                if (consultationInfo != null)
                {
                    return PartialView("ErrorModelPage", "Вы уже забронировали консультацию");
                }

                var consultationDate = consultationDateService.GetByDate(info.Date);

                if (consultationDate == null)
                {
                    return Json(new
                    {
                        status = "validationerror",
                        message = "Забронировать консультацию на выбранную дату невозможно"
                    });
                }

                if (consultationDate.ConsultationsNumber == consultationDate.ConsultationInfos.Count)
                {
                    return Json(new
                    {
                        status = "validationerror",
                        message = "Превышено количество консультаций на этот день"
                    });
                }

                var consultationInfoForCreate = new ConsultationInfo
                {
                    Name = info.Name,
                    Surname = info.Surname,
                    Email = info.Email,
                    PhoneNumber = info.PhoneNumber,
                    ConsultationDateId = consultationDate.Id
                };
                consultationInfoService.CreateConsultation(consultationInfoForCreate);

                StringBuilder emailMessage = new StringBuilder
                (
                    $"Здравствуйте! Данным письмом мы подтверждаем, " +
                    $"что Вами была забронирована консультация со специалистом магазина «The Lines». " +
                    $"В течение двух дней мы с Вами свяжемся для уточнения времени консультации. " +
                    $"С уважением, команда The Lines<br/><br/>"
                );

                emailMessage.AppendLine();
                emailMessage.Append($"Данные о консультации:<br/>" +
                                    $"Имя: {info.Name}<br/>" +
                                    $"Фамилия: {info.Surname}<br/>" +
                                    $"Почта: {info.Email}<br/>" +
                                    $"Дата консультации: {info.Date.ToString("d")}<br/>");

                await emailSender.SendEmailAsync(info.Email, "Подтверждение консультации", emailMessage.ToString());

                return PartialView("SuccessModelPage", "На вашу почту должно прийти письмо с подтверждение");
            }

            return View(info);
        }
    }
}