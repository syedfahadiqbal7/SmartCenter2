using AFFZ_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
namespace AFFZ_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainPageController : ControllerBase
    {
        private readonly MyDbContext _context;
        public MainPageController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Home
        [HttpGet("GetCities/{Prefix?}")]
        public IActionResult GetCities(string Prefix = "")
        {
            // Note: you can bind the same list from the database
            List<City> ObjList = new List<City>()
        {
            new City {Id=1,Name="Sharjah" },
            new City {Id=2,Name="Dubai" },
            new City {Id=3,Name="Abu Dhabi" },
            new City {Id=4,Name="Fujairah" },
            new City {Id=5,Name="Ajman" },
            new City {Id=6,Name="Ras al Khaimah" },
            new City {Id=7,Name="Umm al-Quwain" }
        };

            // Conditional check for the prefix
            // Convert Prefix to lowercase to make the search case-insensitive
            Prefix = Prefix?.ToLower();

            // Conditional check for the prefix with case-insensitive comparison
            var result = string.IsNullOrEmpty(Prefix)
                ? ObjList.Select(c => new { c.Name })
                : ObjList.Where(c => c.Name.ToLower().StartsWith(Prefix)).Select(c => new { c.Name });

            string json = JsonConvert.SerializeObject(result);
            return Ok(json);
        }

        [HttpGet("GetCategories/{Prefix?}")]
        public async Task<IActionResult> GetCategories(string Prefix = "")
        {
            List<ServiceCategory> ObjList = await _context.ServiceCategories.ToListAsync();
            // Convert Prefix to lowercase to make the search case-insensitive
            Prefix = Prefix?.ToLower();
            var result = string.IsNullOrEmpty(Prefix)
                ? ObjList.Select(c => new { c.CategoryName, c.CategoryId })
                : ObjList.Where(c => c.CategoryName.ToLower().StartsWith(Prefix)).Select(c => new { c.CategoryName, c.CategoryId });

            string json = JsonConvert.SerializeObject(result);
            return Ok(json);
        }
    }
}
