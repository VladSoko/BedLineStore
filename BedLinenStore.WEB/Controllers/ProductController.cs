using BedLinenStore.WEB.Data;
using BedLinenStore.WEB.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using BedLinenStore.WEB.Services.Interfaces;

namespace BedLinenStore.WEB.Controllers
{
    [Authorize(Roles = "Admin, AuthorizedUser")]
    public class ProductController : Controller
    {
        private readonly IMainInfoService mainInfoService;
        private readonly IUserService userService;
        private readonly ICartLineService cartLineService;
        private readonly IProductService productService;

        public ProductController(
            IMainInfoService mainInfoService,
            IUserService userService,
            ICartLineService cartLineService,
            IProductService productService)
        {
            this.mainInfoService = mainInfoService;
            this.userService = userService;
            this.cartLineService = cartLineService;
            this.productService = productService;
        }

        [AllowAnonymous]
        public IActionResult List()
        {
            return View(mainInfoService.GetAll());
        }

        public IActionResult AddToCart(int mainInfoId, int categoryId)
        {
            var product = productService.GetByMainInfoAndCategory(mainInfoId, categoryId);
            User user = userService.GetByEmail(User.Identity.Name);

            if (cartLineService.IsProductExist(user.CartLine, product.Id))
            {
                return PartialView("AddToCartError");
            }

            cartLineService.AddProduct(user.CartLine, product);
            return PartialView("AddToCartSuccessfully", product);
        }
    }
}