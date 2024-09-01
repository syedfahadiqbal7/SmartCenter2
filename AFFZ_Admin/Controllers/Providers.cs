using AFFZ_Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AFFZ_Admin.Controllers
{
    public class Providers : Controller
    {
        private readonly HttpClient _httpClient;

        public Providers(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Main");
        }


        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync("Providers");

                var responseString = await response.Content.ReadAsStringAsync();
                List<ProviderUser> providers = JsonConvert.DeserializeObject<List<ProviderUser>>(responseString);
                return View("Providers", providers);

            }
            catch (Exception ex)
            {
                return View("Dashboard");
            }
        }

        public IActionResult Download(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                var fileName = Path.GetFileName(filePath);

                return File(fileBytes, "application/octet-stream", fileName);
            }
            return NotFound();
        }
    }
}
