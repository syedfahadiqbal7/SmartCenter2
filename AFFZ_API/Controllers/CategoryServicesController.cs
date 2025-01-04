using AFFZ_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AFFZ_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryServicesController : ControllerBase
    {
        private readonly MyDbContext _context;
        public CategoryServicesController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("AllCategories")]
        public async Task<ActionResult<IEnumerable<Service>>> GetSubCategoriesList()
        {
            /*
			 * GroupBy: The query uses GroupBy on CategoryName to group the records by the distinct CategoryName.
			 * Select: The Select method is then used to project the grouped data into a new ServiceCategoryDto. The CategoryId is taken from the first record in each group, and the CategoryName is the grouping key (g.Key).
			 
			 */

            try
            {
                //var categories = await _context.Services.GroupBy(sc => new { sc.ServiceName, sc.ServicePrice, sc.Merchant.MerchantLocation, sc.CategoryID, }).Select(g => new SubCatPage
                //{
                //    CatId = g.Key.CategoryID,
                //    ServiceName = g.Key.ServiceName,
                //    ServicePrice = g.Key.ServicePrice,
                //    Location = g.Key.MerchantLocation
                //}).ToListAsync();
                var categories = await _context.Services
                .GroupBy(sc => new
                {
                    sc.ServiceName,
                    sc.ServicePrice,
                    sc.Merchant.MerchantLocation,
                    sc.CategoryID
                })
                .Select(g => new
                {
                    g.Key.ServiceName,
                    g.Key.ServicePrice,
                    g.Key.MerchantLocation,
                    g.Key.CategoryID
                })
                .Join(
                    _context.ServicesLists, // Joining with ServicesList
                    s => s.ServiceName,    // Matching ServiceName in Services
                    sl => sl.ServiceName,  // with ServiceName in ServicesList
                    (s, sl) => new SubCatPage
                    {
                        CatId = s.CategoryID,
                        ServiceName = s.ServiceName,
                        ServicePrice = s.ServicePrice,
                        Location = s.MerchantLocation,
                        ServiceImage = sl.ServiceImage // Including ServiceImage
                    })
                .ToListAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpGet("getServiceByName")]
        public async Task<ActionResult<Service>> GetServiceByName(string id)
        {

            int sid = Convert.ToInt32(id);
            var servicedata = _context.Services.Where(x => x.ServiceId == sid);

            return Ok(servicedata);
        }
    }
    //Defines the data structure returned by the search endpoint, including service name, company name, merchant location, price, and average rating.
    public class ServiceCategoryDto
    {
        public string CategoryName { get; set; }
    }
}
