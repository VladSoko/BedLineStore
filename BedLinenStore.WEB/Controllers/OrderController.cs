using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly IExcelService excelService;

        public OrderController(IOrderService orderService,
            IExcelService excelService)
        {
            this.orderService = orderService;
            this.excelService = excelService;
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
        public async Task<IActionResult> DownloadOrders(DownloadOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var orders = orderService.GetAllByPeriod(model.FromDate, model.ToDate);

                if (orders?.Count() == 0)
                {
                    ModelState.AddModelError("", "За такой период нет заказов");
                }
                else
                {
                    var data = await excelService.GetExcelReportAsync(orders);
                    return new FileContentResult(data,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"Orders.xlsx"
                    };
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