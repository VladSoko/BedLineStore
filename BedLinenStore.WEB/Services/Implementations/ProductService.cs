using System.Linq;
using BedLinenStore.WEB.Data;
using BedLinenStore.WEB.Models.Entities;
using BedLinenStore.WEB.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BedLinenStore.WEB.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext context;

        public ProductService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Product GetByMainInfoAndCategory(int mainInfoId, int categoryId)
        {
            return context.Products
                .Include(product => product.MainInfo)
                .Include(product => product.Category)
                .FirstOrDefault(product => product.CategoryId == categoryId
                                           && product.MainInfoId == mainInfoId);
        }
    }
}