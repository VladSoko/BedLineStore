using BedLinenStore.WEB.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using BedLinenStore.WEB.Services.Interfaces;

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
