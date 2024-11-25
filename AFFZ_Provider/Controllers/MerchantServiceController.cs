using AFFZ_Provider.Models;
using AFFZ_Provider.Utils;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace AFFZ_Provider.Controllers
{
    public class MerchantServiceController : Controller
    {
        private readonly ILogger<MerchantServiceController> _logger;
        private readonly IWebHostEnvironment _environment;
        private static string _merchantIdCat = string.Empty;
        private readonly HttpClient _httpClient;
        private readonly IDataProtector _protector;

        public MerchantServiceController(ILogger<MerchantServiceController> logger, IWebHostEnvironment environment, IHttpClientFactory httpClientFactory, IDataProtectionProvider provider)
        {
            _httpClient = httpClientFactory.CreateClient("Main");
            _protector = provider.CreateProtector("Example.SessionProtection");
            _logger = logger;
            _environment = environment;
        }
        public async Task<IActionResult> MerchantServiceIndex(int pageNumber = 1, int pageSize = 10)
        {
            string _merchantId = HttpContext.Session.GetEncryptedString("ProviderId", _protector);

            if (string.IsNullOrEmpty(_merchantId))
            {
                _logger.LogWarning("Merchant ID not found in session.");
                return Unauthorized();
            }

            int id = Convert.ToInt32(_merchantId);
            try
            {
                var jsonResponse = await _httpClient.GetAsync($"Service/GetAllServices?pageNumber={pageNumber}&pageSize={pageSize}&merchantId={id}");

                if (!jsonResponse.IsSuccessStatusCode)
                {
                    _logger.LogError("Error fetching services. Status Code: {StatusCode}", jsonResponse.StatusCode);
                    ModelState.AddModelError(string.Empty, "Failed to load services.");
                    return View(new List<Service>());
                }

                var responseString = await jsonResponse.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(responseString))
                {
                    var categories = JsonConvert.DeserializeObject<List<Service>>(responseString);
                    ViewBag.ServiceList = categories.Where(x => x.MerchantID == id).ToList();

                    if (jsonResponse.Headers.TryGetValues("X-Total-Count", out var totalCountValues))
                    {
                        ViewBag.TotalCount = int.Parse(totalCountValues.FirstOrDefault());
                    }

                    ViewBag.PageNumber = pageNumber;
                    ViewBag.PageSize = pageSize;
                }
                else
                {
                    _logger.LogWarning("No response data received.");
                    ViewBag.ServiceList = new List<Service>();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error during HTTP request.");
                ModelState.AddModelError(string.Empty, "Error fetching data from the server.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred.");
                ModelState.AddModelError(string.Empty, "An unexpected error occurred.");
            }

            if (TempData.ContainsKey("SuccessMessage") && !string.IsNullOrEmpty(TempData["SuccessMessage"].ToString()))
            {
                ViewBag.SaveResponse = TempData["SuccessMessage"].ToString();
            }

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ViewService(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid service ID: {Id}", id);
                return BadRequest("Invalid service ID.");
            }

            try
            {
                var response = await _httpClient.GetAsync($"Service/GetServiceById?id={id}");
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to fetch service. Status code: {StatusCode}", response.StatusCode);
                    return PartialView("_ServiceDetails", new Service());
                }

                var service = JsonConvert.DeserializeObject<Service>(await response.Content.ReadAsStringAsync());
                return PartialView("_ServiceDetails", service);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching service details for ID {Id}", id);
                ModelState.AddModelError(string.Empty, "Failed to load data.");
                return PartialView("_ServiceDetails", new Service());
            }
        }
        // GET: Service/Create
        public async Task<IActionResult> MerchantServiceCreate(string ServiceName = "", int CategoryID = 0)
        {
            try
            {
                ViewBag.ServiceListComboData = await ServiceListItems(ServiceName);
                ViewBag.ServiceCategoryComboData = await ServiceCategoryItems(CategoryID);
                List<int> defaultDocsList = new List<int>();
                if (!string.IsNullOrEmpty(ServiceName) && ServiceName != "")
                {
                    int sid = await GetServiceId(ServiceName);
                    defaultDocsList = await GetDefaultServiceDocumentListData(sid);
                }
                //ViewBag["defaultDocumentDAta"] = GetDefaultServiceDocumentListData(int.Parse())
                ViewBag.DocumentListComboData = await DocumentListItems(defaultDocsList);

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing service creation page.");
                ModelState.AddModelError(string.Empty, "Failed to load data.");
                return View();
            }

        }

        public async Task<List<SelectListItem>> DocumentListItems(List<int> selectedDocumentIds)
        {
            var jsonResponse = await _httpClient.GetAsync("ServiceDocumenList/GetAllDocuments");
            var responseString = await jsonResponse.Content.ReadAsStringAsync();

            // Initialize an empty list for the documents
            var documentList = new List<SelectListItem>();

            if (!string.IsNullOrEmpty(responseString))
            {
                try
                {
                    // Deserialize the JSON response into a list of documents
                    List<ServiceDocumenList> documents = JsonConvert.DeserializeObject<List<ServiceDocumenList>>(responseString);

                    // Map each document to a SelectListItem
                    documentList = documents.Select(doc => new SelectListItem
                    {
                        Text = doc.ServiceDocumentName,
                        Value = doc.ServiceDocumenListtId.ToString(),
                        Selected = selectedDocumentIds.Contains(doc.ServiceDocumenListtId) // Set selected based on IDs
                    }).ToList();
                }
                catch (JsonSerializationException ex)
                {
                    // Log any deserialization errors
                    _logger.LogError(ex, "JSON deserialization error.");
                }
            }

            // Return the list of SelectListItem objects
            return documentList;
        }
        // POST: Service/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MerchantServiceCreate([Bind("CategoryID,ServiceName,Description,ServicePrice,ServiceAmountPaidToAdmin, SelectedDocumentIds")] Service service, List<int> selectedDocumentIds)
        {
            try
            {
                var merchantId = HttpContext.Session.GetEncryptedString("ProviderId", _protector);
                if (merchantId == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                service.MerchantID = Convert.ToInt32(merchantId);
                service.SelectedDocumentIds = String.Join(",", selectedDocumentIds.Select(i => i.ToString()).ToArray());
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid service model.");
                    return View(service);
                }

                // Create the service
                var jsonResponse = await _httpClient.PostAsync("Service/CreateService", Customs.GetJsonContent(service));
                if (!jsonResponse.IsSuccessStatusCode)
                {
                    _logger.LogError("Service creation failed. Status code: {StatusCode}", jsonResponse.StatusCode);
                    ModelState.AddModelError(string.Empty, await jsonResponse.Content.ReadAsStringAsync());
                    ViewBag.ServiceListComboData = await ServiceListItems("");
                    ViewBag.ServiceCategoryComboData = await ServiceCategoryItems(0);
                    List<int> defaultDocsList = new List<int>();

                    //ViewBag["defaultDocumentDAta"] = GetDefaultServiceDocumentListData(int.Parse())
                    ViewBag.DocumentListComboData = await DocumentListItems(defaultDocsList);
                    return View(service);
                }

                var responseString = await jsonResponse.Content.ReadAsStringAsync();
                var createdService = JsonConvert.DeserializeObject<dynamic>(responseString);
                service.ServiceId = (int)createdService["serviceId"];

                // Save document mappings
                if (selectedDocumentIds != null && selectedDocumentIds.Count > 0)
                {
                    List<ServiceDocumentMapping> mappings = selectedDocumentIds.Select(docId => new ServiceDocumentMapping
                    {
                        ServiceID = service.ServiceId,
                        ServiceDocumentListId = docId
                    }).ToList();

                    var mappingsResponse = await _httpClient.PostAsync("ServiceDocumentMapping/CreateMapping", Customs.GetJsonContent(mappings));
                    if (!mappingsResponse.IsSuccessStatusCode)
                    {
                        _logger.LogError("Failed to create document mappings for ServiceId: {ServiceId}", service.ServiceId);
                    }

                    TempData["SuccessResponse"] = await mappingsResponse.Content.ReadAsStringAsync();
                }

                return RedirectToAction(nameof(MerchantServiceIndex));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during service creation.");
                ModelState.AddModelError(string.Empty, "Failed to create service.");
                return View(service);
            }
        }
        // GET: Service/Edit/{id}
        public async Task<IActionResult> MerchantServiceEdit(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"service/GetServiceById?id=" + id);
                if (!response.IsSuccessStatusCode)
                {
                    return View("Error", new ErrorViewModel { RequestId = "Error fetching service for edit." });
                }

                var jsonString = await response.Content.ReadAsStringAsync();
                var service = JsonConvert.DeserializeObject<Service>(jsonString);

                // Pre-select already attached documents
                var mappingsResponse = await _httpClient.GetAsync($"ServiceDocumentMapping/GetMappingsByService?id=" + id);
                List<int> selectedDocumentIds = new List<int>();

                if (mappingsResponse.IsSuccessStatusCode)
                {
                    var mappingsString = await mappingsResponse.Content.ReadAsStringAsync();
                    var serviceDocumentMappings = JsonConvert.DeserializeObject<List<ServiceDocumentMapping>>(mappingsString);
                    selectedDocumentIds = serviceDocumentMappings.Select(m => m.ServiceDocumentListId).ToList();
                }

                // Populate the document list for selection and pass the selected IDs
                ViewBag.DocumentListComboData = await DocumentListItems(selectedDocumentIds);
                ViewBag.ServiceListComboData = await ServiceListItems(service.ServiceName);
                ViewBag.ServiceCategoryComboData = await ServiceCategoryItems(service.CategoryID);

                // Return the service model to the view
                return View(service);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching service for edit.");
                ModelState.AddModelError(string.Empty, "An error occurred.");
                return View(new Service());
            }
        }

        // POST: Service/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MerchantServiceEdit(int id, [Bind("ServiceId,CategoryID,ServiceName,Description,ServicePrice,ServiceAmountPaidToAdmin")] Service service, List<int> selectedDocumentIds)
        {
            if (id != service.ServiceId)
            {
                return BadRequest();
            }
            var merchantId = HttpContext.Session.GetEncryptedString("ProviderId", _protector);
            try
            {
                service.SelectedDocumentIds = String.Join(",", selectedDocumentIds.Select(i => i.ToString()).ToArray());
                service.MerchantID = Convert.ToInt32(merchantId);
                if (ModelState.IsValid)
                {
                    // Update the service
                    var response = await _httpClient.PostAsync($"service/UpdateService?id={id}", Customs.GetJsonContent(service));
                    response.EnsureSuccessStatusCode();

                    // Update document mappings
                    if (selectedDocumentIds != null && selectedDocumentIds.Count > 0)
                    {
                        // First, delete old mappings
                        await _httpClient.GetAsync($"ServiceDocumentMapping/DeleteMappingsByServiceId?Id={id}");

                        // Add new mappings
                        List<ServiceDocumentMapping> mappings = selectedDocumentIds.Select(docId => new ServiceDocumentMapping
                        {
                            ServiceID = service.ServiceId,
                            ServiceDocumentListId = docId
                        }).ToList();
                        var mappingsPayload = JsonConvert.SerializeObject(mappings);
                        var mappingsResponse = await _httpClient.PostAsync("ServiceDocumentMapping/CreateMapping", Customs.GetJsonContent(mappings));
                        mappingsResponse.EnsureSuccessStatusCode();
                    }

                    return RedirectToAction(nameof(MerchantServiceIndex));
                }

                return View(service);
            }
            catch (Exception ex)
            {
                // Log exception
                _logger.LogError(ex, "Error occurred while updating the service.");
                return View(service);
            }
        }

        // GET: Service/Delete/{id}
        public async Task<IActionResult> MerchantServiceDelete(int id)
        {
            var response = await _httpClient.GetAsync($"service/GetServiceById?id={id}");
            if (!response.IsSuccessStatusCode)
            {
                return View("Error", new ErrorViewModel { RequestId = "Error fetching service for delete." });
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var service = JsonConvert.DeserializeObject<Service>(jsonString);

            // Fetch related documents if needed
            var mappingsResponse = await _httpClient.GetAsync($"ServiceDocumentMapping/GetMappingsByServiceId?id={id}");
            if (mappingsResponse.IsSuccessStatusCode)
            {
                var mappingsString = await mappingsResponse.Content.ReadAsStringAsync();
                var serviceDocumentMappings = JsonConvert.DeserializeObject<List<ServiceDocumentMapping>>(mappingsString);
                ViewBag.ServiceDocumentMappings = serviceDocumentMappings;
            }

            return View(service);
        }

        // POST: Service/DeleteConfirmed/{id}
        public async Task<IActionResult> MerchantServiceDeleteConfirmed(int id)
        {
            try
            {
                // Delete the service document mappings
                var mappingsResponse = await _httpClient.GetAsync($"ServiceDocumentMapping/DeleteMappingsByServiceId?Id={id}");
                if (!mappingsResponse.IsSuccessStatusCode)
                {
                    return View("Error", new ErrorViewModel { RequestId = "Error deleting service document mappings." });
                }

                // Delete the service
                var response = await _httpClient.GetAsync($"service/DeleteService?id={id}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(MerchantServiceIndex));
                }

                return View("Error", new ErrorViewModel { RequestId = "Error deleting service." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting the service.");
                return View("Error", new ErrorViewModel { RequestId = "Error occurred while deleting the service." });
            }
        }
        public async Task<List<SelectListItem>> ServiceListItems(string servicename)
        {
            var jsonResponse = await _httpClient.GetAsync($"ServicesList/GetServicesList");
            var responseString = await jsonResponse.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(responseString))
            {
                try
                {
                    List<ServicesList> xerviceList = JsonConvert.DeserializeObject<List<ServicesList>>(responseString);
                    return xerviceList.Select(i => new SelectListItem
                    {
                        Text = i.ServiceName,
                        Value = i.ServiceName.ToString(),
                        Selected = (servicename == i.ServiceName ? true : false)
                    }).ToList();
                }
                catch (JsonSerializationException ex)
                {
                    // Log the exception details
                    _logger.LogError(ex, "JSON deserialization error.");
                    // Handle the error response accordingly
                    ModelState.AddModelError(string.Empty, "Failed to load Data.");
                    return new List<SelectListItem>();
                }
            }
            else
            {
                return new List<SelectListItem>();
            }

        }
        public async Task<int> GetServiceId(string servicename)
        {
            int ServiceId = 0;
            var jsonResponse = await _httpClient.GetAsync($"ServicesList/GetServicesList");
            var responseString = await jsonResponse.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(responseString))
            {
                try
                {
                    List<ServicesList> xerviceList = JsonConvert.DeserializeObject<List<ServicesList>>(responseString);
                    ServiceId = Convert.ToInt32(xerviceList.Where(x => x.ServiceName == servicename).Select(x => x.ServiceListID).First());
                    return ServiceId;
                }
                catch (JsonSerializationException ex)
                {
                    // Log the exception details
                    _logger.LogError(ex, "JSON deserialization error.");
                    // Handle the error response accordingly
                    ModelState.AddModelError(string.Empty, "Failed to load Data.");
                    return 0;
                }
            }
            else
            {
                return 0;
            }

        }
        public async Task<List<SelectListItem>> ServiceCategoryItems(int? id)
        {
            var jsonResponse = await _httpClient.GetAsync($"ServiceCategory/GetAllServiceCategories");
            var responseString = await jsonResponse.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(responseString))
            {
                try
                {
                    List<ServiceCategory> xerviceList = JsonConvert.DeserializeObject<List<ServiceCategory>>(responseString);
                    return xerviceList.Select(i => new SelectListItem
                    {
                        Text = i.CategoryName,
                        Value = i.CategoryId.ToString(),
                        Selected = (id == i.CategoryId ? true : false)
                    }).ToList();
                }
                catch (JsonSerializationException ex)
                {
                    // Log the exception details
                    _logger.LogError(ex, "JSON deserialization error.");
                    // Handle the error response accordingly
                    ModelState.AddModelError(string.Empty, "Failed to load Data.");
                    return new List<SelectListItem>();
                }
            }
            else
            {
                return new List<SelectListItem>();
            }

        }
        public async Task<List<SelectListItem>> GetDocumentListItems()
        {
            var jsonResponse = await _httpClient.GetAsync("ServiceDocumentList/GetAllDocuments");
            var responseString = await jsonResponse.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(responseString))
            {
                try
                {
                    List<ServiceDocumenList> documentList = JsonConvert.DeserializeObject<List<ServiceDocumenList>>(responseString);
                    return documentList.Select(i => new SelectListItem
                    {
                        Text = i.ServiceDocumentName,
                        Value = i.ServiceDocumenListtId.ToString()
                    }).ToList();
                }
                catch (JsonSerializationException ex)
                {
                    _logger.LogError(ex, "JSON deserialization error.");
                    ModelState.AddModelError(string.Empty, "Failed to load Document List.");
                    return new List<SelectListItem>();
                }
            }
            else
            {
                return new List<SelectListItem>();
            }
        }
        public async Task<List<int>> GetDefaultServiceDocumentListData(int serviceID)
        {
            try
            {
                var response = await _httpClient.GetAsync("MServiceDefaultDocument/GetServiceDocuments?serviceID=" + serviceID);

                if (response.IsSuccessStatusCode)
                {
                    var documents = await response.Content.ReadAsStringAsync();
                    List<M_SericeDefaultDocumentList> documentList = JsonConvert.DeserializeObject<List<M_SericeDefaultDocumentList>>(documents);
                    List<int> result = documentList.Select(x => x.DocumentID).ToList();
                    _logger.LogInformation("Fetched service documents from API successfully.");
                    return result;
                }
                else
                {
                    _logger.LogError($"Failed to fetch documents. Status code: {response.StatusCode}");
                    return new List<int>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred: {ex.Message}");
                return new List<int>();
            }
        }
    }
}
