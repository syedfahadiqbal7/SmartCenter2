using AFFZ_API.Interfaces;
using AFFZ_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace AFFZ_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : Controller
    {
        private readonly MyDbContext _context;
        private readonly ILogger<FileUploadController> _logger;
        private readonly IEmailService _emailService;
        public FileUploadController(MyDbContext context, ILogger<FileUploadController> logger, IEmailService emailService)
        {
            _context = context;
            _logger = logger;
            _emailService = emailService;
        }
        [HttpGet("GetFilesList")]
        public async Task<ActionResult<IEnumerable<UploadedFile>>> GetFilesList()
        {
            try
            {
                List<UploadedFile> FileList = await _context.UploadedFiles.ToListAsync();
                return Ok(FileList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the file list.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("UploadFiles")]
        public async Task<IActionResult> UploadFiles([FromBody] FileUploadModelAPI model)
        {
            _logger.LogInformation("UploadFiles method called with UserId: {UserId}", model.UserId);
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state.");
                    return BadRequest(ModelState);
                }

                foreach (var file in model.UploadedFiles)
                {
                    if (file.FileSize > 0)
                    {
                        var uploadedFile = new UploadedFile
                        {
                            FileName = file.FileName,
                            ContentType = file.ContentType,
                            FileSize = file.FileSize,
                            UserId = model.UserId,
                            MerchantId = model.MID,
                            FolderName = "Documents_" + model.UserId,
                            Status = "Pending",
                            DocumentAddedDate = DateTime.Now,
                            DocumentModifiedDate = DateTime.Now
                        };
                        await _context.UploadedFiles.AddAsync(uploadedFile);
                    }
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation("Files uploaded successfully for UserId: {UserId}", model.UserId);
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while uploading files for UserId: {UserId}", model.UserId);
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("UsersWithDocuments")]
        public async Task<IActionResult> GetUsersWithDocuments()
        {
            _logger.LogInformation("GetUsersWithDocuments method called.");
            try
            {
                var usersWithDocuments = await _context.UploadedFiles
                    .Join(_context.RequestForDisCountToUsers,
                          uf => uf.UserId,
                          rfd => rfd.UID,
                          (uf, rfd) => new { uf.UserId, rfd.MID })
                    .GroupBy(x => new { x.UserId, x.MID })
                    .Select(g => new
                    {
                        g.Key.UserId,
                        g.Key.MID,
                        DocumentCount = g.Count()
                    })
                    .ToListAsync();

                if (!usersWithDocuments.Any())
                {
                    _logger.LogWarning("No users found with uploaded documents.");
                    return NotFound("No users found with uploaded documents.");
                }

                _logger.LogInformation("Users with documents retrieved successfully.");
                return Ok(usersWithDocuments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting users with documents.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("ReviewDocuments/{userId}")]
        public async Task<IActionResult> ReviewDocuments(int userId)
        {
            _logger.LogInformation("ReviewDocuments method called for UserId: {UserId}", userId);
            try
            {
                var documents = await _context.UploadedFiles.Where(d => d.UserId == userId).ToListAsync();
                if (!documents.Any())
                {
                    _logger.LogWarning("No documents found for UserId: {UserId}", userId);
                    return NotFound("No documents found for the user.");
                }

                _logger.LogInformation("Documents retrieved successfully for UserId: {UserId}", userId);
                return Ok(documents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while reviewing documents for UserId: {UserId}", userId);
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("VerifyDocument/{documentId}")]
        public async Task<IActionResult> VerifyDocument(int documentId)
        {
            _logger.LogInformation("VerifyDocument method called for DocumentId: {DocumentId}", documentId);
            try
            {
                var document = await _context.UploadedFiles.FindAsync(documentId);
                if (document == null)
                {
                    _logger.LogWarning("Document not found for DocumentId: {DocumentId}", documentId);
                    return NotFound("Document not found.");
                }

                document.Status = "Verified";
                document.DocumentModifiedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                var notification = new Notification
                {
                    UserId = document.UserId.ToString(),
                    Message = "Merchant has verify some document you shared.",
                };
                _logger.LogInformation("Document verified successfully for DocumentId: {DocumentId}", documentId);
                //return Ok(new { Message = "Document verified successfully." });
                return Ok("Document verified successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while verifying document for DocumentId: {DocumentId}", documentId);
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("DeleteDocument/{documentId}")]
        public async Task<IActionResult> DeleteDocument(int documentId)
        {
            _logger.LogInformation("DeleteDocument method called for DocumentId: {DocumentId}", documentId);
            try
            {
                var document = await _context.UploadedFiles.FindAsync(documentId);
                if (document == null)
                {
                    _logger.LogWarning("Document not found for DocumentId: {DocumentId}", documentId);
                    return NotFound("Document not found.");
                }
                _context.Remove(document);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Document Deleted successfully for DocumentId: {DocumentId}", documentId);
                //return Ok(new { Message = "Document Deleteted successfully." });
                return Ok("Document Deleteted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while Deleting document for DocumentId: {DocumentId}", documentId);
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("ResendDocument/{documentId}")]
        public async Task<IActionResult> ResendDocument(int documentId)
        {
            _logger.LogInformation("ResendDocument method called for DocumentId: {DocumentId}", documentId);
            try
            {
                var document = await _context.UploadedFiles.FindAsync(documentId);
                if (document == null)
                {
                    _logger.LogWarning("Document not found for DocumentId: {DocumentId}", documentId);
                    return NotFound("Document not found.");
                }

                document.Status = "Resend";
                document.DocumentModifiedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                var notification = new Notification
                {
                    UserId = document.UserId.ToString(),
                    Message = "Merchant has request some document you shared to upload again.",
                };
                _logger.LogInformation("Document marked for resending for DocumentId: {DocumentId}", documentId);
                //return Ok(new { Message = "Document marked for resending." });
                return Ok("Document marked for resending.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while resending document for DocumentId: {DocumentId}", documentId);
                return StatusCode(500, "Internal server error.");
            }
        }
        [HttpGet("GetServiceFileListByRFDFUId")]
        public async Task<IActionResult> GetServiceFileListByRFDFUId(int rfdfuId)
        {
            try
            {
                if (rfdfuId <= 0)
                {
                    return BadRequest("Invalid RFDFU ID.");
                }

                var result = await (from r in _context.RequestForDisCountToUsers
                                    join sm in _context.ServiceDocumentMapping on r.SID equals sm.ServiceID
                                    join sdl in _context.ServiceDocumentList on sm.ServiceDocumentListId equals sdl.ServiceDocumenListtId
                                    where r.RFDFU == rfdfuId
                                    select new
                                    {
                                        r.RFDFU,
                                        r.SID,
                                        r.MID,
                                        r.UID,
                                        sdl.ServiceDocumentName,
                                        sdl.ServiceDocumenListtId
                                    }).ToListAsync();

                if (result == null || !result.Any())
                {
                    return NotFound("No data found for the given RFDFU ID.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching discount data by RFDFU ID.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An internal server error occurred.");
            }
        }

        private void NotifyMerchant(int userId)
        {
            // Fetch merchant details based on userId or other logic
            var merchantEmail = "merchant@example.com"; // Replace with actual logic to get the merchant's email

            // Send email or notification
            SendEmail(merchantEmail, "Document Uploaded", "A new document has been uploaded. Please review it.");
        }
        private void SendEmail(string to, string subject, string body)
        {
            _emailService.SendEmail(to, subject, body, to.Split("@")[0]);
        }
    }


    public class FileUploadViewModel
    {
        [Required]
        public List<IFormFileCollection> Files { get; set; }
        public int UserId { get; set; }
        public List<UploadedFile>? UploadedFiles { get; set; }
    }

    public class FileUploadModelAPI
    {
        public int UserId { get; set; }
        public int MID { get; set; }
        public List<UploadedFile>? UploadedFiles { get; set; }
    }

    public class UserDocumentsViewModel
    {
        public int UserId { get; set; }
        public int MID { get; set; }
        public int DocumentCount { get; set; }
    }
}
