using BedLinenStore.WEB.Services.Interfaces;
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

        public IActionResult Info(int id)
        {
            return PartialView(orderService.GetById(id));
        }
    }
}