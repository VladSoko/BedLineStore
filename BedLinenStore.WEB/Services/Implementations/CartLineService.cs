using System.Linq;
using BedLinenStore.WEB.Data;
using BedLinenStore.WEB.Models.Entities;
using BedLinenStore.WEB.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BedLinenStore.WEB.Services.Implementations
{
    public class CartLineService : ICartLineService
    {
        private readonly ApplicationDbContext context;

        public CartLineService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public CartLine GetByEmail(string email)
        {
            return context.CartLines
                .Include(b => b.Products)
                .ThenInclude(a => a.Category)
                .Include(a => a.Products)
                .ThenInclude(a => a.MainInfo)
                .FirstOrDefault(item => item.User.Email == email);
        }

        public bool IsProductExist(CartLine cartLine, int productId)
        {
            return cartLine.Products.FirstOrDefault(product => product.Id == productId) != null;
        }

        public void AddProduct(CartLine cartLine, Product product)
        {
            cartLine.Products.Add(product);
            context.SaveChanges();
        }

        public void DeleteProduct(CartLine cartLine, int productId)
        {
            var product = context.Products.FirstOrDefault(item => item.Id == productId);
            cartLine.Products.Remove(product);
            context.SaveChanges();
        }

        public void Delete(CartLine cartLine)
        {
            context.CartLines.Remove(cartLine);
            context.SaveChanges();
        }
    }
}