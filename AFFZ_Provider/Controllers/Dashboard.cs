using AFFZ_Provider.Models;
using AFFZ_Provider.Utils;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AFFZ_Provider.Controllers
{
    public class Dashboard : Controller
    {
        private readonly ILogger<Dashboard> _logger;
        private readonly IWebHostEnvironment _environment;
        private static string _merchantIdCat = string.Empty;
        private readonly HttpClient _httpClient;
        IDataProtector _protector;
        public Dashboard(ILogger<Dashboard> logger, IWebHostEnvironment environment, IHttpClientFactory httpClientFactory, IDataProtectionProvider provider)
        {

            _httpClient = httpClientFactory.CreateClient("Main");
            _protector = provider.CreateProtector("Example.SessionProtection");
            _logger = logger;
            _environment = environment;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                int merchantId = Convert.ToInt32(HttpContext.Session.GetEncryptedString("ProviderId", _protector));
                var GetTotalRevenue = await _httpClient.GetAsync($"Dashboard/GetTotalRevenueAsync?mId=" + merchantId);
                var GetTopRevenueService = await _httpClient.GetAsync($"Dashboard/GetTopRevenueServiceAsync?mid=" + merchantId);
                var GetRecentTransactions = await _httpClient.GetAsync($"Dashboard/GetRecentTransactionsAsync?mId=" + merchantId + "&count=5");

                if (GetTotalRevenue.IsSuccessStatusCode)
                {
                    ViewBag.GetTotalRevenue = await GetTotalRevenue.Content.ReadAsStringAsync();
                    ViewBag.GetTopRevenueService = await GetTopRevenueService.Content.ReadAsStringAsync();
                    ViewBag.GetRecentTransactions = await GetRecentTransactions.Content.ReadAsStringAsync();
                }
                else
                {
                    ViewBag.GetTotalRevenue = "";
                }
                if (GetTopRevenueService.IsSuccessStatusCode)
                {
                    var TRS = await GetTopRevenueService.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(TRS))
                    {
                        var reviewsTRS = JsonConvert.DeserializeObject<TopServiceRevenueDto>(TRS);
                        if (reviewsTRS == null)
                        {
                            ViewBag.GetTopRevenueService = new TopServiceRevenueDto();
                        }
                        else
                        {
                            ViewBag.GetTopRevenueService = reviewsTRS.ServiceName;
                        }
                    }
                    else
                    {
                        ViewBag.GetTopRevenueService = "0";
                    }

                }
                else
                {
                    ViewBag.GetTopRevenueService = "";
                }
                if (GetRecentTransactions.IsSuccessStatusCode)
                {
                    var RecentTrans = await GetRecentTransactions.Content.ReadAsStringAsync();
                    ViewBag.GetRecentTransactions = JsonConvert.DeserializeObject<List<PaymentHistory>>(RecentTrans);
                }
                else
                {
                    ViewBag.GetRecentTransactions = "";
                }
                var MiniDashboard = await _httpClient.GetAsync($"Dashboard/statistics?merchantId=" + merchantId);
                if (MiniDashboard.IsSuccessStatusCode)
                {
                    var _miniDashboardData = await MiniDashboard.Content.ReadAsStringAsync();
                    ViewBag.MiniDashBoardData = JsonConvert.DeserializeObject<MiniDashBoardData>(_miniDashboardData);
                }
                else
                {
                    ViewBag.MiniDashBoardData = "";
                }
                return View("Dashboard");
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
    public class MiniDashBoardData
    {
        public int TotalUsersServed { get; set; }
        public int TotalSuccessRequests { get; set; }
        public int TotalFailedRequests { get; set; }
        public int TotalInProgressRequests { get; set; }
    }
}
