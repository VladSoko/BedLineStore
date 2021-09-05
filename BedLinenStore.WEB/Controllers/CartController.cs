using BedLinenStore.WEB.Data;
using BedLinenStore.WEB.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using BedLinenStore.WEB.Services.Interfaces;

namespace BedLinenStore.WEB.Controllers
{
    [Authorize(Roles = "AuthorizedUser")]
    public class CartController : Controller
    {
        private readonly ICartLineService cartLineService;
        private readonly IOrderService orderService;

        public CartController(ICartLineService cartLineService,
            IOrderService orderService)
        {
            this.cartLineService = cartLineService;
            this.orderService = orderService;
        }

        public IActionResult Index()
        {
            return View(cartLineService.GetByEmail(User.Identity.Name));
        }

        public IActionResult Delete(int productId)
        {
            CartLine cartLine = cartLineService.GetByEmail(User.Identity.Name);
            cartLineService.DeleteProduct(cartLine, productId);
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            CartLine cartLine = cartLineService.GetByEmail(User.Identity.Name);

            order.Products = cartLine.Products;
            order.Email = User.Identity.Name;
            
            orderService.Create(order);

            return PartialView("OrderSuccessfully");
        }
    }
}
