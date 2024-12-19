using AFFZ_API.Interfaces;
using AFFZ_API.Models;
using AFFZ_API.Models.Partial;
using AFFZ_API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AFFZ_API.Controllers.MerchantControllers
{
    [Route("api/Providers")]
    [ApiController]
    public class ProviderUsersController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ILogger<ProviderUsersController> _logger;

        public ProviderUsersController(MyDbContext context, IEmailService emailService, ILogger<ProviderUsersController> logger)
        {
            _context = context;
            _emailService = emailService;
            _logger = logger;
        }

        // GET: api/ProviderUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProviderUser>>> GetProviderUsers()
        {
            try
            {
                _logger.LogInformation("Fetching all provider users.");
                return await _context.ProviderUsers.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching provider users.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while fetching provider users.");
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<SResponse>> Login(LoginModel loginDetail)
        {
            try
            {
                _logger.LogInformation("Attempting login for email: {Email}", loginDetail.Email);

                // Validate input
                if (string.IsNullOrEmpty(loginDetail.Email) || string.IsNullOrEmpty(loginDetail.Password))
                {
                    _logger.LogWarning("Email or password is missing.");
                    return BadRequest(new SResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "Email and Password are required."
                    });
                }

                string encryptedPassword = Cryptography.Encrypt(loginDetail.Password);
                ProviderUser merchantDetail = await _context.ProviderUsers
                    .Where(x => x.Email == loginDetail.Email && x.Password == encryptedPassword)
                    .FirstOrDefaultAsync();

                if (merchantDetail == null)
                {
                    _logger.LogWarning("Login failed for email: {Email}", loginDetail.Email);
                    return new SResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Your Email/Password is incorrect."
                    };
                }

                _logger.LogInformation("User login successful for email: {Email}", loginDetail.Email);
                return new SResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "User Login Success!",
                    Data = merchantDetail,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred during login for email: {Email}", loginDetail.Email);
                return new SResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "An error occurred while processing your request. Please try again later."
                };
            }
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<SResponse>> PostMerchant(ProviderUser merchant)
        {
            try
            {
                _logger.LogInformation("Attempting to register new provider user with email: {Email}", merchant.Email);

                // Validate input
                if (string.IsNullOrEmpty(merchant.Email) || string.IsNullOrEmpty(merchant.Password) || string.IsNullOrEmpty(merchant.ProviderName))
                {
                    _logger.LogWarning("Missing required fields for registration.");
                    return BadRequest(new SResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "Email, Password, and Provider Name are required."
                    });
                }
                // Check if any existing merchant has the same EmiratesId, Trading License, Contact Number, or Company Registration Number
                var checkProvider = await _context.ProviderUsers
                    .FirstOrDefaultAsync(m => m.Email == merchant.Email ||
                                               m.PhoneNumber == merchant.PhoneNumber);
                if (checkProvider != null)
                {
                    return new SResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "A Provider with the same Email Address or Contact Number already exists.",
                    };
                }
                // Encrypt password before saving
                merchant.RoleId = _context.Roles.Where(x => x.RoleName.ToLower() == "merchant").Select(x => x.RoleId).FirstOrDefault();
                merchant.CreatedBy = 1;
                merchant.CreatedDate = DateTime.Now;
                merchant.Password = Cryptography.Encrypt(merchant.Password);

                _context.ProviderUsers.Add(merchant);
                await _context.SaveChangesAsync();

                bool emailSent = await _emailService.SendEmail(merchant.Email, "Welcome On Board with Smart Center", "You have successfully signed up. Please remember your password for future reference and make sure to bookmark the website for future.", merchant.ProviderName);
                if (!emailSent)
                {
                    _logger.LogWarning("Failed to send registration email to: {Email}", merchant.Email);
                }

                _logger.LogInformation("Provider user registered successfully with email: {Email}", merchant.Email);
                return new SResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Provider Successfully Registered!",
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred during registration for email: {Email}", merchant.Email);
                return new SResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "An error occurred while processing your request. Please try again later."
                };
            }
        }

        [HttpPost]
        [Route("UpdateProfile")]
        public async Task<SResponse> UpdateProfile([FromBody] ProviderUser model)
        {
            try
            {
                _logger.LogInformation("Attempting to update profile for provider ID: {ProviderId}", model.ProviderId);

                var existingProfile = await _context.ProviderUsers.FirstOrDefaultAsync(x => x.ProviderId == model.ProviderId);

                if (existingProfile == null)
                {
                    _logger.LogWarning("Provider profile not found for ID: {ProviderId}", model.ProviderId);
                    return new SResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Provider profile not found."
                    };
                }

                // Update profile details
                if (!string.IsNullOrEmpty(model.FirstName))
                {
                    existingProfile.FirstName = model.FirstName;
                }
                if (!string.IsNullOrEmpty(model.LastName))
                {
                    existingProfile.LastName = model.LastName;
                }
                if (!string.IsNullOrEmpty(model.PhoneNumber))
                {
                    existingProfile.PhoneNumber = model.PhoneNumber;
                }
                if (!string.IsNullOrEmpty(model.Address))
                {
                    existingProfile.Address = model.Address;
                }
                if (!string.IsNullOrEmpty(model.PostalCode))
                {
                    existingProfile.PostalCode = model.PostalCode;
                }
                if (!string.IsNullOrEmpty(model.ProfilePicture))
                {
                    existingProfile.ProfilePicture = model.ProfilePicture;
                }
                if (!string.IsNullOrEmpty(model.Passport))
                {
                    existingProfile.Passport = model.Passport;
                }
                if (!string.IsNullOrEmpty(model.EmiratesId))
                {
                    existingProfile.EmiratesId = model.EmiratesId;
                }
                if (!string.IsNullOrEmpty(model.DrivingLicense))
                {
                    existingProfile.DrivingLicense = model.DrivingLicense;
                }
                if (!string.IsNullOrEmpty(model.ProviderName))
                {
                    existingProfile.ProviderName = model.ProviderName;
                }
                await _context.SaveChangesAsync();

                _logger.LogInformation("Profile updated successfully for provider ID: {ProviderId}", model.ProviderId);
                return new SResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Profile Updated!",
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while updating profile for provider ID: {ProviderId}", model.ProviderId);
                return new SResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "An error occurred while processing your request. Please try again later."
                };
            }
        }
        [HttpPost("SaveMerchantDocument")]
        public IActionResult SaveMerchantDocument([FromBody] MerchantDocuments model)
        {
            try
            {
                if (model == null || model.MerchantId == 0)
                {
                    return BadRequest("Invalid merchant document information.");
                }

                _logger.LogInformation("Processing merchant document for Merchant ID: {MerchantId}", model.MerchantId);

                // Check if the document already exists (for update)
                var existingDocument = _context.MerchantDocuments.FirstOrDefault(d => d.MDID == model.MDID);

                if (existingDocument != null)
                {
                    // Update the existing document
                    _logger.LogInformation("Updating existing document with Document ID: {DocumentId}", model.MDID);

                    existingDocument.FileName = model.FileName;
                    existingDocument.ContentType = model.ContentType;
                    existingDocument.FileSize = model.FileSize;
                    existingDocument.FolderName = model.FolderName;
                    existingDocument.Status = model.Status;
                    existingDocument.DocumentAddedDate = model.DocumentAddedDate;
                    existingDocument.UploadedBy = model.UploadedBy;

                    _context.MerchantDocuments.Update(existingDocument);
                }
                else
                {
                    // Add new document
                    _logger.LogInformation("Adding new document for Merchant ID: {MerchantId}", model.MerchantId);
                    model.DocumentAddedDate = DateTime.Now;
                    _context.MerchantDocuments.Add(model);
                }

                _context.SaveChanges();

                _logger.LogInformation("Merchant document processed successfully for Merchant ID: {MerchantId}", model.MerchantId);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while processing merchant document for Merchant ID: {MerchantId}", model.MerchantId);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }
        // GET: api/ProviderUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProviderUser>> GetMerchantUser(int id)
        {
            try
            {
                _logger.LogInformation("Fetching provider user with ID: {ProviderId}", id);
                var merchantUser = await _context.ProviderUsers.FindAsync(id);

                if (merchantUser == null)
                {
                    _logger.LogWarning("Provider user not found with ID: {ProviderId}", id);
                    return NotFound();
                }

                _logger.LogInformation("Provider user fetched successfully with ID: {ProviderId}", id);
                return merchantUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while fetching provider user with ID: {ProviderId}", id);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }
        //UpdateDocumentStatus

        [HttpPost("UpdateDocumentStatus")]
        public async Task<IActionResult> UpdateDocumentStatus([FromBody] UpdateDocumentStatusRequest request)
        {
            try
            {
                int id = Convert.ToInt32(request.MDid);

                _logger.LogInformation("Fetching MerchantDocuments details with ID: {id}", id);
                var merchantDocs = await _context.MerchantDocuments.FindAsync(id);

                if (merchantDocs == null)
                {
                    _logger.LogWarning("Provider Document not found with ID: {id}", id);
                    return NotFound();
                }

                merchantDocs.Status = request.Status;

                await _context.SaveChangesAsync(); // Save changes to update the status in the database
                _logger.LogInformation("Provider Document fetched and updated successfully with ID: {id}", id);

                return Ok("Status Updated Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while updating provider document with ID: {id}", request.MDid);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }
        // GET: api/ProviderUsers/5
        [HttpGet("FetchMerchantVerificationDocumentList")]
        public async Task<IActionResult> FetchMerchantVerificationDocumentList()
        {
            try
            {
                var merchantDocsList = await _context.MerchantVerificationDocumentList.ToListAsync();

                if (merchantDocsList == null)
                {
                    return NotFound();
                }

                return Ok(merchantDocsList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }

        // PUT: api/ProviderUsers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMerchantUser(int id, ProviderUser merchantUser)
        {
            if (id != merchantUser.ProviderId)
            {
                _logger.LogWarning("Provider ID mismatch. Provided ID: {Id}, Merchant ID: {MerchantId}", id, merchantUser.ProviderId);
                return BadRequest("Provider ID mismatch.");
            }

            _context.Entry(merchantUser).State = EntityState.Modified;

            try
            {
                _logger.LogInformation("Updating provider user with ID: {ProviderId}", id);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Provider user updated successfully with ID: {ProviderId}", id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!MerchantUserExists(id))
                {
                    _logger.LogWarning("Provider user not found for update with ID: {ProviderId}", id);
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, "Concurrency exception occurred while updating provider user with ID: {ProviderId}", id);
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while updating provider user with ID: {ProviderId}", id);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }

            return NoContent();
        }

        // DELETE: api/ProviderUsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMerchantUser(int id)
        {
            try
            {
                _logger.LogInformation("Deleting provider user with ID: {ProviderId}", id);
                var merchantUser = await _context.ProviderUsers.FindAsync(id);
                if (merchantUser == null)
                {
                    _logger.LogWarning("Provider user not found for deletion with ID: {ProviderId}", id);
                    return NotFound();
                }

                _context.ProviderUsers.Remove(merchantUser);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Provider user deleted successfully with ID: {ProviderId}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while deleting provider user with ID: {ProviderId}", id);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }
        // Additionally, implement an endpoint in your API to handle the linking of the provider to a merchant
        // Update the LinkProviderToMerchant endpoint to check if Merchant already exists and linked
        [HttpPost("ProviderMerchantLink")]
        public async Task<IActionResult> LinkProviderToMerchant([FromBody] Merchant providerMerchant, int providerId, int merchantId)
        {
            try
            {
                if (providerMerchant == null || providerId == 0)
                {
                    return BadRequest("Invalid provider or merchant information.");
                }
                //Check Provider Details:
                var existingProfile = await _context.ProviderUsers.FirstOrDefaultAsync(x => x.ProviderId == providerId);

                if (existingProfile == null)
                {
                    _logger.LogWarning("Provider profile not found for ID: {ProviderId}", providerId);
                    return BadRequest("Provider profile not found..");

                }


                // Check if the merchant exists by MerchantID
                var existingMerchant = _context.Merchants.FirstOrDefault(m => m.MerchantId == merchantId);
                if (existingMerchant != null)
                {
                    // Update existing merchant details
                    existingMerchant.CompanyName = providerMerchant.CompanyName;
                    existingMerchant.ContactInfo = providerMerchant.ContactInfo;
                    existingMerchant.RegistrationMethod = providerMerchant.RegistrationMethod;
                    existingMerchant.IsActive = existingProfile.IsActive;
                    existingMerchant.CompanyRegistrationNumber = providerMerchant.CompanyRegistrationNumber;
                    existingMerchant.TradingLicense = providerMerchant.TradingLicense;
                    existingMerchant.EmiratesId = providerMerchant.EmiratesId;
                    existingMerchant.MerchantLocation = providerMerchant.MerchantLocation;
                    existingMerchant.ModifyDate = DateTime.Now;
                    existingMerchant.ModifiedBy = providerMerchant.ModifiedBy;
                }
                else
                {
                    // Add new merchant
                    // Validate required fields for a new merchant
                    if (string.IsNullOrEmpty(providerMerchant.EmiratesId) ||
                        string.IsNullOrEmpty(providerMerchant.TradingLicense) ||
                        string.IsNullOrEmpty(providerMerchant.ContactInfo) ||
                        string.IsNullOrEmpty(providerMerchant.CompanyRegistrationNumber))
                    {
                        return BadRequest("EmiratesId, Trading License, Contact Number, and Company Registration Number are required.");
                    }

                    // Check if any existing merchant has the same EmiratesId, Trading License, Contact Number, or Company Registration Number
                    var checkMerchant = await _context.Merchants
                        .FirstOrDefaultAsync(m => m.EmiratesId == providerMerchant.EmiratesId ||
                                                   m.TradingLicense == providerMerchant.TradingLicense ||
                                                   m.ContactInfo == providerMerchant.ContactInfo ||
                                                   m.CompanyRegistrationNumber == providerMerchant.CompanyRegistrationNumber);
                    if (checkMerchant != null)
                    {
                        return BadRequest("A merchant with the same EmiratesId, Trading License, Contact Number, or Company Registration Number already exists.");
                    }
                    providerMerchant.CreatedDate = DateTime.Now;
                    providerMerchant.ModifyDate = DateTime.Now;
                    providerMerchant.Deactivate = false;
                    _context.Merchants.Add(providerMerchant);
                    _context.SaveChanges();

                    // Update merchantId to reflect the newly added merchant's ID
                    merchantId = providerMerchant.MerchantId;
                }

                // Check if the provider is already linked to this merchant
                var existingProviderMerchant = _context.ProviderMerchant
                    .FirstOrDefault(pm => pm.ProviderID == providerId && pm.MerchantID == merchantId);
                if (existingProviderMerchant != null)
                {
                    // Update existing provider-merchant link
                    existingProviderMerchant.ModifiedDate = DateTime.Now;
                }
                else
                {
                    // Add new provider-merchant link
                    var newProviderMerchant = new ProviderMerchant
                    {
                        ProviderID = providerId,
                        MerchantID = merchantId,
                        IsActive = false, // Default to In active
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                    };
                    _context.ProviderMerchant.Add(newProviderMerchant);
                }

                // Save all changes
                _context.SaveChanges();

                return Ok("Provider and merchant linked successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while linking provider to merchant.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }
        [HttpGet("GetMerchantDocs/{MerchantId}")]
        public async Task<IActionResult> GetMerchantDocs(int MerchantId)
        {
            try
            {
                var ProviderDocs = await _context.MerchantDocuments.Where(x => x.MerchantId == MerchantId).ToListAsync();
                if (ProviderDocs == null)
                {
                    return NotFound("No data found for merchant.");
                }

                return Ok(ProviderDocs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching MerchantId Docs: {MerchantId}", MerchantId);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }

        // Implement an endpoint to get ProviderMerchant by ProviderID
        [HttpGet("GetByProvider/{providerId}")]
        public IActionResult GetProviderMerchantByProviderId(int providerId)
        {
            try
            {
                var providerMerchant = _context.ProviderMerchant.FirstOrDefault(pm => pm.ProviderID == providerId);
                if (providerMerchant == null)
                {
                    return NotFound("No merchant link found for the provided provider ID.");
                }

                return Ok(providerMerchant);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching provider-merchant link for provider ID: {ProviderId}", providerId);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }
        // Implement an endpoint to get ProviderMerchant by ProviderID
        [HttpGet("GetMerchantDetail/{MerchantId}")]
        public IActionResult GetMerchantDetail(int MerchantId)
        {
            try
            {
                var ProviderMerchant = _context.ProviderMerchant.Where(x => x.ProviderID == MerchantId).FirstOrDefault();
                if (ProviderMerchant == null)
                {
                    return NotFound("No Provider Merchant Link found for the provided provider ID.");
                }


                var merchant = _context.Merchants.Where(x => x.MerchantId == ProviderMerchant.MerchantID).FirstOrDefault();
                if (merchant == null)
                {
                    return NotFound("No merchant found for the provided provider ID.");
                }
                return Ok(merchant);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching MerchantId: {MerchantId}", MerchantId);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }
        private bool MerchantUserExists(int id)
        {
            return _context.ProviderUsers.Any(e => e.ProviderId == id);
        }
        [HttpPost("ToggleMerchantStatus")]
        public IActionResult ToggleMerchantStatus([FromBody] ToggleMerchantStatusRequest request)
        {
            try
            {
                var providerUsers = _context.ProviderUsers.FirstOrDefault(p => p.ProviderId == request.ProviderId);

                if (providerUsers == null)
                {
                    return NotFound("Merchant not found.");
                }

                providerUsers.IsActive = request.IsActive;
                providerUsers.ModifyDate = DateTime.Now; // Optional, if needed for auditing
                _context.SaveChanges();

                _logger.LogInformation("Merchant status updated successfully for Provider ID: {ProviderId}", request.ProviderId);

                //Update Provider Merchant.

                var providerMerchant = _context.ProviderMerchant.FirstOrDefault(p => p.ProviderID == request.ProviderId);

                if (providerMerchant == null)
                {
                    return NotFound("Provider and Merchant Link not found.");
                }

                providerMerchant.IsActive = request.IsActive;
                providerMerchant.ModifiedDate = DateTime.Now; // Optional, if needed for auditing
                _context.SaveChanges();

                _logger.LogInformation("Provider and Merchant Link status updated successfully for Provider ID: {ProviderId}", request.ProviderId);

                //Update Merchant Table Status.

                var merchant = _context.Merchants.FirstOrDefault(p => p.MerchantId == providerMerchant.MerchantID);

                if (merchant == null)
                {
                    return NotFound("Merchant not found.");
                }

                merchant.IsActive = request.IsActive;
                merchant.ModifyDate = DateTime.Now; // Optional, if needed for auditing
                _context.SaveChanges();

                _logger.LogInformation("Merchant status updated successfully for Provider ID: {ProviderId}", request.ProviderId);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling status for Provider ID: {ProviderId}", request.ProviderId);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while updating the status.");
            }
        }
    }
    public class UpdateDocumentStatusRequest
    {
        public string MDid { get; set; }
        public string Status { get; set; }
    }
    public class ToggleMerchantStatusRequest
    {
        public int ProviderId { get; set; }
        public bool IsActive { get; set; }
    }
}
