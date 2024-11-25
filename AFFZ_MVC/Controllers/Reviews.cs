using AFFZ_Customer.Models;
using AFFZ_Customer.Utils;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AFFZ_Customer.Controllers
{
    [Route("UserReviews")]
    public class Reviews : Controller
    {
        private readonly IDataProtector _protector;
        private readonly ILogger<Reviews> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly HttpClient _httpClient;

        public Reviews(IHttpClientFactory httpClientFactory, IDataProtectionProvider provider, ILogger<Reviews> logger, IWebHostEnvironment environment)
        {
            _protector = provider.CreateProtector("Example.SessionProtection");
            _httpClient = httpClientFactory.CreateClient("Main");
            _logger = logger;
            _environment = environment;
        }
        public async Task<ActionResult> Index()
        {
            ViewBag.CustomerId = HttpContext.Session.GetEncryptedString("UserId", _protector);
            ViewBag.FirstName = HttpContext.Session.GetEncryptedString("FirstName", _protector);
            ViewBag.MemberSince = HttpContext.Session.GetEncryptedString("MemberSince", _protector);

            string userId = HttpContext.Session.GetEncryptedString("UserId", _protector); // Placeholder for session user ID retrieval
            try
            {
                var jsonResponse = await _httpClient.GetAsync($"ReViewsApi/GetUserReviewList?userId={userId}");
                jsonResponse.EnsureSuccessStatusCode();
                if (jsonResponse != null)
                {
                    var responseString = await jsonResponse.Content.ReadAsStringAsync();
                    List<ReViewDto> MyReviews = JsonConvert.DeserializeObject<List<ReViewDto>>(responseString);
                    ViewBag.MyReviews = MyReviews;
                }
                else
                {
                    _logger.LogWarning("Empty response received from API for userId: {UserId}", userId);
                    ViewBag.ResponseForDisCountFromMerchant = new List<ReViewDto>();
                }
            }
            catch (JsonSerializationException ex)
            {
                _logger.LogError(ex, "JSON deserialization error for userId: {UserId}", userId);
                ViewBag.ResponseForDisCountFromMerchant = new List<ReViewDto>();
                ModelState.AddModelError(string.Empty, "Failed to load data.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching data for userId: {UserId}", userId);
                ViewBag.ResponseForDisCountFromMerchant = new List<ReViewDto>();
                ModelState.AddModelError(string.Empty, "An unexpected error occurred while loading data.");
            }

            if (TempData.TryGetValue("SaveResponse", out var saveResponse))
            {
                ViewBag.SaveResponse = saveResponse.ToString();
            }




            return View("Reviews");
        }
    }
}
