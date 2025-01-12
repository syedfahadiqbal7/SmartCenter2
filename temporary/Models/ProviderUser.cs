﻿namespace temporary.Models;

public partial class ProviderUser
{
    public int ProviderId { get; set; }

    public string? ProviderName { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? RoleId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? ModifyDate { get; set; }

    public int? ModifiedBy { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string? PostalCode { get; set; }

    public DateTime? MemberSince { get; set; }

    public string? ProfilePicture { get; set; }

    public virtual Role? Role { get; set; }
}
