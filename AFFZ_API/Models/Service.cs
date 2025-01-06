﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFFZ_API.Models;

public partial class Service
{
    [Key] // Specifies that this is the primary key
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ensure auto-generation
    public int ServiceId { get; set; } // Identity column, auto-generated by SQL Server
    [Required(ErrorMessage = "Service Category is required.")]
    public int? CategoryID { get; set; }

    public int? MerchantID { get; set; }
    [Required(ErrorMessage = "Service Title is required.")]
    public int SID { get; set; }

    public string? Description { get; set; }
    [Range(0, int.MaxValue, ErrorMessage = "Price must be a positive number.")]
    public int? ServicePrice { get; set; }

    public virtual ServiceCategory? Category { get; set; }

    public virtual Merchant? Merchant { get; set; }
    [Range(0, int.MaxValue, ErrorMessage = "Amount to Admin must be a positive number.")]
    public int ServiceAmountPaidToAdmin { get; set; }
    [Required(ErrorMessage = "Description is required.")]
    public string? SelectedDocumentIds { get; set; }

}


public partial class SubCatPage
{
    public int? ServicePrice { get; set; }
    public int? CatId { get; set; }
    public string ServiceName { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string? ServiceImage { get; set; } = null!;
}