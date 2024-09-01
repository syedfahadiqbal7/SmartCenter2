using AFFZ_Customer.Utils;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace AFFZ_Customer.Controllers
{
    [Route("UserReviews")]
    public class Reviews : Controller
    {
        private readonly IDataProtector _protector;

        public Reviews(IHttpClientFactory httpClientFactory, IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("Example.SessionProtection");
        }
        public IActionResult Index()
        {
            ViewBag.CustomerId = HttpContext.Session.GetEncryptedString("UserId", _protector);
            ViewBag.FirstName = HttpContext.Session.GetEncryptedString("FirstName", _protector);
            ViewBag.MemberSince = HttpContext.Session.GetEncryptedString("MemberSince", _protector);
            return View("Reviews");
        }
    }
}
