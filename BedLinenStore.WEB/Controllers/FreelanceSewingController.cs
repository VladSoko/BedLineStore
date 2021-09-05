using BedLinenStore.WEB.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BedLinenStore.WEB.Controllers
{
    public class FreelanceSewingController : Controller
    {
        private readonly IFreelanceSewingService freelanceSewingService;

        public FreelanceSewingController(IFreelanceSewingService freelanceSewingService)
        {
            this.freelanceSewingService = freelanceSewingService;
        }

        public IActionResult List()
        {
            return View(freelanceSewingService.GetAll());
        }

        public IActionResult Delete(int id)
        {
            freelanceSewingService.DeleteById(id);
            return RedirectToAction("List");
        }
    }
}