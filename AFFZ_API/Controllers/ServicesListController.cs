using AFFZ_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AFFZ_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesListController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly ILogger<ServicesListController> _logger;

        public ServicesListController(MyDbContext context, ILogger<ServicesListController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/ServicesList
        [HttpGet("GetServicesList")]
        public async Task<ActionResult<IEnumerable<ServicesList>>> GetServicesList()
        {
            return await _context.ServicesLists.ToListAsync();
        }

        // GET: api/ServicesList/5
        [HttpGet("GetServicesListById")]
        public async Task<ActionResult<ServicesList>> GetServicesListById(int id)
        {
            var servicesList = await _context.ServicesLists.FindAsync(id);

            if (servicesList == null)
            {
                return NotFound();
            }

            return servicesList;
        }

        // PUT: api/ServicesList/5
        [HttpPost("UpdateServicesList")]
        public async Task<IActionResult> UpdateServicesList(int id, ServicesList servicesList)
        {
            if (id != servicesList.ServiceListID)
            {
                return BadRequest();
            }

            _context.Entry(servicesList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServicesListExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ServicesList
        [HttpPost]
        public async Task<ActionResult<ServicesList>> PostServicesList(ServicesList servicesList)
        {
            _context.ServicesLists.Add(servicesList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetServicesList", new { id = servicesList.ServiceListID }, servicesList);
        }

        // DELETE: api/ServicesList/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServicesList(int id)
        {
            var servicesList = await _context.ServicesLists.FindAsync(id);
            if (servicesList == null)
            {
                return NotFound();
            }

            _context.ServicesLists.Remove(servicesList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServicesListExists(int id)
        {
            return _context.ServicesLists.Any(e => e.ServiceListID == id);
        }
    }
}
