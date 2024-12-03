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

			var merchantResponse = await _httpClient.GetAsync($"Provider/GetByProvider/{providerId}");
			if (merchantResponse.IsSuccessStatusCode)
			{
				var merchantResponseString = await merchantResponse.Content.ReadAsStringAsync();
				ProviderMerchant providerMerchant = JsonConvert.DeserializeObject<ProviderMerchant>(merchantResponseString);
				ViewBag.MerchantStatus = providerMerchant.IsActive ? "Active" : "Under Review";
			}
			else
			{
				ViewBag.MerchantStatus = "Not Linked";
			}
            var merchantResponse = await _httpClient.GetAsync($"Provider/GetByProvider/{providerId}");
            if (merchantResponse.IsSuccessStatusCode)
            {
                var merchantResponseString = await merchantResponse.Content.ReadAsStringAsync();
                ProviderMerchant providerMerchant = JsonConvert.DeserializeObject<ProviderMerchant>(merchantResponseString);
                ViewBag.MerchantStatus = providerMerchant.IsActive ? "Active" : "Under Review";
            }
            else
            {
                ViewBag.MerchantStatus = "Not Linked";
            }
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

				// Handle file uploads here (e.g., profile picture, passport)
				if (model.ProfileImage != null)
				{
					string fileName = $"ProfilePicture{model.ProfileImage.FileName.Substring(model.ProfileImage.FileName.LastIndexOf("."))}";
					profilePicturePath = Path.Combine(_uploadPath, fileName);
					using (var fileStream = new FileStream(profilePicturePath, FileMode.Create))
					{
						await model.ProfileImage.CopyToAsync(fileStream);
					}
				}

				model.ProfilePicture = profilePicturePath;

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
		[HttpPost]
		public async Task<IActionResult> LinkMerchant(Merchant model)
		{
			try
			{
				string providerId = HttpContext.Session.GetEncryptedString("ProviderId", _protector);
				model.CreatedDate = DateTime.Now;
				model.IsActive = false;

				var response = await _httpClient.PostAsync("Providers/ProviderMerchantLink?providerId="+ providerId, Customs.GetJsonContent(model));
				response.EnsureSuccessStatusCode();

				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				return RedirectToAction("Index");
			}
		}
	}
}
