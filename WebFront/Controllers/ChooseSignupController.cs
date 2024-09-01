using Microsoft.AspNetCore.Mvc;

namespace SCAPI.WebFront.Controllers
{
    public class ChooseSignupController : Controller
    {
        public IConfiguration Configuration { get; }

        public ChooseSignupController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IActionResult Index()
        {
            ViewBag.UserUrl = Configuration["AppSettings:UserUrl"];
            ViewBag.ProviderUrl = Configuration["AppSettings:ProviderUrl"];
            return View();
        }
    }
}
