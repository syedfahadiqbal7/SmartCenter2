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
        public async Task<IActionResult> GetAllReviews()
        {
            try
            {
                var _review = await _context.Review
                    .Include(s => s.Service) // Include related reviews
                    .Include(s => s.CUser)
                    .Where(s => s.CustomerId == s.CUser.CustomerId)
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

        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Review.Add(review);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetAllReviews), new { id = review.ReviewId }, review);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(CreateReview)}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
