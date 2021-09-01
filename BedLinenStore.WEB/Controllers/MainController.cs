using BedLinenStore.WEB.Data;
using BedLinenStore.WEB.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BedLinenStore.WEB.Controllers
{
    public class MainController : Controller
    {
        private readonly ApplicationDbContext context;

        public MainController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendRequest(FreelanceSewing freelanceSewing)
        {
            context.FreelanceSewings.Add(freelanceSewing);
            context.SaveChanges();
            return PartialView("RequestSuccessfully", freelanceSewing);
        }
    }
}
