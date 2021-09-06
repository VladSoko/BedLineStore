using System.IO;
using System.Linq;
using BedLinenStore.WEB.Models;
using BedLinenStore.WEB.Services.Interfaces;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BedLinenStore.WEB.Controllers
{
    [Authorize(Roles = "AuthorizedUser, Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        public IActionResult List()
        {
            return View(orderService.GetAll());
        }

        public IActionResult Delete(int id)
        {
            orderService.DeleteById(id);
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult DownloadOrders()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DownloadOrders(DownloadOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var orders = orderService.GetAllByPeriod(model.FromDate, model.ToDate);

                if (orders?.Count() == 0)
                {
                    ModelState.AddModelError("", "За такой период нет заказов");
                }

                using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
                {
                    var worksheet = workbook.Worksheets.Add("Orders");

                    worksheet.Cell("A1").Value = "Номер заказа";
                    worksheet.Cell("B1").Value = "Дата заказа";
                    worksheet.Cell("C1").Value = "Данные о заказе";
                    worksheet.Cell("D1").Value = "ФИО";
                    worksheet.Cell("E1").Value = "Номер телефона";
                    worksheet.Cell("F1").Value = "Адресс доставки";
                    worksheet.Row(1).Style.Font.Bold = true;

                    // //нумерация строк/столбцов начинается с индекса 1 (не 0)
                    // for (int i = 0; i < phoneBrands.Count; i++)
                    // {
                    //     worksheet.Cell(i + 2, 1).Value = phoneBrands[i].Title;
                    //     worksheet.Cell(i + 2, 2).Value = string.Join(", ", phoneBrands[i].PhoneModels.Select(x => x.Title));
                    // }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        stream.Flush();

                        return new FileContentResult(stream.ToArray(),
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                        {
                            FileDownloadName = $"Orders.xlsx"
                        };
                    }
                }
            }

            return View(model);
        }

        public IActionResult Info(int id)
        {
            return PartialView(orderService.GetById(id));
        }
    }
}