using System.Linq;
using BedLinenStore.WEB.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BedLinenStore.WEB.Components
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private readonly ICartLineService cartLineService;

        public CartSummaryViewComponent(ICartLineService cartLineService)
        {
            this.cartLineService = cartLineService;
        }

        public IViewComponentResult Invoke()
        {
            var userCartLine = cartLineService.GetByEmail(User.Identity.Name);
            if (userCartLine == null)
                return View(0);
            return View(userCartLine.Products.Count());
        }
    }
}