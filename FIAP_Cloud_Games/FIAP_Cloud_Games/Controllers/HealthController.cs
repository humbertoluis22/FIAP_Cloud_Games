using Microsoft.AspNetCore.Mvc;

namespace FIAP_Cloud_Games.Controllers
{
    public class HealthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
