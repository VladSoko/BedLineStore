using BedLinenStore.WEB.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BedLinenStore.WEB.Controllers
{
    public class ErrorController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Problem()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
