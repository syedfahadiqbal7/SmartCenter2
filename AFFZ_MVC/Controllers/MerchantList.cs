using AFFZ_Customer.Models;
using AFFZ_Customer.Utils;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AFFZ_Customer.Controllers
{
	public class MerchantList : Controller
	{
		private readonly ILogger<MerchantList> _logger;
		private static string _merchantIdCat = string.Empty;
		private readonly HttpClient _httpClient;
		IDataProtector _protector;
		public MerchantList(IHttpClientFactory httpClientFactory, ILogger<MerchantList> logger, IDataProtectionProvider provider)
		{
			_httpClient = httpClientFactory.CreateClient("Main");
			_protector = provider.CreateProtector("Example.SessionProtection");
			_logger = logger;
		}
		[HttpGet]
		public async Task<ActionResult> SelectedMerchantList(string catName)
		{
			_logger.LogInformation("Index method called with CatName: {CatName}", catName);

			if (!string.IsNullOrEmpty(catName))
			{
				_merchantIdCat = catName;
			}

			List<CatWithMerchant> categories = new List<CatWithMerchant>();

			try
			{
				var jsonResponse = await _httpClient.GetAsync($"CategoryWithMerchant/GetServiceListByMerchant?CatName={_merchantIdCat}");
				jsonResponse.EnsureSuccessStatusCode();
				if (jsonResponse != null)
				{
					var responseString = await jsonResponse.Content.ReadAsStringAsync();
					categories = JsonConvert.DeserializeObject<List<CatWithMerchant>>(responseString);
					ViewBag.SubCategoriesWithMerchant = categories;
					//categories = JsonConvert.DeserializeObject<List<CatWithMerchant>>(jsonResponse);
					//ViewBag.SubCategoriesWithMerchant = categories;
				}
				else
				{
					_logger.LogWarning("Received empty response from API");
					ViewBag.SubCategoriesWithMerchant = new List<CatWithMerchant>();
				}
			}
			catch (JsonSerializationException ex)
			{
				_logger.LogError(ex, "JSON deserialization error.");
				ViewBag.SubCategoriesWithMerchant = new List<CatWithMerchant>();
				ModelState.AddModelError(string.Empty, "Failed to load categories.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An unexpected error occurred.");
				ViewBag.SubCategoriesWithMerchant = new List<CatWithMerchant>();
				ModelState.AddModelError(string.Empty, "An unexpected error occurred while loading categories.");
			}

			if (TempData.TryGetValue("SuccessMessage", out var saveResponse))
			{
				ViewBag.SaveResponse = saveResponse.ToString();
			}

			return View();
		}
		public async Task<ActionResult> RequestForDiscount(string id)
		{
			_logger.LogInformation("RequestForDiscount method called with id: {Id}", id);
			string userId = HttpContext.Session.GetEncryptedString("UserId", _protector);
			if (string.IsNullOrEmpty(id))
			{
				_logger.LogWarning("RequestForDiscount called with empty id");
				return NotFound();
			}

			try
			{
				var reqIds = id.Split('~');
				var discountRequestClass = new DiscountRequestClass
				{
					MerchantId = Convert.ToInt32(reqIds[0]),
					ServiceId = Convert.ToInt32(reqIds[1]),
					UserId = Convert.ToInt32(HttpContext.Session.GetEncryptedString("UserId", _protector)) //HttpContext.Session.GetString("UserId")
				};

				var Request = await _httpClient.PostAsync("CategoryWithMerchant/sendRequestForDiscount", Customs.GetJsonContent(discountRequestClass));
				var responseString = await Request.Content.ReadAsStringAsync();
				// Trigger notification
				var notification = new Notification
				{
					UserId = HttpContext.Session.GetEncryptedString("UserId", _protector).ToString(),
					Message = $"User[{reqIds[1].ToString()}] has asked for a discount.",
					MerchantId = reqIds[0],
					RedirectToActionUrl = "UserRequestToMerchant/CheckReqest",
					MessageFromId = Convert.ToInt32(HttpContext.Session.GetEncryptedString("UserId", _protector))
				};

				var res = await _httpClient.PostAsync("Notifications/CreateNotification", Customs.GetJsonContent(notification));
				string resString = await res.Content.ReadAsStringAsync();
				_logger.LogInformation("Notification Response : " + resString);
				TempData["SuccessMessage"] = responseString;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while sending discount request.");
				TempData["FailMessage"] = "An error occurred while sending the discount request.";
			}

			return RedirectToAction("SelectedMerchantList");
		}
	}
	public class CatWithMerchant
	{
		public string? MID { get; set; }
		public string? SID { get; set; }
		public string? MERCHANTNAME { get; set; }
		public string? SERVICENAME { get; set; }
		public string? PRICE { get; set; }
		public string? MERCHANTLOCATION { get; set; }
	}

	public class DiscountRequestClass
	{
		public int MerchantId { get; set; }
		public int ServiceId { get; set; }
		public int UserId { get; set; }
	}
}
