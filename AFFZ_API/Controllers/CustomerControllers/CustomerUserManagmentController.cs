using AFFZ_API.Interfaces;
using AFFZ_API.Models;
using AFFZ_API.Models.Partial;
using AFFZ_API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AFFZ_API.Controllers.CustomerControllers
{
    [Route("api/Customers")]
    [ApiController]
    public class CustomerUserManagmentController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly MyDbContext _context;
        public CustomerUserManagmentController(MyDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<SResponse>> Login(LoginModel loginDetail)
        {
            try
            {
                if(loginDetail == null)
                {
                    return new SResponse
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        Message = $"Please Fill Email and Password.",
                    };
                }
                string encryptedPassword = Cryptography.Encrypt(loginDetail.Password);
                Customers customerdetail = await _context.Customers.Where(x => x.Email == loginDetail.Email && x.Password == encryptedPassword).FirstOrDefaultAsync();
                if (customerdetail == null)
                {
                    return new SResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Your Email/Password is wrong!"
                    };
                }
                string temp = Cryptography.Decrypt(customerdetail.Password);

               

                return new SResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "User Login Success!",
                    Data = customerdetail,
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

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<SResponse>> PostCustomer(Customers customers, string referralCode = null)
        {
            try
            {
                customers.RoleId = _context.Roles.Where(x => x.RoleName.ToLower() == "customers").Select(x => x.RoleId).FirstOrDefault();
                customers.CreatedBy = 1;
                customers.CreatedDate = DateTime.Now;
                customers.Password = Cryptography.Encrypt(customers.Password); //need to encrypt
                _context.Customers.Add(customers);
                await _context.SaveChangesAsync();

                // Call stored procedure to update referral table if referral code is provided
                if (!string.IsNullOrEmpty(referralCode))
                {
                    var commandText = "EXEC UpdateReferralOnSignup @ReferredCustomerID, @ReferralCode";
                    var referredCustomerIdParam = new SqlParameter("@ReferredCustomerID", customers.CustomerId);
                    var referralCodeParam = new SqlParameter("@ReferralCode", referralCode);

                    await _context.Database.ExecuteSqlRawAsync(commandText, referredCustomerIdParam, referralCodeParam);
                }

                bool res = await _emailService.SendEmail(customers.Email, "Welcome On Board with Smart Center", "You have successfully signed up. Please remember your password for future reference and make sure to bookmark the website for future.", customers.CustomerName);

                return new SResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = $"Customer successfully registered!",
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


        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customers>>> GetCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customers>> GetCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customers customers)
        {
            if (id != customers.CustomerId)
            {
                return BadRequest();
            }

            _context.Entry(customers).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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

        [HttpPost]
        [Route("UpdateProfile")]
        public async Task<SResponse> UpdateProfile([FromBody] Customers model)
        {
            try
            {
                var existingProfile = _context.Customers.Where(x => x.CustomerId == model.CustomerId).FirstOrDefault();



                if (existingProfile != null)
                {
                    existingProfile.FirstName = model.FirstName;
                    existingProfile.LastName = model.LastName;
                    existingProfile.PhoneNumber = model.PhoneNumber;
                    existingProfile.Address = model.Address;
                    existingProfile.DOB = model.DOB;
                    existingProfile.PostalCode = model.PostalCode;
                    existingProfile.ProfilePicture = model.ProfilePicture;
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


        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }



    }
}
