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

        public Login(IHttpClientFactory httpClientFactory, IDataProtectionProvider provider)
        {
            _httpClient = httpClientFactory.CreateClient("Main");
            _protector = provider.CreateProtector("Example.SessionProtection");
        }

        public IActionResult Index()
        {
            return View("Login", new LoginModel());
        }

        [HttpPost]
        public async Task<IActionResult> CustomersLogin(LoginModel model)
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

        public IActionResult GetImage(string userId)
        {
            string prodilePicturePath = HttpContext.Session.GetEncryptedString("ProfilePicturePath", _protector);
            if (!string.IsNullOrEmpty(prodilePicturePath))
            {
                var imageBytes = System.IO.File.ReadAllBytes(prodilePicturePath);
                return File(imageBytes, "image/jpeg"); // Adjust content type as per your image type
            }
            return null;
        }
    }
}
