using AFFZ_API.Interfaces;
using AFFZ_API.Models;
using AFFZ_API.Models.Partial;
using AFFZ_API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

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
                existingProfile.FirstName = model.FirstName;
                existingProfile.LastName = model.LastName;
                existingProfile.PhoneNumber = model.PhoneNumber;
                existingProfile.Address = model.Address;
                existingProfile.PostalCode = model.PostalCode;
                existingProfile.ProfilePicture = model.ProfilePicture;
                existingProfile.Passport = model.Passport;
                existingProfile.EmiratesId = model.EmiratesId;
                existingProfile.DrivingLicense = model.DrivingLicense;

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
        public IActionResult LinkProviderToMerchant([FromBody] Merchant providerMerchant, int providerId)
        {
            try
            {
                if (providerMerchant == null || providerId == 0)
                {
                    return BadRequest("Invalid provider or merchant information.");
                }

                // Check if the merchant already exists and linked
                var existingProviderMerchant = _context.ProviderMerchant.FirstOrDefault(pm => pm.ProviderID == providerId);
                if (existingProviderMerchant != null)
                {
                    // Update existing link
                    existingProviderMerchant.ModifiedDate = DateTime.Now;
                    existingProviderMerchant.IsActive = false;
                    _context.SaveChanges();
                }
                else
                {
                    // Add new Merchant and link to provider
                    _context.Merchants.Add(providerMerchant);
                    _context.SaveChanges();

                    // Get the MerchantID of the saved merchant
                    int merchantId = providerMerchant.MerchantId;

                    ProviderMerchant _providerMerchant = new ProviderMerchant()
                    {
                        IsActive = false,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        ProviderID = providerId,
                        MerchantID = merchantId,
                    };
                    _context.ProviderMerchant.Add(_providerMerchant);
                    _context.SaveChanges();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while linking provider to merchant.");
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
                var merchant = _context.Merchants.Where(x => x.MerchantId == MerchantId).FirstOrDefault();
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
    }
}
