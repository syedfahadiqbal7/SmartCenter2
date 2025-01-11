using AFFZ_API.Interfaces;
using AFFZ_API.Models;
using AFFZ_API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AFFZ_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsApiController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly ILogger<ReviewsApiController> _logger;
        private readonly IEmailService _emailService;
        public ReviewsApiController(MyDbContext context, ILogger<ReviewsApiController> logger, IEmailService emailService)
        {
            _context = context;
            _logger = logger;
            _emailService = emailService;
        }

        [HttpGet("GetAllReviews")]
        public async Task<IActionResult> GetAllReviews(int merchantId)
        {
            try
            {
                var _review = await _context.Review
                    .Include(s => s.Service) // Include related reviews
                    .Include(s => s.CUser)
                    .Where(s => s.CustomerId == s.CUser.CustomerId && s.merchantId == merchantId)
                    .ToListAsync();
                if (_review == null)
                {
                    return NotFound($"Review not found.");
                }

                var reviews = _review; // Get reviews related to the service
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetAllReviews)}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("GetUserReviewList")]
        public async Task<IActionResult> GetUserReviewList(int userId)
        {
            try
            {
                var _review = await _context.Review
                    .Include(s => s.Service) // Include related reviews
                    .Include(s => s.CUser)
                    .Where(s => s.CustomerId == s.CUser.CustomerId && s.CustomerId == userId)
                    .ToListAsync();
                if (_review == null)
                {
                    return NotFound($"Review not found.");
                }

                var reviews = _review; // Get reviews related to the service
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetUserReviewList)}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetReviewByServiceId")]
        public async Task<IActionResult> GetReviewByServiceId(int serviceId)
        {
            try
            {
                var _review = await _context.Review
                    .Include(s => s.Service) // Include related reviews
                    .FirstOrDefaultAsync(s => s.ServiceId == serviceId);

                if (_review == null)
                {
                    return NotFound($"Review with Service ID {serviceId} not found.");
                }

                var reviews = _review; // Get reviews related to the service
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetReviewByServiceId)}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("CreateReview")]
        public async Task<IActionResult> CreateOrUpdateReview([FromBody] ReviewCreate _review)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingReview = await _context.Review
                    .FirstOrDefaultAsync(r => r.ServiceId == _review.ServiceId && r.CustomerId == _review.CustomerId && r.merchantId == _review.merchantId && r.RFDFU == _review.RFDFU);

                if (existingReview != null)
                {
                    // Update existing review
                    existingReview.ReviewText = _review.ReviewText;
                    existingReview.Rating = _review.Rating;
                    existingReview.ReviewDate = _review.ReviewDate;

                    _context.Review.Update(existingReview);
                    await _context.SaveChangesAsync();

                    return Ok(new
                    {
                        message = "Review updated successfully",
                        review = existingReview
                    });
                }
                else
                {
                    // Create a new review
                    Review review = new Review
                    {
                        ServiceId = _review.ServiceId,
                        CustomerId = _review.CustomerId,
                        merchantId = _review.merchantId,
                        RFDFU = _review.RFDFU,
                        ReviewText = _review.ReviewText,
                        Rating = _review.Rating,
                        ReviewDate = _review.ReviewDate,
                        CUser = await _context.Customers.FirstOrDefaultAsync(x => x.CustomerId == _review.CustomerId),
                        Service = await _context.Services.FirstOrDefaultAsync(x => x.ServiceId == _review.ServiceId)
                    };

                    _context.Review.Add(review);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction(nameof(GetAllReviews), new { id = review.ReviewId }, review);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(CreateOrUpdateReview)}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("CheckIfReviewed")]
        public async Task<IActionResult> CheckIfReviewed(int serviceId, int customerId)
        {
            try
            {
                var review = await _context.Review.FirstOrDefaultAsync(r => r.ServiceId == serviceId && r.CustomerId == customerId);
                return Ok(new { HasReviewed = review != null });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(CheckIfReviewed)}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("GetAllReviewsWithAverageRating")]
        public async Task<IActionResult> GetAllReviewsWithAverageRating(int merchantId)
        {
            try
            {
                var reviewData = await _context.Review
                .Include(r => r.Service)
                .Where(r => r.merchantId == merchantId)
                .GroupBy(r => new { r.ServiceId, r.Service.SID })
                .Join(
                    _context.ServicesLists, // Join with ServicesLists to get ServiceName
                    g => g.Key.SID,
                    serviceList => serviceList.ServiceListID,
                    (g, serviceList) => new
                    {
                        ServiceId = g.Key.ServiceId,
                        ServiceName = serviceList.ServiceName, // Fetch ServiceName from ServicesList
                        AverageRating = g.Average(r => r.Rating),
                        TotalReviews = g.Count(),
                        Reviews = g.Select(r => new
                        {
                            r.ReviewText,
                            r.Rating,
                            r.ReviewDate
                        }).ToList()
                    }
                )
                .ToListAsync();

                if (!reviewData.Any())
                {
                    _logger.LogInformation($"No reviews found for merchantId: {merchantId}");
                    return NotFound("No reviews found.");
                }

                return Ok(reviewData);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetAllReviewsWithAverageRating)}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        private async Task<bool> SendNotificationEmailToMerchant(ReviewCreate notification)
        {
            try
            {
                string RedirectUrlLoc = string.Empty;


                EmailTemplate emailTemplate = new EmailTemplate();
                string userName = string.Empty;
                string EmailAddress = string.Empty;
                string _Message = $"{notification.ReviewText}. Rating Stars - {notification.Rating} out of 5";
                

                int CID = Convert.ToInt32(notification.CustomerId);
                int MID = Convert.ToInt32(notification.merchantId);
                userName = _context.ProviderUsers.FirstOrDefault(p => p.ProviderId == MID)?.ProviderName;
                string SenderName = _context.Customers.FirstOrDefault(c => c.CustomerId == CID)?.CustomerName;
                EmailAddress = _context.ProviderUsers.FirstOrDefault(p => p.ProviderId == MID)?.Email;
                string EmailTemplate = "<!DOCTYPE html>\n<html>\n<head>\n<style>\nbody { font-family: Arial, sans-serif; background-color: #f8f9fa; margin: 0; padding: 0; }\n.email-container { max-width: 600px; margin: 20px auto; background: #ffffff; border-radius: 10px; box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1); padding: 20px; }\n.header { text-align: center; color: #343a40; margin-bottom: 20px; }\n.header h1 { font-size: 24px; }\n.content { color: #555555; line-height: 1.6; }\n.footer { margin-top: 20px; text-align: center; font-size: 12px; color: #888888; }\n</style>\n</head>\n<body>\n<div class='email-container'>\n<div class='header'><h1>Reiew</h1></div>\n<div class='content'>\n<p style='font-weight:bold;'>Hello <strong>{{Name}}</strong>,</p>\n<p>" + _Message + "</p>\n</div>\n<div class='footer'><p>&copy; {{CurrentYear}} SmartCenter. All Rights Reserved.</p></div>\n</div>\n</body>\n</html>";
                emailTemplate.Body = EmailTemplate;


                // Replace Placeholders
                string emailBody = emailTemplate.Body
                    .Replace("{{Name}}", userName ?? "Application Merchant")
                    .Replace("{{ResetLink}}", RedirectUrlLoc)
                    .Replace("{{CurrentYear}}", DateTime.UtcNow.Year.ToString());

                // Simulate Email Sending
                _logger.LogInformation("Sending Email to: {Email}, Subject: {Subject}", EmailAddress, emailTemplate.Subject);
                _logger.LogInformation("Email Body: {Body}", emailBody);

                // Use your IEmailService here to send the email
                // Example:
                bool emailSent = await _emailService.SendEmail(EmailAddress, emailTemplate.Subject, emailBody, userName, isHtml: true);

                // Simulated Success
                return emailSent;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email for notification.");
                return false;
            }
        }
    }
}
