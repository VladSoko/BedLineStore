using BedLinenStore.WEB.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BedLinenStore.WEB.Controllers
{
    [Authorize(Roles = "Admin, AuthorizedUser")]
    public class ProductController : Controller
    {
        private readonly ICartLineService cartLineService;
        private readonly IMainInfoService mainInfoService;
        private readonly IProductService productService;
        private readonly IUserService userService;

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
            var cartLine = cartLineService.GetByEmail(User.Identity.Name);

            if (cartLineService.IsProductExist(cartLine, product.Id)) return PartialView("AddToCartError");

            cartLineService.AddProduct(cartLine, product);
            return PartialView("AddToCartSuccessfully", product);
        }
    }
}