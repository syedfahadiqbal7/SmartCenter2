using AFFZ_API.Models;
using AFFZ_API.NotificationsHubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace AFFZ_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly ILogger<NotificationsController> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationsController(MyDbContext context, ILogger<NotificationsController> logger, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _logger = logger;
            _hubContext = hubContext;
        }

        [HttpPost("CreateNotification")]
        public async Task<IActionResult> CreateNotification(Notification notification)
        {
            try
            {
                notification.CreatedDate = DateTime.UtcNow;
                notification.ReadDate = new DateTime(1900, 1, 1);

                notification.IsRead = false;
                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();
                // Notify connected clients
                string userId = string.Empty;
                userId = notification.SenderType == "Customer" ? notification.MerchantId : notification.UserId;
                List<NotificationDTO> notify = await GetMerchantNotification(userId, notification.SenderType);
                // Send notifications based on sender type to only the targeted clients
                if (notification.SenderType == "Customer")
                {
                    await _hubContext.Clients.Group("AFFZ_Provider").SendAsync("ReceiveNotification", notify);
                }
                else if (notification.SenderType == "Merchant")
                {
                    await _hubContext.Clients.Group("AFFZ_Customer").SendAsync("ReceiveNotification", notify);
                }

                // await _hubContext.Clients.All.SendAsync("ReceiveNotification", notify);

                return Ok(notification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("GetUserNotifications/{userId}")]
        public async Task<IActionResult> GetUserNotifications(string userId)
        {
            try
            {
                int id = Convert.ToInt32(userId);
                string q = _context.Notifications
                                              .Where(n => n.MessageFromId != id && n.UserId == userId)
                                              .OrderByDescending(n => n.Id).ToQueryString();
                var notifications = await _context.Notifications
                        .Where(n => n.UserId == userId && n.SenderType == "Merchant")
                        .OrderByDescending(n => n.Id)
                        .Join(
                            _context.Merchants,
                            notification => notification.UserId,
                            merchant => merchant.MerchantId.ToString(),
                            (notification, merchant) => new NotificationDTO
                            {
                                Id = notification.Id,
                                UserId = notification.UserId,
                                SenderType = notification.SenderType,
                                MerchantId = notification.MerchantId,
                                MessageFromId = notification.MessageFromId,
                                Message = notification.Message,
                                CreatedDate = notification.CreatedDate,
                                ReadDate = notification.ReadDate,
                                SenderName = merchant.CompanyName,
                                RedirectToActionUrl = notification.RedirectToActionUrl,
                                NotificationType = notification.NotificationType,
                                IsRead = notification.IsRead
                            }
                        ).ToListAsync();
                // Apply placeholder replacements after data retrieval
                foreach (var notification in notifications)
                {
                    notification.Message = ReplaceAllPlaceholders(notification.Message, null, notification.SenderName, null);
                }

                return Ok(notifications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the file list.");
                return StatusCode(500, "Internal server error.");
            }

        }

        [HttpGet("GetMerchantNotifications/{merchantId}")]
        public async Task<IActionResult> GetMerchantNotifications(string merchantId)
        {
            int id = Convert.ToInt32(merchantId);
            var notifications = await _context.Notifications
                        .Where(n => n.MerchantId == merchantId && n.SenderType == "Customer")
                        .OrderByDescending(n => n.Id)
                        .Join(
                            _context.Customers,
                            notification => notification.UserId,
                            customer => customer.CustomerId.ToString(),
                            (notification, customer) => new NotificationDTO
                            {
                                Id = notification.Id,
                                UserId = notification.UserId,
                                SenderType = notification.SenderType,
                                MerchantId = notification.MerchantId,
                                MessageFromId = notification.MessageFromId,
                                Message = notification.Message,
                                CreatedDate = notification.CreatedDate,
                                ReadDate = notification.ReadDate,
                                SenderName = customer.CustomerName,
                                RedirectToActionUrl = notification.RedirectToActionUrl,
                                NotificationType = notification.NotificationType,
                                IsRead = notification.IsRead
                            }
                        ).ToListAsync();
            // Apply placeholder replacements after data retrieval
            foreach (var notification in notifications)
            {
                notification.Message = ReplaceAllPlaceholders(notification.Message, notification.SenderName, null, null);
            }

            return Ok(notifications);
        }

        [HttpPost("{id}/mark-as-read")]
        public async Task<IActionResult> MarkAsRead(string id)
        {
            var notification = await _context.Notifications.FindAsync(Convert.ToInt32(id));
            if (notification == null)
            {
                return NotFound();
            }

            notification.IsRead = true;
            notification.ReadDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok();
        }
        [HttpPost("mark-all-as-read")]
        public async Task<IActionResult> MarkAllAsRead(string UserOrMerchantid, bool isUser)
        {
            List<Notification> notifications = new List<Notification>();
            if (isUser)
            {
                notifications = await _context.Notifications.Where(x => x.UserId == UserOrMerchantid && x.SenderType == "Merchant" && x.IsRead == false).ToListAsync();
            }
            else
            {
                notifications = await _context.Notifications.Where(x => x.MerchantId == UserOrMerchantid && x.SenderType == "Customer" && x.IsRead == false).ToListAsync();
            }
            if (notifications == null)
            {
                return NotFound();
            }
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                notification.ReadDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        private async Task<List<NotificationDTO>> GetMerchantNotification(string userId, string Sendertype)
        {
            try
            {
                int id = Convert.ToInt32(userId);
                List<NotificationDTO> notifications = new List<NotificationDTO>();

                if (Sendertype == "Merchant")
                {
                    notifications = await _context.Notifications
                        .Where(n => n.UserId == userId && n.SenderType == Sendertype)
                        .OrderByDescending(n => n.Id)
                        .Join(
                            _context.Merchants,
                            notification => notification.UserId,
                            merchant => merchant.MerchantId.ToString(),
                            (notification, merchant) => new NotificationDTO
                            {
                                Id = notification.Id,
                                UserId = notification.UserId,
                                SenderType = notification.SenderType,
                                MerchantId = notification.MerchantId,
                                MessageFromId = notification.MessageFromId,
                                Message = notification.Message,
                                CreatedDate = notification.CreatedDate,
                                ReadDate = notification.ReadDate,
                                SenderName = merchant.CompanyName,
                                RedirectToActionUrl = notification.RedirectToActionUrl,
                                NotificationType = notification.NotificationType,
                                IsRead = notification.IsRead
                            }
                        ).ToListAsync();
                    // Apply placeholder replacements after data retrieval
                    foreach (var notification in notifications)
                    {
                        notification.Message = ReplaceAllPlaceholders(notification.Message, null, notification.SenderName, null);
                    }
                }
                else if (Sendertype == "Customer")
                {
                    notifications = await _context.Notifications
                        .Where(n => n.MerchantId == userId && n.SenderType == Sendertype)
                        .OrderByDescending(n => n.Id)
                        .Join(
                            _context.Customers,
                            notification => notification.UserId,
                            customer => customer.CustomerId.ToString(),
                            (notification, customer) => new NotificationDTO
                            {
                                Id = notification.Id,
                                UserId = notification.UserId,
                                SenderType = notification.SenderType,
                                MerchantId = notification.MerchantId,
                                MessageFromId = notification.MessageFromId,
                                Message = notification.Message,
                                CreatedDate = notification.CreatedDate,
                                ReadDate = notification.ReadDate,
                                SenderName = customer.CustomerName,
                                RedirectToActionUrl = notification.RedirectToActionUrl,
                                NotificationType = notification.NotificationType,
                                IsRead = notification.IsRead
                            }
                        ).ToListAsync();
                    // Apply placeholder replacements after data retrieval
                    foreach (var notification in notifications)
                    {
                        notification.Message = ReplaceAllPlaceholders(notification.Message, notification.SenderName, null, null);
                    }
                }

                return notifications;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the file list.");
                return new List<NotificationDTO>();
            }
        }

        // Helper method to replace placeholders based on multiple possible prefixes
        private string ReplaceAllPlaceholders(string message, string userName, string merchantName, string serviceName)
        {
            // Replace any occurrences of "User[<ID>]" with the userName
            if (!string.IsNullOrEmpty(userName))
            {
                message = Regex.Replace(message, @"User\[\d+\]", $"User[{userName}]");
            }

            // Replace any occurrences of "Merchant[<ID>]" with the merchantName
            if (!string.IsNullOrEmpty(merchantName))
            {
                message = Regex.Replace(message, @"Merchant\[\d+\]", $"Merchant[{merchantName}]");
            }

            // Extract and replace "Service[<ID>]" with the corresponding ServiceName from the database
            var serviceMatch = Regex.Match(message, @"Service\[(\d+)\]");
            string _sName = string.Empty;
            if (serviceMatch.Success)
            {
                int serviceId = int.Parse(serviceMatch.Groups[1].Value);

                // Query the database to get the ServiceName for the extracted ServiceId
                _sName = _context.Services.Where(s => s.ServiceId == serviceId).Select(s => s.ServiceName).FirstOrDefault();

                if (!string.IsNullOrEmpty(_sName))
                {
                    message = Regex.Replace(message, @"Service\[\d+\]", $"Service[{_sName}]");
                }
            }

            // Extract and replace "Service[<ID>]" with the corresponding ServiceName from the database
            var documentMatch = Regex.Match(message, @"Document\[(\d+)\]");
            string _dName = string.Empty;
            if (documentMatch.Success)
            {
                int docId = int.Parse(documentMatch.Groups[1].Value);

                // Query the database to get the ServiceName for the extracted ServiceId
                _dName = _context.UploadedFiles.Where(s => s.Ufid == docId).Select(s => s.FileName).FirstOrDefault();

                if (!string.IsNullOrEmpty(_dName))
                {
                    message = Regex.Replace(message, @"Document\[\d+\]", $"Document[{_dName}]");
                }
            }
            return message;
        }
    }
}
