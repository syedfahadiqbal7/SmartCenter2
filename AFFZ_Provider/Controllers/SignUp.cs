using AFFZ_Provider.Models;
using AFFZ_Provider.Utils;
using Microsoft.AspNetCore.Mvc;

namespace AFFZ_Provider.Controllers
{
    public class SignUp : Controller
    {
        private readonly HttpClient _httpClient;

        public SignUp(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Main");
        }

        public IActionResult Index()
        {
            return View("SignUp", new ProviderUser());
        }

        [HttpPost]
        public async Task<IActionResult> ProvidersRegister(ProviderUser model)
        {
            try
            {
                var response = await _httpClient.PostAsync("Providers/Register", Customs.GetJsonContent(model));
                //response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();

                ViewData["Message"] = "Provider Registered!";

                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
    }
}
