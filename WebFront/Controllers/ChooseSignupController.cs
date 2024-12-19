using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SCAPI.WebFront.Models;

namespace SCAPI.WebFront.Controllers
{
    public class ChooseSignupController : Controller
    {
        private readonly string _userUrl;
        private readonly string _providerUrl;
        public ChooseSignupController(IOptions<AppSettings> appSettings)
        {
            _userUrl = appSettings.Value.UserUrl;
            _providerUrl = appSettings.Value.ProviderUrl;
        }
        public IActionResult Index()
        {
            ViewBag.UserUrl = _userUrl;
            ViewBag.ProviderUrl = _providerUrl;
            return View();
        }
    }
}
