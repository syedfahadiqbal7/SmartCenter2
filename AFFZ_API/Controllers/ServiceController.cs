using AFFZ_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class ServiceController : ControllerBase
{
    private readonly MyDbContext _context;
    private readonly ILogger<ServiceController> _logger;
    public ServiceController(MyDbContext context, ILogger<ServiceController> logger)
    {
        _context = context;
        _logger = logger;
    }
    // GET: api/Service
    [HttpGet("GetAllServices")]
    public async Task<ActionResult<IEnumerable<Service>>> GetAllServices(int pageNumber = 1, int pageSize = 10, int merchantId = 0)
    {
        try
        {
            // Skip and take based on the pageNumber and pageSize
            var paginatedServices = await _context.Services
                .Where(x => x.MerchantID == merchantId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalRecords = await _context.Services.Where(x => x.MerchantID == merchantId).CountAsync();

            // Return paginated list and total count in headers
            Response.Headers.Add("X-Total-Count", totalRecords.ToString());

            return Ok(paginatedServices);
        }
        catch (Exception)
        {
            throw;
        }
    }


    // GET: api/Service/5
    [HttpGet("GetServiceById")]
    public async Task<ActionResult<Service>> GetServiceById(int id)
    {
        var service = await _context.Services.FindAsync(id);

        if (service == null)
        {
            return NotFound();
        }

        return service;
    }

    // PUT: api/Service/5
    [HttpPost("UpdateService")]
    public async Task<IActionResult> UpdateService(int id, Service service)
    {
        if (id != service.ServiceId)
        {
            return BadRequest();
        }

        _context.Entry(service).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ServiceExists(id))
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
    // POST: api/Service
    [HttpPost("CreateService")]
    public async Task<IActionResult> CreateService(Service service)
    {
        try
        {
            if (service == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid request data.");
            }

            // Do not set service.ServiceId explicitly
            _context.Services.Add(service);
            await _context.SaveChangesAsync(); // ServiceId is generated here

            return Ok(new { service.ServiceId }); // Return the new ServiceId
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the service.");
            return StatusCode(500, "An internal server error occurred.");
        }
    }
    // DELETE: api/Service/5
    [HttpGet("DeleteService")]
    public async Task<IActionResult> DeleteService(int id)
    {
        var service = await _context.Services.FindAsync(id);
        if (service == null)
        {
            return NotFound();
        }

        _context.Services.Remove(service);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    private bool ServiceExists(int id)
    {
        return _context.Services.Any(e => e.ServiceId == id);
    }
}
