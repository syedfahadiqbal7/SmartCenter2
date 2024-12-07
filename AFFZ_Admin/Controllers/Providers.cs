using AFFZ_Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace AFFZ_Admin.Controllers
{
    public class Providers : Controller
    {
        private readonly HttpClient _httpClient;

        public Providers(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Main");
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync("Providers");
                var responseString = await response.Content.ReadAsStringAsync();
                var providers = JsonConvert.DeserializeObject<List<ProviderUser>>(responseString);

                return View("Providers", providers);
            }
            catch (Exception ex)
            {
                return View("Dashboard");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMerchantDocuments(int providerId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Providers/GetMerchantDocs/{providerId}");
                var responseString = await response.Content.ReadAsStringAsync();
                var documents = JsonConvert.DeserializeObject<List<MerchantDocuments>>(responseString);

                return Json(documents);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error fetching documents.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDocumentStatus([FromBody] DocumentStatusUpdateModel model)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("Providers/UpdateDocumentStatus", content);

                if (response.IsSuccessStatusCode)
                {
                    return Ok("Document status updated.");
                }

                return StatusCode(500, "Failed to update document status.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error updating document status.");
            }
        }
        [HttpGet]
        public IActionResult DownloadFile(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    return BadRequest("File path is required.");
                }

                // Decode the file path from the URL
                filePath = Uri.UnescapeDataString(filePath);

                // Ensure the file path uses the correct directory separator
                filePath = filePath.Replace('/', Path.DirectorySeparatorChar);

                // Combine the file path with the root directory of uploads
                var uploadsRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads").Replace("AFFZ_Admin", "AFFZ_Provider");
                var fullPath = Path.Combine(uploadsRoot, Path.GetFileName(filePath));

                // Validate that the file exists within the uploads directory
                if (!fullPath.StartsWith(uploadsRoot, StringComparison.OrdinalIgnoreCase) || !System.IO.File.Exists(fullPath))
                {
                    return NotFound("File not found or invalid path.");
                }

                // Get the file content and file name
                var fileContent = System.IO.File.ReadAllBytes(fullPath);
                var fileName = Path.GetFileName(fullPath);

                // Return the file with explicit Content-Disposition header
                return File(fileContent, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                // Log the exception (if needed) and return an error response
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        public async Task<IActionResult> ToggleStatus(int providerId, bool isActive)
        {
            try
            {
                var response = await _httpClient.PostAsync("Providers/ToggleMerchantStatus",
                    new StringContent(JsonConvert.SerializeObject(new { ProviderId = providerId, IsActive = isActive }), Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true });
                }

                return Json(new { success = false, error = await response.Content.ReadAsStringAsync() });
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error toggling merchant status for Provider ID: {ProviderId}", providerId);
                return Json(new { success = false, error = ex.Message });
            }
        }


    }

    public class DocumentStatusUpdateModel
    {
        public int Id { get; set; }
        public string Status { get; set; }
    }
}
