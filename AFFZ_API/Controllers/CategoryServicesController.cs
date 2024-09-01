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

            var categories = await _context.Services.GroupBy(sc => new { sc.ServiceName, sc.ServicePrice, sc.Merchant.MerchantLocation, sc.CategoryId }).Select(g => new SubCatPage
            {
                CatId = g.Key.CategoryId,
                ServiceName = g.Key.ServiceName,
                ServicePrice = g.Key.ServicePrice,
                Location = g.Key.MerchantLocation
            }).ToListAsync();

            return Ok(categories);
        }
    }
    //Defines the data structure returned by the search endpoint, including service name, company name, merchant location, price, and average rating.
    public class ServiceCategoryDto
    {
        public string CategoryName { get; set; }
    }
}
