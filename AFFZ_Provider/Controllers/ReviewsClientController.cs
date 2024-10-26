using AFFZ_Provider.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AFFZ_Provider.Controllers
{
    public class ReviewsClientController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private static string _merchantIdCat = string.Empty;
        private readonly HttpClient _httpClient;
        private readonly IDataProtector _protector;
        private ILogger<ReviewsClientController> _logger;
        public ReviewsClientController(ILogger<ReviewsClientController> logger, IWebHostEnvironment environment, IHttpClientFactory httpClientFactory, IDataProtectionProvider provider)
        {
            _httpClient = httpClientFactory.CreateClient("Main");
            _protector = provider.CreateProtector("Example.SessionProtection");
            _logger = logger;
            _environment = environment;
        }

        public async Task<IActionResult> ProviderReviews()
        {
            var response = await _httpClient.GetAsync($"ReviewsApi/GetAllReviews");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var reviews = JsonConvert.DeserializeObject<List<ReviewViewModel>>(content);
                return View(reviews);
            }
            else
            {
                return View(new List<ReviewViewModel>());
            }
        }
    }
}
