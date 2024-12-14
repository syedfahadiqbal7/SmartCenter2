using System.ComponentModel.DataAnnotations;

namespace AFFZ_Provider.Models;

public partial class ProviderUser
{
    public int ProviderId { get; set; }
    [Required(ErrorMessage = "Company Name is required.")]
    public string ProviderName { get; set; } = null!;
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = null!;
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
    public string Password { get; set; } = null!;

    public int? RoleId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public DateOnly? DOB { get; set; }

    public DateTime? ModifyDate { get; set; }

    public int? ModifiedBy { get; set; }
    [Required(ErrorMessage = "First Name is required.")]
    public string? FirstName { get; set; }

    public string? LastName { get; set; }
    [Required(ErrorMessage = "Phone Number is required.")]
    public string? PhoneNumber { get; set; }

    public string? ProfilePicture { get; set; }

    public string? Address { get; set; }

    public string? PostalCode { get; set; }

    public string? Passport { get; set; }

    public string? EmiratesId { get; set; }

    public string? DrivingLicense { get; set; }

    public DateTime? MemberSince { get; set; }

    public IFormFile? ProfileImage { get; set; }

    public IFormFile? PassportFile { get; set; }

    public IFormFile? EmiratesIdFile { get; set; }

    public IFormFile? DrivingLicenseFile { get; set; }

    public virtual Role? Role { get; set; }

}
