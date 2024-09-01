using AFFZ_Customer.Models;
using Microsoft.AspNetCore.DataProtection;
using Newtonsoft.Json;

namespace AFFZ_Customer.Utils
{
    public class NotificationService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        IDataProtector _protector;
        public NotificationService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IDataProtectionProvider provider, IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Main");
            _httpContextAccessor = httpContextAccessor;
            _protector = provider.CreateProtector("Example.SessionProtection");
        }
        public string GetUserId()
        {
            return _httpContextAccessor.HttpContext.Session.GetEncryptedString("UserId", _protector);
        }
        public async Task<List<Notification>> GetNotificationsAsync()
        {
            List<Notification> notificationsList = new List<Notification>();
            string userId = _httpContextAccessor.HttpContext.Session.GetEncryptedString("UserId", _protector); // Placeholder for session user ID retrieval
            if (string.IsNullOrEmpty(userId)) return new List<Notification>();
            var notifications = await _httpClient.GetAsync($"Notifications/GetUserNotifications/{userId}");
            notifications.EnsureSuccessStatusCode();
            if (notifications != null)
            {
                var responseString = await notifications.Content.ReadAsStringAsync();
                notificationsList = JsonConvert.DeserializeObject<List<Notification>>(responseString);
            }
            return notificationsList ?? new List<Notification>();
        }
    }
}
