using AFFZ_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AFFZ_API.Controllers.MerchantControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly MyDbContext _context;

        public DashboardController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetDashboardStatistics(int merchantId)
        {
            try
            {
                var totalUsersServed = await _context.RequestForDisCountToUsers
                    .Select(r => r.UID).Distinct().CountAsync();

                var totalSuccessRequests = await _context.RequestForDisCountToUsers
                    .CountAsync(r => r.IsPaymentDone == 1);

                var totalFailedRequests = await _context.RequestForDisCountToUsers
                    .CountAsync(r => r.IsPaymentDone == 0);

                var totalInProgressRequests = await _context.RequestForDisCountToUsers
                    .CountAsync(r => r.IsPaymentDone == null && r.IsMerchantSelected == 1);

                var result = new
                {
                    TotalUsersServed = totalUsersServed,
                    TotalSuccessRequests = totalSuccessRequests,
                    TotalFailedRequests = totalFailedRequests,
                    TotalInProgressRequests = totalInProgressRequests
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving dashboard statistics.", error = ex.Message });
            }
        }

        [HttpGet("GetTotalRevenueAsync")]
        public async Task<decimal> GetTotalRevenueAsync(int mId)
        {
            var totalRevenue = await _context.PaymentHistories
                .AsNoTracking()
                .Where(p => p.MERCHANTID == mId)
                .Select(p => p.AMOUNT)
                .ToListAsync();

            decimal sum = 0;

            foreach (var amountStr in totalRevenue)
            {
                if (decimal.TryParse(amountStr, out decimal amount))
                {
                    sum += amount;
                }
                else
                {
                    // Handle parsing error (e.g., log or skip invalid entries)
                }
            }

            return sum;
        }
        //
        [HttpGet("GetTotalRevenueLastWeekAsync")]
        public async Task<decimal> GetTotalRevenueLastWeekAsync(int mId)
        {
            var startOfWeek = DateTime.UtcNow.Date.AddDays(-(int)DateTime.UtcNow.DayOfWeek); // Start of the current week (Sunday)
            var endOfWeek = startOfWeek.AddDays(7).AddSeconds(-1); // End of the current week (Saturday at 23:59:59)

            var totalRevenue = (await _context.PaymentHistories
                .AsNoTracking()
                .Where(p => p.MERCHANTID == mId && p.PAYMENTDATETIME >= startOfWeek && p.PAYMENTDATETIME <= endOfWeek)
                .ToListAsync())
                .Sum(p => decimal.TryParse(p.AMOUNT, out var amount) ? amount : 0); // Parse AMOUNT on the client side



            return totalRevenue;
        }

        [HttpGet("GetTopRevenueServiceAsync")]
        public async Task<TopServiceRevenueDto> GetTopRevenueServiceAsync(int mid)
        {
            try
            {
                var payments = await _context.PaymentHistories
                                .Include(p => p.Service)
                                .Where(p => p.MERCHANTID == mid)
                                .ToListAsync();

                var topService = payments
                    .GroupBy(p => new { p.SERVICEID, p.Service.SID })
                    .Select(g => new
                    {
                        g.Key.SERVICEID,
                        g.Key.SID,
                        TotalRevenue = g.Sum(p =>
                            decimal.TryParse(p.AMOUNT, out decimal amount) ? amount : 0)
                    })
                    .Join(
                        _context.ServicesLists, // Join with ServicesLists to get ServiceName
                        grouped => grouped.SID,
                        serviceList => serviceList.ServiceListID,
                        (grouped, serviceList) => new TopServiceRevenueDto
                        {
                            ServiceId = grouped.SERVICEID,
                            ServiceName = serviceList.ServiceName, // Fetch ServiceName from ServicesList
                            TotalRevenue = grouped.TotalRevenue
                        }
                    )
                    .OrderByDescending(s => s.TotalRevenue)
                    .FirstOrDefault();

                return topService;


            }
            catch (InvalidCastException ex)
            {
                // Log the exception (implement logging as per your logging framework)
                throw new Exception("An error occurred while retrieving the top revenue service.", ex);
            }
            catch (Exception ex)
            {
                // Log the exception (implement logging as per your logging framework)
                throw new Exception("An error occurred while retrieving the top revenue service.", ex);
            }
        }
        [HttpGet("GetRecentTransactionsAsync")]
        public async Task<IActionResult> GetRecentTransactionsAsync(int mId, int count)
        {
            //return await _context.PaymentHistories
            //    .AsNoTracking()
            //    .Where(p => p.MERCHANTID == mId)
            //    .OrderByDescending(p => p.PAYMENTDATETIME)
            //    .Take(count)
            //    .ToListAsync();
            var paymentHistoriesWithCustomerNames = await (
                from payment in _context.PaymentHistories.AsNoTracking()
                join customer in _context.Customers.AsNoTracking()
                    on payment.PAYERID equals customer.CustomerId
                where payment.MERCHANTID == mId
                orderby payment.PAYMENTDATETIME descending
                select new
                {
                    payment.ID,
                    payment.PAYMENTTYPE,
                    payment.AMOUNT,
                    payment.PAYERID,
                    payment.MERCHANTID,
                    payment.ISPAYMENTSUCCESS,
                    payment.Quantity,
                    payment.SERVICEID,
                    payment.PAYMENTDATETIME,
                    CustomerName = customer.CustomerName
                }
            ).Take(count).ToListAsync();

            return Ok(paymentHistoriesWithCustomerNames);
        }
    }
}
