using AFFZ_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace AFFZ_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly ILogger<PaymentController> _logger;
        public PaymentController(MyDbContext context, ILogger<PaymentController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [HttpPost("sendRequestToSavePayment")]
        public async Task<IActionResult> sendRequestToSavePayment(PaymentHistory SavePaymentHistory)
        {
            _logger.LogInformation("sendRequestToSavePayment method called with UserId: {UserId}", SavePaymentHistory.PAYERID);
            try
            {
                _context.Add(SavePaymentHistory);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Payment Done Successfully.");
                return Ok("Payment Done Successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception details
                // Use your preferred logging framework here. For example:
                _logger.LogError(ex, "An error occurred while processing the discount request.");

                return StatusCode(500, ex.Message);
            }
        }
        //
        [HttpPost("UpdateRequestForDisCountToUserForPaymentDone")]
        public async Task<IActionResult> UpdateRequestForDisCountToUserForPaymentDone(RequestForDisCountToUser updatePaymentStatus)
        {

            _logger.LogInformation("UpdateRequestForDisCountToUserForPaymentDont method called with UserId: {UserId}", updatePaymentStatus.UID);
            try
            {

                _context.Update(updatePaymentStatus);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Payment status updated.");
                return Ok("Payment status updated.");
            }
            catch (Exception ex)
            {
                // Log the exception details
                // Use your preferred logging framework here. For example:
                _logger.LogError(ex, "An error occurred while processing the discount request.");

                return StatusCode(500, ex.Message);
            }
        }
    }
}
