
using AFFZ_Provider.Models;
using AFFZ_Provider.Models.Partial;
using AFFZ_Provider.Utils;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace AFFZ_Provider.Controllers
{
    public class Login : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IDataProtector _protector;
        private readonly IWebHostEnvironment _environment;

        public Login(IHttpClientFactory httpClientFactory, IDataProtectionProvider provider, IWebHostEnvironment environment)
        {
            _httpClient = httpClientFactory.CreateClient("Main");
            _protector = provider.CreateProtector("Example.SessionProtection");
            _environment = environment;
        }

        public IActionResult Index()
        {
            return View("Login", new LoginModel());
        }

        [HttpPost]
        public async Task<IActionResult> ProviderLogin(LoginModel model)
        {
            try
            {
                var response = await _httpClient.PostAsync("Providers/Login", Customs.GetJsonContent(model));
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                SResponse sResponse = JsonConvert.DeserializeObject<SResponse>(responseString);



                if (sResponse.StatusCode == HttpStatusCode.OK)
                {

                    ProviderUser providerDetail = JsonConvert.DeserializeObject<ProviderUser>(sResponse.Data.ToString());

                    //HttpContext.Session.SetEncryptedString("ProviderId", $"{providerDetail?.ProviderId}", _protector);
                    //HttpContext.Session.SetEncryptedString("RoleId", $"{providerDetail?.RoleId}", _protector);

                    HttpContext.Session.SetEncryptedString("ProviderId", $"{providerDetail?.ProviderId}", _protector);
                    HttpContext.Session.SetEncryptedString("ProviderName", $"{providerDetail?.ProviderName}", _protector);
                    HttpContext.Session.SetEncryptedString("Email", $"{providerDetail?.Email}", _protector);
                    HttpContext.Session.SetEncryptedString("RoleId", $"{providerDetail?.RoleId}", _protector);
                    HttpContext.Session.SetEncryptedString("IsActive", $"{providerDetail?.IsActive}", _protector);
                    HttpContext.Session.SetEncryptedString("ProfilePicturePath", $"{providerDetail?.ProfilePicture}", _protector);

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

            var _uploadPath = $"\\User_{HttpContext.Session.GetEncryptedString("ProviderId", _protector)}";
            var filePath = prodilePicturePath;// Path.Combine(_environment.WebRootPath, _uploadPath, prodilePicturePath);
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
    }
}
