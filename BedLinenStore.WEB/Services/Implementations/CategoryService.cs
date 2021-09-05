using System.Collections.Generic;
using System.Linq;
using BedLinenStore.WEB.Data;
using BedLinenStore.WEB.Models.Entities;
using BedLinenStore.WEB.Services.Interfaces;

namespace BedLinenStore.WEB.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext context;

        public CategoryService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Category GetById(int id)
        {
            return context.Categories
                .FirstOrDefault(item => item.Id == id);
        }

        public IEnumerable<Category> GetAll()
        {
            return context.Categories.ToList();
        }
    }
}