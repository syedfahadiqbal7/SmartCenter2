using AFFZ_API.Models;
using AFFZ_API.SignalRHub;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace AFFZ_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class NotificationsController : ControllerBase
	{
		private readonly MyDbContext _context;
		private readonly ILogger<NotificationsController> _logger;

		public NotificationsController(MyDbContext context, ILogger<NotificationsController> logger)
		{
			_context = context;
			_logger = logger;
		}

		[HttpPost("CreateNotification")]
		public async Task<IActionResult> CreateNotification(Notification notification)
		{
			notification.CreatedDate = DateTime.UtcNow;
			notification.IsRead = false;
			_context.Notifications.Add(notification);
			await _context.SaveChangesAsync();
			// Broadcast the notification
			var hubContext = (IHubContext<NotificationHub>)HttpContext.RequestServices.GetService(typeof(IHubContext<NotificationHub>));
			await hubContext.Clients.User(notification.UserId == notification.MessageFromId.ToString() ? notification.MerchantId : notification.UserId)
								  .SendAsync("ReceiveNotification", notification.Message);
			return Ok(notification);
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
											  .Where(n => n.MessageFromId != id && n.UserId == userId)
											  .OrderByDescending(n => n.Id)
											  .ToListAsync();

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
											  .Where(n => n.MessageFromId != id && n.MerchantId == merchantId)
											  .OrderByDescending(n => n.Id)
											  .ToListAsync();

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
			await _context.SaveChangesAsync();

			return Ok();
		}
		[HttpPost("mark-all-as-read")]
		public async Task<IActionResult> MarkAllAsRead(string UserOrMerchantid, bool isUser)
		{
			List<Notification> notifications = new List<Notification>();
			if (isUser)
			{
				notifications = await _context.Notifications.Where(x => x.UserId == UserOrMerchantid).ToListAsync();
			}
			else
			{
				notifications = await _context.Notifications.Where(x => x.MerchantId == UserOrMerchantid).ToListAsync();
			}
			if (notifications == null)
			{
				return NotFound();
			}
			foreach (var notification in notifications)
			{
				notification.IsRead = true;
				await _context.SaveChangesAsync();
			}
			return Ok();
		}
	}
}
