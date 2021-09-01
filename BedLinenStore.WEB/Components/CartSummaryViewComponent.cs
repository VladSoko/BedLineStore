using BedLinenStore.WEB.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BedLinenStore.WEB.Components
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext context;

        public CartSummaryViewComponent(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IViewComponentResult Invoke()
        {
            var cartLines = context.CartLines.Include(item => item.User).Include(b => b.Products).ToList();
            var userCartLine = cartLines.FirstOrDefault(a => a.User.Email == User.Identity.Name);
            if (userCartLine == null)
            {
                return View(0);
            }
            else
            {
                return View(userCartLine.Products.Count());
            }            
        }
    }
}
