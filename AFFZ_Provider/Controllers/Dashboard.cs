using Microsoft.AspNetCore.Mvc;

namespace AFFZ_Provider.Controllers
{
    public class Dashboard : Controller
    {
        public IActionResult Index()
        {
            return View("Dashboard");
        }
    }
}
