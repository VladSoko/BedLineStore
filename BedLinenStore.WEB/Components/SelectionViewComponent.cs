using BedLinenStore.WEB.Data;
using BedLinenStore.WEB.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BedLinenStore.WEB.Components
{
    public class SelectionViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext context;

        public SelectionViewComponent(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IViewComponentResult Invoke(int id)
        {
            CategoryViewModel categoryViewModel = new CategoryViewModel
            {
                Categories = context.Categories.ToList(),
                BedLineId = id,
            };
            return View(categoryViewModel);
        }
    }
}
