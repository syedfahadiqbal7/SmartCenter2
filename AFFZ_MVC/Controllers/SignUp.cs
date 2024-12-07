using AFFZ_Customer.Models;
using AFFZ_Customer.Models.Partial;
using AFFZ_Customer.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AFFZ_Customer.Controllers
{
    public class SignUp : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<SignUp> _logger;

        // Constructor to initialize HttpClient and Logger
        public SignUp(IHttpClientFactory httpClientFactory, ILogger<SignUp> logger)
        {
            _httpClient = httpClientFactory.CreateClient("Main");
            _logger = logger;
            _logger.LogDebug("SignUp controller initialized.");
        }

        // GET: SignUp page
        public IActionResult Index()
        {
            _logger.LogDebug("SignUp page requested.");
            return View("SignUp", new Customers()); // Render the SignUp view with an empty Customers model
        }

        // POST: Register a new customer
        [HttpPost]
        public async Task<IActionResult> CustomersRegister(Customers model)
        {
            _logger.LogDebug("CustomersRegister action called with model: {@Model}", model);

            // Validate the model state
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid registration model state.");
                // Log each validation error for debugging purposes
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning("Model validation error: {ErrorMessage}", error.ErrorMessage);
                }
                return View("SignUp", model); // Return the SignUp view with the current model to display validation errors
            }

            try
            {
                _logger.LogDebug("Sending registration request to API with referral code: {ReferrerCode}", model.ReferrerCode);
                // Send a POST request to the API to register the customer
                var response = await _httpClient.PostAsync($"Customers/Register?referralCode={model.ReferrerCode}", Customs.GetJsonContent(model));
                _logger.LogDebug("Received response with status code: {StatusCode}", response.StatusCode);

                // Ensure the response indicates success
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Registration request failed with status code: {StatusCode}", response.StatusCode);
                    ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
                    return View("SignUp", model); // Return the SignUp view with an error message
                }

                var responseString = await response.Content.ReadAsStringAsync();
                _logger.LogDebug("Received response content: {ResponseString}", responseString);

                // Parse the response to check for any additional errors
                var sResponse = JsonConvert.DeserializeObject<SResponse>(responseString);
                if (sResponse == null || sResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    _logger.LogWarning("Registration failed. StatusCode: {StatusCode}, Data: {Data}", sResponse?.StatusCode, sResponse?.Data);
                    ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
                    return View("SignUp", model); // Return the SignUp view with an error message
                }

                _logger.LogInformation("User registered successfully with customer ID: {CustomerId}", sResponse.Data);
                ViewData["Message"] = "User Registered!";
                return RedirectToAction("Index", "Login"); // Redirect to the login page after successful registration
            }
            catch (HttpRequestException httpEx)
            {
                // Handle HTTP request errors (e.g., network issues)
                _logger.LogError(httpEx, "HTTP request failed during customer registration.");
                ModelState.AddModelError(string.Empty, "Unable to connect to the server. Please try again later.");
                return View("SignUp", model); // Return the SignUp view with an error message
            }
            catch (Exception ex)
            {
                // Handle any other unexpected errors
                _logger.LogError(ex, "An unexpected error occurred during customer registration.");
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again.");
                return View("SignUp", model); // Return the SignUp view with an error message
            }
        }
    }
}
