using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SCAPI.WebFront.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		// GET: HomeController
		[HttpGet]
		public async Task<ActionResult> Index()
		{
			_logger.LogInformation("Index method called");

			try
			{
				var jsonResponse = await WebApiHelper.GetData("/api/MainPage/GetCategories");

				if (string.IsNullOrEmpty(jsonResponse))
				{
					_logger.LogWarning("Received empty response from API");
					ViewBag.Categories = new List<ServiceCat>();
				}
				else
				{

					// Deserialize JSON string to List<ServiceCategory>
					// Remove outer quotes to get the actual JSON string
					jsonResponse = JsonConvert.DeserializeObject<string>(jsonResponse);
					// Deserialize JSON string to List<ServiceCat>
					List<ServiceCat> categories = JsonConvert.DeserializeObject<List<ServiceCat>>(jsonResponse);
					ViewBag.Categories = categories;
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while fetching categories");
				ViewBag.Categories = new List<ServiceCat>();
				return StatusCode(500, "Internal server error");
			}

			return View();
		}
	}
	public class ServiceCat
	{
		public string? CategoryName { get; set; }
		public int CategoryId { get; set; }
	}
}
