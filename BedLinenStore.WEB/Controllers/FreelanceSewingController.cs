using BedLinenStore.WEB.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BedLinenStore.WEB.Controllers
{
    public class FreelanceSewingController : Controller
    {
        private readonly ApplicationDbContext context;

        public FreelanceSewingController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult List()
        {
            return View(context.FreelanceSewings.ToList());
        }

        public IActionResult Delete(int id)
        {
            var freelanceSewing = context.FreelanceSewings.FirstOrDefault(item => item.Id == id);
            context.FreelanceSewings.Remove(freelanceSewing);
            context.SaveChanges();
            return RedirectToAction("List");
        }
    }
}
