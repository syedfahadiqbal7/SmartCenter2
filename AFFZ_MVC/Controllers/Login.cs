using AFFZ_Customer.Models;
using AFFZ_Customer.Models.Partial;
using AFFZ_Customer.Utils;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace AFFZ_Customer.Controllers
{
    public class Login : Controller
    {

        private readonly HttpClient _httpClient;
        private readonly IDataProtector _protector;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<Login> _logger;
        public Login(IHttpClientFactory httpClientFactory, IDataProtectionProvider provider, IWebHostEnvironment environment, ILogger<Login> logger)
        {
            _httpClient = httpClientFactory.CreateClient("Main");
            _protector = provider.CreateProtector("Example.SessionProtection");
            _environment = environment;
            _logger = logger;
        }

        public IActionResult Index(string returnUrl = "")
        {
            // Save the returnUrl in ViewBag to use it on the login page
            ViewBag.ReturnUrl = returnUrl;
            return View("Login", new LoginModel());
        }

        [HttpPost]
        public async Task<IActionResult> CustomersLogin(LoginModel model, string returnUrl = null)
        {
            try
            {
                var response = await _httpClient.PostAsync("Customers/Login", Customs.GetJsonContent(model));
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                SResponse sResponse = JsonConvert.DeserializeObject<SResponse>(responseString);
                if (sResponse.StatusCode == HttpStatusCode.OK)
                {
                    Customers customerDetail = JsonConvert.DeserializeObject<Customers>(sResponse.Data.ToString());
                    HttpContext.Session.SetEncryptedString("UserId", $"{customerDetail?.CustomerId}", _protector);
                    HttpContext.Session.SetEncryptedString("RoleId", $"{customerDetail?.RoleId}", _protector);
                    HttpContext.Session.SetEncryptedString("ProfilePicturePath", $"{customerDetail?.ProfilePicture}", _protector);
                    HttpContext.Session.SetEncryptedString("Email", $"{customerDetail?.Email}", _protector);
                    HttpContext.Session.SetEncryptedString("MemberSince", $"{customerDetail?.CreatedDate.ToString("MMM yyyy")}", _protector);
                    HttpContext.Session.SetEncryptedString("FirstName", $"{customerDetail?.FirstName}", _protector);
                    HttpContext.Session.SetEncryptedString("CustomerName", $"{customerDetail?.CustomerName}", _protector);
                    string refcode = await GetReferralCode(customerDetail?.CustomerId);
                    decimal walletPoints = await GetWalletData(customerDetail?.CustomerId);
                    HttpContext.Session.SetEncryptedString("walletPoints", $"{walletPoints}", _protector);
                    HttpContext.Session.SetEncryptedString("ReferralCode", $"{refcode}", _protector);
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public IActionResult Logout()
        {
            try
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex) { return null; }
        }

        public IActionResult GetImage()
        {
            string prodilePicturePath = HttpContext.Session.GetEncryptedString("ProfilePicturePath", _protector);
            var filePath = Path.Combine(_environment.WebRootPath, "assets\\img\\profiles\\", prodilePicturePath);
            if (!string.IsNullOrEmpty(filePath) && System.IO.File.Exists(filePath))
            {
                var imageBytes = System.IO.File.ReadAllBytes(filePath);
                if (prodilePicturePath.Contains("png"))
                {
                    return File(imageBytes, "image/png"); // Adjust content type as per your image type
                }
                if (prodilePicturePath.Contains("jpeg"))
                {
                    return File(imageBytes, "image/jpeg"); // Adjust content type as per your image type
                }
            }
            return null;
        }
        //GetMemberSince
        public async Task<string> GetReferralCode(int? referrerCustomerId)
        {
            string ReferralCode = string.Empty;
            try
            {
                var response = await _httpClient.GetAsync($"Referral/GetUserReferral?CustomerId={referrerCustomerId}");
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                SResponse sResponse = JsonConvert.DeserializeObject<SResponse>(responseString);

                if (sResponse.StatusCode == HttpStatusCode.OK)
                {
                    if (sResponse.Data != null)
                    {
                        // Successfully created a referral
                        ReferralCode = sResponse.Data.ToString();
                    }
                    else
                    {
                        ReferralCode = "No Code.";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred: {ex.Message}");
                ReferralCode = ex.Message;
            }
            return ReferralCode;
        }
        public async Task<decimal> GetWalletData(int? referrerCustomerId)
        {
            decimal WalletPoints = Convert.ToDecimal("0.0");
            try
            {
                var response = await _httpClient.GetAsync($"Wallet/GetWalletPoints?CustomerId={referrerCustomerId}");
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                SResponse sResponse = JsonConvert.DeserializeObject<SResponse>(responseString);

                if (sResponse.StatusCode == HttpStatusCode.OK)
                {
                    if (sResponse.Data != null)
                    {
                        // Successfully created a referral
                        var wallet = JsonConvert.DeserializeObject<WalletModel>(sResponse.Data.ToString());
                        //ViewBag.ReferralCode = referral.ReferralCode;
                        WalletPoints = wallet.Points;
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred: {ex.Message}");
                throw;
            }
            return WalletPoints;
        }
    }
}
