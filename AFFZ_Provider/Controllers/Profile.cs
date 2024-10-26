using AFFZ_Provider.Models;
using AFFZ_Provider.Models.Partial;
using AFFZ_Provider.Utils;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace AFFZ_Provider.Controllers
{
    public class Profile : Controller
    {
        private readonly HttpClient _httpClient;
        string _uploadPath;
        IDataProtector _protector;

        public Profile(IHttpClientFactory httpClientFactory, IConfiguration configuration, IDataProtectionProvider provider)
        {
            _httpClient = httpClientFactory.CreateClient("Main");
            _uploadPath = configuration.GetValue<string>("ProfilePicStorage:UploadPath");

            _protector = provider.CreateProtector("Example.SessionProtection");
        }

        public async Task<IActionResult> Index()
        {

            string providerId = HttpContext.Session.GetEncryptedString("ProviderId", _protector);
            var response = await _httpClient.GetAsync($"Providers/{providerId}");

            var responseString = await response.Content.ReadAsStringAsync();
            ProviderUser customer = JsonConvert.DeserializeObject<ProviderUser>(responseString);
            return View("Profile", customer);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateProfile(ProviderUser model)
        {
            try
            {
                string profilePicturePath = string.Empty;
                string passportPath = string.Empty;
                string emiratesIdPath = string.Empty;
                string drivingLicensePath = string.Empty;
                _uploadPath += $"\\User_{model.ProviderId}";

                if (!Directory.Exists(_uploadPath))
                {
                    Directory.CreateDirectory(_uploadPath);
                }
                if (model.ProfileImage != null)
                {
                    string userId = HttpContext.Session.GetEncryptedString("ProviderId", _protector);
                    string fileName = $"ProfilePicture{model.ProfileImage.FileName.Substring(model.ProfileImage.FileName.LastIndexOf("."))}";
                    profilePicturePath = Path.Combine(_uploadPath, fileName);
                    using (var fileStream = new FileStream(profilePicturePath, FileMode.Create))
                    {
                        await model.ProfileImage.CopyToAsync(fileStream);
                    }
                }

                if (model.PassportFile != null)
                {
                    string userId = HttpContext.Session.GetEncryptedString("ProviderId", _protector);
                    string fileName = $"Passport{model.PassportFile.FileName.Substring(model.PassportFile.FileName.LastIndexOf("."))}";
                    passportPath = Path.Combine(_uploadPath, fileName);
                    using (var fileStream = new FileStream(passportPath, FileMode.Create))
                    {
                        await model.PassportFile.CopyToAsync(fileStream);
                    }
                }
                if (model.EmiratesIdFile != null)
                {
                    string userId = HttpContext.Session.GetEncryptedString("ProviderId", _protector);
                    string fileName = $"EmiratesId{model.EmiratesIdFile.FileName.Substring(model.EmiratesIdFile.FileName.LastIndexOf("."))}";
                    emiratesIdPath = Path.Combine(_uploadPath, fileName);
                    using (var fileStream = new FileStream(emiratesIdPath, FileMode.Create))
                    {
                        await model.EmiratesIdFile.CopyToAsync(fileStream);
                    }
                }
                if (model.DrivingLicenseFile != null)
                {
                    string userId = HttpContext.Session.GetEncryptedString("ProviderId", _protector);
                    string fileName = $"DrivingLicense{model.DrivingLicenseFile.FileName.Substring(model.DrivingLicenseFile.FileName.LastIndexOf("."))}";
                    drivingLicensePath = Path.Combine(_uploadPath, fileName);
                    using (var fileStream = new FileStream(drivingLicensePath, FileMode.Create))
                    {
                        await model.DrivingLicenseFile.CopyToAsync(fileStream);
                    }
                }

                model.ProfilePicture = profilePicturePath;
                model.Passport = passportPath;
                model.EmiratesId = emiratesIdPath;
                model.DrivingLicense = drivingLicensePath;

                var response = await _httpClient.PostAsync("Providers/UpdateProfile", Customs.GetJsonContent(model));
                response.EnsureSuccessStatusCode();


                var responseString = await response.Content.ReadAsStringAsync();
                SResponse sResponse = JsonConvert.DeserializeObject<SResponse>(responseString);

                if (sResponse.StatusCode == HttpStatusCode.OK)
                {
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
    }
}
