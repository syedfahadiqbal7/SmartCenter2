using AFFZ_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> sendRequestToSavePayment(PaymentHistory savePaymentHistory)
        {
            _logger.LogInformation("sendRequestToSavePayment method called with UserId: {UserId}", savePaymentHistory.PAYERID);
            try
            {
                // Save payment history
                _context.Add(savePaymentHistory);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Payment Done Successfully.");

                // Check if the payment was successful
                if (savePaymentHistory.ISPAYMENTSUCCESS == 1)
                {
                    // Call CompleteReferral stored procedure to update referral status
                    var commandCompleteReferral = "EXEC CompleteReferral @ReferredCustomerID";
                    var referredCustomerIdParam = new SqlParameter("@ReferredCustomerID", savePaymentHistory.PAYERID);
                    await _context.Database.ExecuteSqlRawAsync(commandCompleteReferral, referredCustomerIdParam);

                    // Call UpdateReferrerPoints stored procedure to add points to referrer and referred customer
                    var commandUpdateReferrerPoints = "EXEC UpdateReferrerPoints @ReferredCustomerID, @AmountSpent";
                    var amountSpentParam = new SqlParameter("@AmountSpent", savePaymentHistory.AMOUNT);
                    await _context.Database.ExecuteSqlRawAsync(commandUpdateReferrerPoints, referredCustomerIdParam, amountSpentParam);
                }

                return CreatedAtAction(nameof(sendRequestToSavePayment), new { id = savePaymentHistory.ID }, savePaymentHistory);
            }
            catch (Exception ex)
            {
                // Log the exception details
                _logger.LogError(ex, "An error occurred while processing the payment request.");
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
