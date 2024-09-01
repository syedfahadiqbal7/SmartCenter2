using AFFZ_Customer.Models;
using AFFZ_Customer.Utils;
using Microsoft.AspNetCore.Mvc;

namespace AFFZ_Customer.Controllers
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
            return View("SignUp", new Customers());
        }

        [HttpPost]
        public async Task<IActionResult> CustomersRegister(Customers model)
        {
            try
            {
                var response = await _httpClient.PostAsync("Customers/Register", Customs.GetJsonContent(model));
                try
                {
                    response.EnsureSuccessStatusCode();
                }
                catch
                {
					return RedirectToAction("CustomersRegister", "Login");
				}

                var responseString = await response.Content.ReadAsStringAsync();

                ViewData["Message"] = "User Registered!";

                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
    }
}
