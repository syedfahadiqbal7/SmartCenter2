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

        public async Task<ActionResult> ProceedDirecttoPayment(string id)
        {
            _logger.LogInformation("ProceedDirecttoPayment method called with id: {Id}", id);
            string userId = HttpContext.Session.GetEncryptedString("UserId", _protector);
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogWarning("ProceedDirecttoPayment called with empty id");
                return NotFound();
            }
            RequestForDisCountToUserViewModel requestForDisCount = new RequestForDisCountToUserViewModel();
            var reqIds = id.Split('~');
            int MerchantId = Convert.ToInt32(reqIds[0]);
            int ServiceId = Convert.ToInt32(reqIds[1]);
            int UserId = Convert.ToInt32(HttpContext.Session.GetEncryptedString("UserId", _protector));
            try
            {

                var discountRequestClass = new DiscountRequestClass
                {
                    MerchantId = MerchantId,
                    ServiceId = ServiceId,
                    UserId = UserId
                };
                //Create Request...
                var Request = await _httpClient.PostAsync("CategoryWithMerchant/sendRequestForDiscount", Customs.GetJsonContent(discountRequestClass));
                var responseString = await Request.Content.ReadAsStringAsync();
                //get Detail of newly added request:
                RequestForDiscountViewModel mdl = new RequestForDiscountViewModel();
                var jsonResponse = await _httpClient.GetAsync("CategoryWithMerchant/AllRequestMerchant?Mid=" + reqIds[0]);
                jsonResponse.EnsureSuccessStatusCode();


                responseString = await jsonResponse.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(responseString))
                {
                    try
                    {
                        List<RequestForDiscountViewModel> categories = JsonConvert.DeserializeObject<List<RequestForDiscountViewModel>>(responseString);

                        mdl = categories.Where(x => x.MID == MerchantId && x.SID == ServiceId && x.UID == UserId).OrderByDescending(x => x.RequestDatetime).FirstOrDefault();
                    }
                    catch (JsonSerializationException ex)
                    {
                        // Log the exception details
                        _logger.LogError(ex, "JSON deserialization error.");

                        // Handle the error response accordingly
                        ViewBag.RequestForDisCountToMerchant = new List<RequestForDiscountViewModel>();
                        ModelState.AddModelError(string.Empty, "Failed to load Data.");
                    }
                }
                else
                {
                    TempData["FailMessage"] = "An error occurred while payment. Please Try Again";
                }

                //Discounted SAving...

                SubmitResponseByMerchant SRBM = new SubmitResponseByMerchant();
                SRBM.RFDTM = mdl.RFDTM.ToString();
                SRBM.DiscountPrice = mdl.ServicePrice.ToString();
                SRBM.UID = UserId.ToString();
                SRBM.SID = ServiceId.ToString();
                SRBM.MID = MerchantId.ToString();

                try
                {
                    var responseMessage1 = await _httpClient.PostAsync("CategoryWithMerchant/SaveMerchantResponseForDiscount", Customs.GetJsonContent(SRBM));
                    responseMessage1.EnsureSuccessStatusCode();
                    responseString = await responseMessage1.Content.ReadAsStringAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while processing the discount request.");
                    TempData["FailMessage"] = ex.Message;
                }
                //Check Response from Merchant...

                var jsonResponse1 = await _httpClient.GetAsync($"CategoryWithMerchant/AllResponsesFromMerchant?Uid={userId}");
                jsonResponse1.EnsureSuccessStatusCode();
                if (jsonResponse != null)
                {
                    var responseString3 = await jsonResponse1.Content.ReadAsStringAsync();
                    List<RequestForDisCountToUserViewModel> Check_Response_from_Merchant = JsonConvert.DeserializeObject<List<RequestForDisCountToUserViewModel>>(responseString3);
                    requestForDisCount = Check_Response_from_Merchant.Where(x => x.MerchantID == MerchantId && x.UID == UserId && x.SID == ServiceId && x.IsMerchantSelected == 0 && x.IsPaymentDone == 0).OrderByDescending(x => x.ResponseDateTime).FirstOrDefault();
                }
                else
                {
                    _logger.LogWarning("Empty response received from API for userId: {UserId}", userId);
                    ViewBag.ResponseForDisCountFromMerchant = new List<RequestForDisCountToUserViewModel>();
                }



                //Select Final Merchant...

                var srbm = new RequestForDisCountToUser
                {
                    RFDFU = requestForDisCount.RFDFU,
                    UID = requestForDisCount.UID,
                    MID = requestForDisCount.MerchantID,
                    IsMerchantSelected = 1,
                    IsPaymentDone = 0
                };

                var responseMessage2 = await _httpClient.PostAsync("CategoryWithMerchant/SelectFinalMerchant", Customs.GetJsonContent(srbm));
                responseString = await responseMessage2.Content.ReadAsStringAsync();



                // Trigger notification
                var notification = new Notification
                {
                    UserId = HttpContext.Session.GetEncryptedString("UserId", _protector).ToString(),
                    Message = $"User[{reqIds[1].ToString()}] has payed the amount. Proceed to next steps.",
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
            return RedirectToAction("Payment", "MerchantResponseToUser", new { rfdfu = requestForDisCount.RFDFU, uid = UserId, merchantId = MerchantId });
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
        public bool IsRequestedAlready { get; set; }
    }

    public class DiscountRequestClass
    {
        public int MerchantId { get; set; }
        public int ServiceId { get; set; }
        public int UserId { get; set; }
    }
    public class SubmitResponseByMerchant
    {
        public string RFDTM { get; set; }
        public string DiscountPrice { get; set; }
        public string MID { get; set; }
        public string UID { get; set; }
        public string SID { get; set; }
    }
}
