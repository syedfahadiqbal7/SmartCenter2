using AFFZ_Customer.Models;
using AFFZ_Customer.Utils;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using Stripe;
using Stripe.Checkout;
using System.ComponentModel.DataAnnotations;

namespace AFFZ_Customer.Controllers
{
    public class MerchantResponseToUser : Controller
    {
        private readonly ILogger<MerchantResponseToUser> _logger;
        private readonly IWebHostEnvironment _environment;
        private static string _merchantIdCat = string.Empty;
        private readonly HttpClient _httpClient;
        IDataProtector _protector;
        public MerchantResponseToUser(ILogger<MerchantResponseToUser> logger, IWebHostEnvironment environment, IHttpClientFactory httpClientFactory, IDataProtectionProvider provider)
        {

            _httpClient = httpClientFactory.CreateClient("Main");
            _protector = provider.CreateProtector("Example.SessionProtection");
            _logger = logger;
            _environment = environment;
        }

        [HttpGet]
        public async Task<ActionResult> MerchantResponseIndex()
        {
            string userId = HttpContext.Session.GetEncryptedString("UserId", _protector); // Placeholder for session user ID retrieval
            try
            {
                var jsonResponse = await _httpClient.GetAsync($"CategoryWithMerchant/AllResponsesFromMerchant?Uid={userId}");
                jsonResponse.EnsureSuccessStatusCode();
                if (jsonResponse != null)
                {
                    var responseString = await jsonResponse.Content.ReadAsStringAsync();
                    List<RequestForDisCountToUserViewModel> categories = JsonConvert.DeserializeObject<List<RequestForDisCountToUserViewModel>>(responseString);
                    ViewBag.ResponseForDisCountFromMerchant = categories;
                }
                else
                {
                    _logger.LogWarning("Empty response received from API for userId: {UserId}", userId);
                    ViewBag.ResponseForDisCountFromMerchant = new List<RequestForDisCountToUserViewModel>();
                }
            }
            catch (JsonSerializationException ex)
            {
                _logger.LogError(ex, "JSON deserialization error for userId: {UserId}", userId);
                ViewBag.ResponseForDisCountFromMerchant = new List<RequestForDisCountToUserViewModel>();
                ModelState.AddModelError(string.Empty, "Failed to load data.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching data for userId: {UserId}", userId);
                ViewBag.ResponseForDisCountFromMerchant = new List<RequestForDisCountToUserViewModel>();
                ModelState.AddModelError(string.Empty, "An unexpected error occurred while loading data.");
            }

            if (TempData.TryGetValue("SaveResponse", out var saveResponse))
            {
                ViewBag.SaveResponse = saveResponse.ToString();
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> PaymentDone(string amount, string payerId, string merchantId, string serviceId, string rfdfu)
        {
            _logger.LogInformation("PaymentDone called with amount: {Amount}, payerId: {PayerId}, merchantId: {MerchantId}, serviceId: {ServiceId}, rfdfu: {RFDFU}", amount, payerId, merchantId, serviceId, rfdfu);

            if (string.IsNullOrEmpty(amount) || string.IsNullOrEmpty(payerId) || string.IsNullOrEmpty(merchantId) || string.IsNullOrEmpty(serviceId) || string.IsNullOrEmpty(rfdfu))
            {
                _logger.LogWarning("Invalid parameters passed to PaymentDone");
                return BadRequest("Invalid parameters.");
            }
            ProviderUser p = new ProviderUser();
            var getMerchantName = await _httpClient.GetAsync("Providers/" + merchantId);
            getMerchantName.EnsureSuccessStatusCode();
            if (getMerchantName != null)
            {
                var responseString = await getMerchantName.Content.ReadAsStringAsync();
                p = JsonConvert.DeserializeObject<ProviderUser>(responseString);

            }
            Service s = new Service();
            var getServiceName = await _httpClient.GetAsync("CategoryServices/getServiceByName?id=" + serviceId);
            getServiceName.EnsureSuccessStatusCode();
            if (getServiceName != null)
            {
                var responseString = await getServiceName.Content.ReadAsStringAsync();
                // Deserialize the JSON into a List<Service>
                List<Service> services = JsonConvert.DeserializeObject<List<Service>>(responseString);
                s = services[0];

            }
            HttpContext.Session.SetEncryptedString("amount", $"{amount}", _protector);
            HttpContext.Session.SetEncryptedString("paymentType", "Online", _protector);
            HttpContext.Session.SetEncryptedString("payerId", $"{payerId}", _protector);
            HttpContext.Session.SetEncryptedString("merchantId", $"{merchantId}", _protector);
            HttpContext.Session.SetEncryptedString("serviceId", $"{serviceId}", _protector);
            HttpContext.Session.SetEncryptedString("rfdfu", $"{rfdfu}", _protector);

            var paymentGatewayResponse = await PaymentGateway(amount, 1, s.serviceName, p.ProviderName);//stripe
            //var paymentGatewayResponse = await PaymentGatewaytelr(amount, 1, s.serviceName, p.ProviderName);//telr
            return paymentGatewayResponse;

        }
        [HttpGet]
        public async Task<ActionResult> cancel()
        { return View(); }
        [HttpGet]
        public async Task<ActionResult> success()
        {
            try
            {
                string amount = HttpContext.Session.GetEncryptedString("amount", _protector);
                string paymentType = HttpContext.Session.GetEncryptedString("paymentType", _protector);
                string payerId = HttpContext.Session.GetEncryptedString("payerId", _protector);
                string merchantId = HttpContext.Session.GetEncryptedString("merchantId", _protector);
                string serviceId = HttpContext.Session.GetEncryptedString("serviceId", _protector);
                string rfdfu = HttpContext.Session.GetEncryptedString("rfdfu", _protector);

                var paymentHistory = new PaymentHistory
                {
                    PAYMENTTYPE = paymentType,
                    AMOUNT = amount,
                    PAYERID = payerId,
                    MERCHANTID = merchantId,
                    ISPAYMENTSUCCESS = 1,
                    SERVICEID = serviceId,
                    PAYMENTDATETIME = DateTime.Now,
                };

                var responseMessage = await _httpClient.PostAsync("Payment/sendRequestToSavePayment", Customs.GetJsonContent(paymentHistory));
                var responseString = await responseMessage.Content.ReadAsStringAsync();
                if (responseString.Contains("Payment Done Successfully"))
                {
                    var discountUpdateInfo = new RequestForDisCountToUser
                    {
                        RFDFU = Convert.ToInt32(rfdfu),
                        SID = Convert.ToInt32(serviceId),
                        MID = Convert.ToInt32(merchantId),
                        UID = Convert.ToInt32(payerId),
                        ResponseDateTime = DateTime.Now,
                        IsMerchantSelected = 1,
                        FINALPRICE = Convert.ToInt32(amount.Split('.')[0]),
                        IsPaymentDone = 1
                    };

                    responseMessage = await _httpClient.PostAsync("Payment/UpdateRequestForDisCountToUserForPaymentDone", Customs.GetJsonContent(discountUpdateInfo));
                    responseString = await responseMessage.Content.ReadAsStringAsync();
                    // Trigger notification
                    var notification = new Notification
                    {
                        UserId = payerId.ToString(),
                        Message = $"User[{payerId.ToString()}] has payment has been recieved. Please continue to Apply for the service requested.",
                        MerchantId = merchantId,
                        RedirectToActionUrl = "",
                        MessageFromId = Convert.ToInt32(payerId)
                    };

                    var res = await _httpClient.PostAsync("Notifications/CreateNotification", Customs.GetJsonContent(notification));
                    string resString = await res.Content.ReadAsStringAsync();
                    _logger.LogInformation("Notification Response : " + resString);
                }

                TempData["SuccessResponse"] = responseString;
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the payment.");
                return StatusCode(500, "An internal server error occurred. Please try again later.");
            }
        }
        [HttpGet]
        public async Task<ActionResult> Payment(string rfdfu, string uid, string merchantId)
        {
            string userId = HttpContext.Session.GetEncryptedString("UserId", _protector); // Placeholder for session user ID retrieval

            try
            {
                var jsonResponse = await _httpClient.GetAsync($"CategoryWithMerchant/AllResponsesFromMerchant?Uid={userId}");
                string responseString = await jsonResponse.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(responseString))
                {
                    int selectedServiceId = Convert.ToInt32(rfdfu);
                    List<RequestForDisCountToUserViewModel> categories = JsonConvert.DeserializeObject<List<RequestForDisCountToUserViewModel>>(responseString);
                    ViewBag.ResponseForDisCountFromMerchant = categories.FirstOrDefault(x => x.RFDFU == selectedServiceId);
                }
                else
                {
                    _logger.LogWarning("Empty response received from API for userId: {UserId}", userId);
                    ViewBag.ResponseForDisCountFromMerchant = new List<RequestForDisCountToUserViewModel>();
                }
            }
            catch (JsonSerializationException ex)
            {
                _logger.LogError(ex, "JSON deserialization error for userId: {UserId}", userId);
                ViewBag.ResponseForDisCountFromMerchant = new List<RequestForDisCountToUserViewModel>();
                ModelState.AddModelError(string.Empty, "Failed to load data.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching data for userId: {UserId}", userId);
                ViewBag.ResponseForDisCountFromMerchant = new List<RequestForDisCountToUserViewModel>();
                ModelState.AddModelError(string.Empty, "An unexpected error occurred while loading data.");
            }

            if (TempData.TryGetValue("SaveResponse", out var saveResponse))
            {
                ViewBag.SaveResponse = saveResponse.ToString();
            }

            return View();
        }
        private async Task<ActionResult> PaymentGatewaytelr(string _price, long _quantity, string servicetype, string merchantname)
        {

            var domain = "https://localhost:7195/MerchantResponseToUser/";
            string SuccessUrl = domain + "success";
            string CancelUrl = domain + "cancel";

            var optionstelr = new RestClientOptions("https://secure.telr.com/gateway/order.json");
            var client = new RestClient(optionstelr);
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            request.AddJsonBody("{\"method\":\"create\",\"store\":1234,\"authkey\":\"mykey1234\",\"framed\":0,\"order\":{\"cartid\":\"1234\",\"test\":\"1\",\"amount\":\"" + _price + "\",\"currency\":\"AED\",\"description\":\"1 month Visa\"},\"return\":{\"authorised\":\"" + SuccessUrl + "\",\"declined\":\"" + CancelUrl + "\",\"cancelled\":\"" + CancelUrl + "\"}}", false);
            var response = await client.PostAsync(request);

            return Ok(response.Content);
        }
        private async Task<ActionResult> PaymentGateway(string _price, long _quantity, string servicetype, string merchantname)
        {

            StripeConfiguration.ApiKey = "sk_test_51Q0Hq0KyRuGjwLZlAu0o3PzTHoVHZhb9HZwJuvGFl7CLsa1NI3xgPJwYKITHYYVbQKVHVv2h85E1b7YBB2OTa5bF00k0mbfMR3";
            var domain = "https://localhost:7195/MerchantResponseToUser/";
            var optionsProduct = new ProductCreateOptions
            {
                Name = "1 month Visa",
                Description = "1 month Visa Purchase from Merchant",
            };
            var serviceProduct = new ProductService();
            Product product = serviceProduct.Create(optionsProduct);
            // Console.Write("Success! Here is your starter subscription product id: {0}\n", product.Id);
            double priceValue = Convert.ToDouble(_price);

            // Multiply by 100 and then convert to long
            long finalPrice = Convert.ToInt64(priceValue * 100);
            var optionsPrice = new PriceCreateOptions
            {
                UnitAmount = finalPrice,
                Currency = "aed",
                //Recurring = new PriceRecurringOptions
                //{
                //    Interval = "one-time",
                //},
                Product = product.Id
            };
            var servicePrice = new PriceService();
            Price price = servicePrice.Create(optionsPrice);
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    // Provide the exact Price ID (for example, pr_1234) of the product you want to sell
                    Price = price.Id,
                    Quantity = 1,
                  },
                },
                Mode = "payment",
                SuccessUrl = domain + "success",
                CancelUrl = domain + "cancel",


            };
            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
        public async Task<ActionResult> SelectFinalMerchant(RequestForDisCountToUserViewModel requestForDisCount)
        {
            if (requestForDisCount == null || requestForDisCount.RFDFU == 0)
            {
                _logger.LogWarning("Invalid request data passed to SelectFinalMerchant");
                return BadRequest("Invalid request data.");
            }

            _logger.LogInformation("SelectFinalMerchant called with request data: {RequestData}", requestForDisCount);

            try
            {
                var srbm = new RequestForDisCountToUser
                {
                    RFDFU = requestForDisCount.RFDFU,
                    UID = requestForDisCount.UID,
                    MID = requestForDisCount.MerchantID,
                    IsMerchantSelected = 1,
                    IsPaymentDone = 0
                };

                var responseMessage = await _httpClient.PostAsync("CategoryWithMerchant/SelectFinalMerchant", Customs.GetJsonContent(srbm));
                string responseString = await responseMessage.Content.ReadAsStringAsync();
                TempData["SaveResponse"] = responseString;
                // Trigger notification
                var notification = new Notification
                {
                    UserId = requestForDisCount.UID.ToString(),
                    Message = $"Congratulations. User[{requestForDisCount.UID.ToString()}] has selected you as a final merchant among 500 others. ",
                    MerchantId = requestForDisCount.MerchantID.ToString(),
                    RedirectToActionUrl = "#",
                    MessageFromId = Convert.ToInt32(requestForDisCount.UID)
                };

                var res = await _httpClient.PostAsync("Notifications/CreateNotification", Customs.GetJsonContent(notification));
                string resString = await res.Content.ReadAsStringAsync();
                _logger.LogInformation("Notification Response : " + resString);
                return RedirectToAction("MerchantResponseIndex");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while selecting the final merchant.");
                return StatusCode(500, "An internal server error occurred. Please try again later.");
            }
        }
        public async Task<ActionResult> DeSelectFinalMerchant(RequestForDisCountToUserViewModel requestForDisCount)
        {
            if (requestForDisCount == null || requestForDisCount.RFDFU == 0)
            {
                _logger.LogWarning("Invalid request data passed to DeSelectFinalMerchant");
                return BadRequest("Invalid request data.");
            }

            _logger.LogInformation("DeSelectFinalMerchant called with request data: {RequestData}", requestForDisCount);

            try
            {
                var srbm = new RequestForDisCountToUser
                {
                    RFDFU = requestForDisCount.RFDFU,
                    UID = requestForDisCount.UID,
                    MID = requestForDisCount.MerchantID,
                    IsMerchantSelected = 0
                };

                var responseMessage = await _httpClient.PostAsync("CategoryWithMerchant/DeSelectFinalMerchant", Customs.GetJsonContent(srbm));
                string responseString = await responseMessage.Content.ReadAsStringAsync();
                TempData["SaveResponse"] = responseString;
                // Trigger notification
                var notification = new Notification
                {
                    UserId = requestForDisCount.UID.ToString(),
                    Message = $"User[{requestForDisCount.UID.ToString()}] has decided not to move with you.",
                    MerchantId = requestForDisCount.MerchantID.ToString(),
                    RedirectToActionUrl = "#",
                    MessageFromId = Convert.ToInt32(requestForDisCount.UID)
                };

                var res = await _httpClient.PostAsync("Notifications/CreateNotification", Customs.GetJsonContent(notification));
                string resString = await res.Content.ReadAsStringAsync();
                _logger.LogInformation("Notification Response : " + resString);
                return RedirectToAction("MerchantResponseIndex");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deselecting the final merchant.");
                return StatusCode(500, "An internal server error occurred. Please try again later.");
            }
        }
        //CheckDocumentStatus
        [HttpGet]
        public async Task<ActionResult> CheckDocumentStatus()
        {
            try
            {
                int UserId = Convert.ToInt32(HttpContext.Session.GetEncryptedString("UserId", _protector));//Convert.ToInt32(HttpContext.Session.GetString("UserId"));

                var jsonResponse = await _httpClient.GetAsync("FileUpload/GetFilesList");
                string responseString = await jsonResponse.Content.ReadAsStringAsync();
                FileUploadViewModel model = new FileUploadViewModel();

                if (!string.IsNullOrEmpty(responseString))
                {
                    List<UploadedFile> documentList = JsonConvert.DeserializeObject<List<UploadedFile>>(responseString);
                    model.UploadedFiles = documentList.Where(x => x.UserId == UserId).ToList();
                }

                ViewBag.SaveResponse = model;
                return View(model);
            }
            catch (JsonSerializationException ex)
            {
                _logger.LogError(ex, "JSON deserialization error while fetching file list.");
                ModelState.AddModelError(string.Empty, "Failed to load data.");
                return View(new FileUploadViewModel());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching file list.");
                ModelState.AddModelError(string.Empty, "An unexpected error occurred while loading data.");
                return View(new FileUploadViewModel());
            }
        }
        [HttpGet]
        public async Task<ActionResult> UploadDocuments(string MerchantID)
        {
            try
            {
                int UserId = Convert.ToInt32(HttpContext.Session.GetEncryptedString("UserId", _protector));//Convert.ToInt32(HttpContext.Session.GetString("UserId"));

                var jsonResponse = await _httpClient.GetAsync("FileUpload/GetFilesList");
                string responseString = await jsonResponse.Content.ReadAsStringAsync();
                FileUploadViewModel model = new FileUploadViewModel();

                if (!string.IsNullOrEmpty(responseString))
                {
                    List<UploadedFile> documentList = JsonConvert.DeserializeObject<List<UploadedFile>>(responseString);
                    model.UploadedFiles = documentList.Where(x => x.UserId == UserId).ToList();
                }
                ViewBag.MerchantID = MerchantID;
                ViewBag.SaveResponse = model;
                //// Trigger notification
                //var notification = new Notification
                //{
                //    UserId = UserId.ToString(),
                //    Message = $"User[{UserId.ToString()}] has uploaded Documents. Please Check.",
                //    MerchantId = MerchantID.ToString(),
                //    RedirectToActionUrl = "#",
                //    MessageFromId = UserId
                //};

                //var res = await _httpClient.PostAsync("Notifications/CreateNotification", Customs.GetJsonContent(notification));
                //string resString = await res.Content.ReadAsStringAsync();
                //_logger.LogInformation("Notification Response : " + resString);
                return View(model);
            }
            catch (JsonSerializationException ex)
            {
                _logger.LogError(ex, "JSON deserialization error while fetching file list.");
                ModelState.AddModelError(string.Empty, "Failed to load data.");
                return View(new FileUploadViewModel());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching file list.");
                ModelState.AddModelError(string.Empty, "An unexpected error occurred while loading data.");
                return View(new FileUploadViewModel());
            }
        }

        [HttpGet]
        public async Task<ActionResult> ReviewDocument(int userId)
        {
            try
            {
                var jsonResponse = await _httpClient.GetAsync($"FileUpload/ReviewDocuments/{userId}");
                string responseString = await jsonResponse.Content.ReadAsStringAsync();
                FileUploadViewModel model = new FileUploadViewModel();

                if (!string.IsNullOrEmpty(responseString))
                {
                    List<UploadedFile> documentList = JsonConvert.DeserializeObject<List<UploadedFile>>(responseString);
                    model.UploadedFiles = documentList.Where(x => x.UserId == 1).ToList();
                }
                ViewBag.UserId = userId;
                ViewBag.SaveResponse = model;
                return View(model);
            }
            catch (JsonSerializationException ex)
            {
                _logger.LogError(ex, "JSON deserialization error while reviewing document for userId: {UserId}", userId);
                ModelState.AddModelError(string.Empty, "Failed to load data.");
                return View(new FileUploadViewModel());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while reviewing document for userId: {UserId}", userId);
                ModelState.AddModelError(string.Empty, "An unexpected error occurred while loading data.");
                return View(new FileUploadViewModel());
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadDocuments(FileUploadViewModel model)
        {
            if (model == null || model.UserDocuments == null || model.UserDocuments.Count == 0)
            {
                _logger.LogWarning("Invalid model passed to UploadDocuments");
                return BadRequest("Invalid model.");
            }

            _logger.LogInformation("UploadDocuments called with model: {Model}", model);

            try
            {
                if (ModelState.IsValid)
                {
                    int loginUserId = Convert.ToInt32(HttpContext.Session.GetEncryptedString("UserId", _protector)); // Placeholder for session user ID retrieval
                    var username = $"Documents_{loginUserId}";

                    var folderPath = Path.Combine(_environment.WebRootPath, "uploads", username);
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    var fileUploadModelAPI = new FileUploadModelAPI
                    {
                        UserId = loginUserId,
                        UploadedFiles = new List<UploadedFile>()
                    };

                    foreach (var file in model.UserDocuments)
                    {
                        if (file.Length > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            var filePath = Path.Combine(folderPath, fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            var uploadedFile = new UploadedFile
                            {
                                FileName = fileName,
                                ContentType = file.ContentType,
                                FileSize = file.Length,
                                FolderName = username,
                                Status = "Pending",
                                UserId = loginUserId,
                                MerchantId = model.Merchant
                            };

                            fileUploadModelAPI.UploadedFiles.Add(uploadedFile);
                        }
                    }

                    var responseMessage = await _httpClient.PostAsync("FileUpload/UploadFiles", Customs.GetJsonContent(fileUploadModelAPI));
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    TempData["SuccessResponse"] = responseString;
                    // Trigger notification
                    var notification = new Notification
                    {
                        UserId = loginUserId.ToString(),
                        Message = $"User[{loginUserId.ToString()}] has upload some documents.",
                        MerchantId = model.Merchant.ToString(),
                        RedirectToActionUrl = "MerchantResponseToUser/GetUsersWithDocuments",
                        MessageFromId = Convert.ToInt32(loginUserId)
                    };

                    var res = await _httpClient.PostAsync("Notifications/CreateNotification", Customs.GetJsonContent(notification));
                    string resString = await res.Content.ReadAsStringAsync();
                    _logger.LogInformation("Notification Response : " + resString);
                    return RedirectToAction("UploadDocuments");
                }

                return RedirectToAction("UploadDocuments");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while uploading documents.");
                return StatusCode(500, "An error occurred. Please try again later.");

            }
        }

        [HttpGet]
        public IActionResult DownloadFile(string fileName, string folderName)
        {
            _logger.LogInformation("DownloadFile called with fileName: {FileName}, folderName: {FolderName}", fileName, folderName);

            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName))
            {
                _logger.LogWarning("Invalid parameters passed to DownloadFile");
                return BadRequest("Invalid parameters.");
            }

            var filePath = Path.Combine(_environment.WebRootPath, "uploads", folderName, fileName);
            if (System.IO.File.Exists(filePath))
            {
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, "application/octet-stream", fileName);
            }
            else
            {
                _logger.LogWarning("File not found: {FilePath}", filePath);
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersWithDocuments()
        {
            int merchantId = 1; // Placeholder for session merchant ID retrieval

            try
            {
                var jsonResponse = await _httpClient.GetAsync("FileUpload/UsersWithDocuments");
                string responseString = await jsonResponse.Content.ReadAsStringAsync();
                UserDocumentsViewModel model = new UserDocumentsViewModel();

                if (!string.IsNullOrEmpty(responseString))
                {
                    List<UserDocumentsViewModel> documentList = JsonConvert.DeserializeObject<List<UserDocumentsViewModel>>(responseString);
                    ViewBag.UsersWithDocuments = documentList.Where(x => x.MID == merchantId).ToList();
                }

                return View();
            }
            catch (JsonSerializationException ex)
            {
                _logger.LogError(ex, "JSON deserialization error while fetching users with documents.");
                ModelState.AddModelError(string.Empty, "Failed to load data.");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching users with documents.");
                ModelState.AddModelError(string.Empty, "An unexpected error occurred while loading data.");
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> VerifyDocument(string documentId, string userId)
        {
            _logger.LogInformation("VerifyDocument method called for DocumentId: {DocumentId}", documentId);
            try
            {
                var responseMessage = await _httpClient.PostAsync($"FileUpload/VerifyDocument/{documentId}", Customs.GetJsonContent(documentId));
                string responseString = await responseMessage.Content.ReadAsStringAsync();

                _logger.LogInformation($"Document verification response for DocumentId: {documentId}. Response : {responseString}");
                TempData["SuccessMessage"] = responseString;
                ViewBag.SaveResponse = responseString;

                return RedirectToAction("ReviewDocument", new { userId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while verifying document for DocumentId: {DocumentId}", documentId);
                return StatusCode(500, "Internal server error.");
            }
        }
        [HttpGet]
        public async Task<IActionResult> DeleteDocument(string documentId)
        {
            _logger.LogInformation("DeleteDocument method called for DocumentId: {DocumentId}", documentId);
            try
            {
                var responseMessage = await _httpClient.PostAsync($"FileUpload/DeleteDocument/{documentId}", Customs.GetJsonContent(documentId));
                string responseString = await responseMessage.Content.ReadAsStringAsync();
                _logger.LogInformation($"Document Deletion response for DocumentId: {documentId}. Response : {responseString}");
                //TempData["SaveResponse"] = responseMessage;
                ViewBag.SaveResponse = responseString;
                return RedirectToAction("UploadDocuments");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while verifying document for DocumentId: {DocumentId}", documentId);
                return StatusCode(500, "Internal server error.");
            }
        }

    }
    public class RequestForDisCountToUserViewModel
    {
        public int SID { get; set; }
        public int ServicePrice { get; set; }
        public string ServiceName { get; set; }
        public int MerchantID { get; set; }
        public decimal FINALPRICE { get; set; }
        public int UID { get; set; }
        public int RFDFU { get; set; }
        public int IsMerchantSelected { get; set; }
        public int IsPaymentDone { get; set; }
        public DateTime ResponseDateTime { get; set; }
    }
    public class FileUploadViewModel
    {
        [Required]
        public IFormFileCollection UserDocuments { get; set; }
        public int UserId { get; set; }
        public int Merchant { get; set; }
        public List<UploadedFile>? UploadedFiles { get; set; }
    }
    public class UserDocumentsViewModel
    {
        public int UserId { get; set; }
        public int MID { get; set; }
        public int DocumentCount { get; set; }
    }
    public class FileUploadModelAPI
    {

        public int UserId { get; set; }
        public List<UploadedFile>? UploadedFiles { get; set; }
    }
}
