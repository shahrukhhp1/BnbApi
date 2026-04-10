using Microsoft.AspNetCore.Mvc;

namespace BnbApi.Controllers
{
    /// <summary>
    /// Serves the Bounce n Bang game page at the site root (/) via MVC Razor view.
    /// </summary>
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
