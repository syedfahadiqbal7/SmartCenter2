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

        public ProviderUsersController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/ProviderUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProviderUser>>> GetProviderUsers()
        {
            return await _context.ProviderUsers.ToListAsync();
        }



        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<SResponse>> Login(LoginModel loginDetail)
        {
            try
            {
                string encryptedPassword = Cryptography.Encrypt(loginDetail.Password);
                ProviderUser merchantDetail = await _context.ProviderUsers.Where(x => x.Email == loginDetail.Email && x.Password == encryptedPassword).FirstOrDefaultAsync();

                string temp = Cryptography.Decrypt(merchantDetail.Password);

                if (merchantDetail == null)
                {
                    return new SResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Your Email/Password is wrong!"
                    };
                }

                return new SResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "User Login Success!",
                    Data = merchantDetail,
                };
            }
            catch (Exception ex)
            {
                return new SResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = $"Exception: {ex.Message}",
                };
            }
        }


        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<SResponse>> PostMerchant(ProviderUser merchant)
        {
            try
            {
                merchant.RoleId = _context.Roles.Where(x => x.RoleName.ToLower() == "merchant").Select(x => x.RoleId).FirstOrDefault();
                merchant.CreatedBy = 1;
                merchant.CreatedDate = DateTime.Now;
                merchant.Password = Cryptography.Encrypt(merchant.Password); //need to encrypt
                _context.ProviderUsers.Add(merchant);
                await _context.SaveChangesAsync();

                return new SResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = $"Provider Successfully Registered!",
                };
            }
            catch (Exception ex)
            {
                return new SResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = $"Exception: {ex.Message}",
                };
            }
        }


        [HttpPost]
        [Route("UpdateProfile")]
        public async Task<SResponse> UpdateProfile([FromBody] ProviderUser model)
        {
            try
            {
                var existingProfile = _context.ProviderUsers.Where(x => x.ProviderId == model.ProviderId).FirstOrDefault();


                if (existingProfile != null)
                {
                    existingProfile.FirstName = model.FirstName;
                    existingProfile.LastName = model.LastName;
                    existingProfile.PhoneNumber = model.PhoneNumber;
                    existingProfile.Address = model.Address;
                    //existingProfile.DOB = model.DOB;
                    existingProfile.PostalCode = model.PostalCode;
                    existingProfile.ProfilePicture = model.ProfilePicture;
                    existingProfile.Passport = model.Passport;
                    existingProfile.EmiratesId = model.EmiratesId;
                    existingProfile.DrivingLicense = model.DrivingLicense;
                }


                await _context.SaveChangesAsync();
                return new SResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Profile Updated!",
                };
            }
            catch (Exception ex)
            {
                return new SResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = $"Exception: {ex.Message}",
                };
            }
        }





        // GET: api/ProviderUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProviderUser>> GetMerchantUser(int id)
        {
            var merchantUser = await _context.ProviderUsers.FindAsync(id);

            if (merchantUser == null)
            {
                return NotFound();
            }

            return merchantUser;
        }

        // PUT: api/ProviderUsers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMerchantUser(int id, ProviderUser merchantUser)
        {
            if (id != merchantUser.ProviderId)
            {
                return BadRequest();
            }

            _context.Entry(merchantUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MerchantUserExists(id))
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

        // POST: api/ProviderUsers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProviderUser>> PostMerchantUser(ProviderUser merchantUser)
        {
            _context.ProviderUsers.Add(merchantUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMerchantUser", new { id = merchantUser.ProviderId }, merchantUser);
        }

        // DELETE: api/ProviderUsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMerchantUser(int id)
        {
            var merchantUser = await _context.ProviderUsers.FindAsync(id);
            if (merchantUser == null)
            {
                return NotFound();
            }

            _context.ProviderUsers.Remove(merchantUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MerchantUserExists(int id)
        {
            return _context.ProviderUsers.Any(e => e.ProviderId == id);
        }
    }
}
