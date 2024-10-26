using AFFZ_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AFFZ_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceDocumentMappingController : ControllerBase
    {
        private readonly MyDbContext _context;

        public ServiceDocumentMappingController(MyDbContext context)
        {
            _context = context;
        }

        [HttpPost("CreateMapping")]
        public async Task<IActionResult> CreateMapping(List<ServiceDocumentMapping> mappings)
        {
            try
            {
                if (mappings == null || mappings.Count == 0)
                {
                    return BadRequest("Invalid data.");
                }

                // Log the received mappings for debugging
                foreach (var mapping in mappings)
                {
                    Console.WriteLine($"Received Mapping: ServiceID: {mapping.ServiceID}, ServiceDocumentListId: {mapping.ServiceDocumentListId}");
                }

                await _context.ServiceDocumentMapping.AddRangeAsync(mappings);
                await _context.SaveChangesAsync();

                return Ok("Mappings created successfully.");
            }
            catch (Exception ex)
            {
                // Log exception
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("GetMappingsByService")]
        public async Task<ActionResult<IEnumerable<ServiceDocumentMapping>>> GetMappingsByService(int Id)
        {
            var mappings = await _context.ServiceDocumentMapping
                                         .Where(m => m.ServiceID == Id)
                                         .ToListAsync();

            if (mappings == null || mappings.Count == 0)
            {
                return NotFound();
            }

            return Ok(mappings);
        }

        [HttpGet("DeleteMappingsByServiceId")]
        public IActionResult DeleteMappingsByServiceId(int Id)
        {
            List<ServiceDocumentMapping> service = _context.ServiceDocumentMapping.Where(x => x.ServiceID == Id).ToList();
            _context.ServiceDocumentMapping.RemoveRange(service);
            _context.SaveChanges();

            return Ok("Mappings deleted successfully.");
        }

    }
}
