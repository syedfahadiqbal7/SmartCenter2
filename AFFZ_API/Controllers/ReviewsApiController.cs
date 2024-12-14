using AFFZ_API.Models;
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

        public ReviewsApiController(MyDbContext context, ILogger<ReviewsApiController> logger)
        {
            _context = context;
            _logger = logger;
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
                _logger.LogError($"Error in {nameof(GetAllReviews)}: {ex.Message}");
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
                    .GroupBy(r => new { r.ServiceId, r.Service.ServiceName })
                    .Select(g => new
                    {
                        ServiceId = g.Key.ServiceId,
                        ServiceName = g.Key.ServiceName,
                        AverageRating = g.Average(r => r.Rating),
                        TotalReviews = g.Count(),
                        Reviews = g.Select(r => new
                        {
                            r.ReviewText,
                            r.Rating,
                            r.ReviewDate
                        }).ToList()
                    })
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
    }
}
