using BedLinenStore.WEB.Data;
using BedLinenStore.WEB.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BedLinenStore.WEB.Controllers
{
    [Authorize(Roles = "Admin, AuthorizedUser")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext context;

        public ProductController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [AllowAnonymous]
        public IActionResult List()
        {
            return View(context.MainInfos.ToList());
        }

        public IActionResult AddToCart(int mainInfoId, int categoryId)
        {
            MainInfo mainInfo = context.MainInfos.FirstOrDefault(item => item.Id == mainInfoId);
            Category category = context.Categories.First(item => item.Id == categoryId);

            User user = context.Users.FirstOrDefault(item => item.Email == User.Identity.Name);
            CartLine cartLine = context.CartLines
                .Include(b => b.Products)
                .FirstOrDefault(item => item.User.Email == user.Email);


            Product product = new Product
            {
                MainInfo = mainInfo,
                Category = context.Categories.First(item => item.Id == categoryId),
            };

            Product product1 =
                cartLine.Products.FirstOrDefault(item =>
                    item.CategoryId == categoryId && item.MainInfoId == mainInfoId);
            if (product1 != null)
            {
                return PartialView("AddToCartError");
            }

            cartLine.Products.Add(product);

            context.SaveChanges();
            return PartialView("AddToCartSuccessfully", product);
        }
    }
}