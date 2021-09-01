using BedLinenStore.WEB.Data;
using BedLinenStore.WEB.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BedLinenStore.WEB.Controllers
{
    [Authorize(Roles = "AuthorizedUser")]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext context;

        public CartController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            CartLine cartLine = context.CartLines
                .Include(b => b.Products)
                .ThenInclude(a => a.Category)
                .Include(a => a.Products)
                .ThenInclude(a => a.BedLinen)
                .FirstOrDefault(item => item.User.Email == User.Identity.Name);

            return View(cartLine);
        }

        public IActionResult Delete(int productId)
        {
            Product product = context.Products.FirstOrDefault(item => item.Id == productId);
            CartLine cartLine = context.CartLines
                .Include(b => b.Products)
                .FirstOrDefault(item => item.User.Email == User.Identity.Name);

            cartLine.Products.Remove(product);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Checkout(Order order, int cartLineId)
        {
            CartLine cartLine = context.CartLines
                .Include(b => b.Products)
                .FirstOrDefault(item => item.Id == cartLineId);

            order.Products = cartLine.Products;

            order.Email = User.Identity.Name;
            context.Orders.Add(order);
            context.SaveChanges();

            return PartialView("OrderSuccessfully");
        }
    }
}
