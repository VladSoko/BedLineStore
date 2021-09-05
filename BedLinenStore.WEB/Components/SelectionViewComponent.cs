using BedLinenStore.WEB.Models;
using BedLinenStore.WEB.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BedLinenStore.WEB.Components
{
    public class SelectionViewComponent : ViewComponent
    {
        private readonly ICategoryService categoryService;

        public SelectionViewComponent(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public IViewComponentResult Invoke(int id)
        {
            var categoryViewModel = new CategoryViewModel
            {
                Categories = categoryService.GetAll(),
                BedLineId = id
            };
            return View(categoryViewModel);
        }
    }
}