using BedLinenStore.WEB.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BedLinenStore.WEB.Controllers
{
    [Authorize(Roles = "AuthorizedUser, Admin")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext context;

        public OrderController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult List()
        {
            var orders = context.Orders.Include(item => item.Products).ThenInclude(a => a.Category).Include(item => item.Products).ThenInclude(a => a.BedLinen);
            return View(orders);
        }

        public IActionResult Delete(int id)
        {
            var order = context.Orders.FirstOrDefault(item => item.Id == id);
            context.Orders.Remove(order);
            context.SaveChanges();
            return RedirectToAction("List");
        }

        public IActionResult Info(int id)
        {
            return PartialView(context.Orders
                .Include(item => item.Products)
                .ThenInclude(a => a.Category)
                .Include(item => item.Products)
                .ThenInclude(a => a.BedLinen)
                .FirstOrDefault(item => item.Id == id));
        }
    }
}
