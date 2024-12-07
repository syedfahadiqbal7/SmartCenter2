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
        private readonly ILogger<Login> _logger;

        public Login(IHttpClientFactory httpClientFactory, IDataProtectionProvider provider, IWebHostEnvironment environment, ILogger<Login> logger)
        {
            _httpClient = httpClientFactory.CreateClient("Main");
            _protector = provider.CreateProtector("Example.SessionProtection");
            _environment = environment;
            _logger = logger;
        }

        /// <summary>
        /// Displays the login page.
        /// </summary>
        /// <returns>Login view.</returns>
        public IActionResult Index()
        {
            return View("Login", new LoginModel());
        }

        /// <summary>
        /// Handles the provider login process.
        /// </summary>
        /// <param name="model">The login model containing user credentials.</param>
        /// <returns>Redirects to dashboard on success, or back to login on failure.</returns>
        [HttpPost]
        public async Task<IActionResult> ProviderLogin(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                // Log model state errors
                _logger.LogWarning("Invalid login model state.");
                return View("Login", model);
            }

            try
            {
                // Validate input variables
                if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
                {
                    ModelState.AddModelError(string.Empty, "Email and Password are required.");
                    _logger.LogWarning("Email or Password is null or empty.");
                    return View("Login", model);
                }

                // Prepare the HTTP request
                var content = Customs.GetJsonContent(model);
                var response = await _httpClient.PostAsync("Providers/Login", content);

                // Ensure the response is successful
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Login failed with status code: {StatusCode}", response.StatusCode);
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View("Login", model);
                }

                var responseString = await response.Content.ReadAsStringAsync();

                // Deserialize the response
                var sResponse = JsonConvert.DeserializeObject<SResponse>(responseString);

                if (sResponse == null)
                {
                    _logger.LogError("Failed to deserialize response.");
                    ModelState.AddModelError(string.Empty, "An error occurred during login.");
                    return View("Login", model);
                }

                if (sResponse.StatusCode == HttpStatusCode.OK)
                {
                    var providerDetail = JsonConvert.DeserializeObject<ProviderUser>(sResponse.Data.ToString());

                    if (providerDetail == null)
                    {
                        _logger.LogError("Provider detail is null.");
                        ModelState.AddModelError(string.Empty, "An error occurred during login.");
                        return View("Login", model);
                    }

                    // Store user information in session securely
                    HttpContext.Session.SetEncryptedString("ProviderId", providerDetail.ProviderId.ToString(), _protector);
                    HttpContext.Session.SetEncryptedString("ProviderName", providerDetail.ProviderName, _protector);
                    HttpContext.Session.SetEncryptedString("Email", providerDetail.Email, _protector);
                    HttpContext.Session.SetEncryptedString("RoleId", providerDetail.RoleId.ToString(), _protector);
                    HttpContext.Session.SetEncryptedString("IsActive", providerDetail.IsActive.ToString(), _protector);
                    HttpContext.Session.SetEncryptedString("ProfilePicturePath", providerDetail.ProfilePicture == null ? "" : providerDetail.ProfilePicture, _protector);

                    _logger.LogInformation("User {Email} logged in successfully.", providerDetail.Email);

                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    // Handle specific status codes or messages
                    _logger.LogWarning("Login failed: {Message}", sResponse.Message);
                    ModelState.AddModelError(string.Empty, sResponse.Message ?? "Invalid login attempt.");
                    return View("Login", model);
                }
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "HTTP request error during login.");
                ModelState.AddModelError(string.Empty, "An error occurred while connecting to the server.");
                return View("Login", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during login.");
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                return View("Login", model);
            }
        }

        /// <summary>
        /// Logs out the current user by clearing the session.
        /// </summary>
        /// <returns>Redirects to the login page.</returns>
        public IActionResult Logout()
        {
            try
            {
                HttpContext.Session.Clear();
                _logger.LogInformation("User logged out successfully.");
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during logout.");
                ModelState.AddModelError(string.Empty, "An error occurred during logout.");
                return RedirectToAction("Index", "Login");
            }
        }

        /// <summary>
        /// Retrieves the profile picture of the logged-in user.
        /// </summary>
        /// <returns>The profile picture file or null if not found.</returns>
        public IActionResult GetImage()
        {
            try
            {
                string profilePicturePath = HttpContext.Session.GetEncryptedString("ProfilePicturePath", _protector);

                if (string.IsNullOrEmpty(profilePicturePath))
                {
                    _logger.LogWarning("Profile picture path is null or empty.");
                    return NotFound();
                }

                var providerId = HttpContext.Session.GetEncryptedString("ProviderId", _protector);
                var uploadPath = Path.Combine(_environment.WebRootPath, $"User_{providerId}");
                var filePath = Path.Combine(uploadPath, profilePicturePath);

                if (System.IO.File.Exists(filePath))
                {
                    var imageBytes = System.IO.File.ReadAllBytes(filePath);
                    var contentType = GetContentType(profilePicturePath);
                    return File(imageBytes, contentType);
                }
                else
                {
                    _logger.LogWarning("Profile picture not found at path: {FilePath}", filePath);
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the profile picture.");
                return StatusCode(500, "An error occurred while retrieving the profile picture.");
            }
        }

        /// <summary>
        /// Determines the content type based on the file extension.
        /// </summary>
        /// <param name="fileName">The file name or path.</param>
        /// <returns>The content type string.</returns>
        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".png" => "image/png",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                _ => "application/octet-stream",
            };
        }
    }
}
