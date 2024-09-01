namespace AFFZ_API.Models;

public partial class Service
{
    public int ServiceId { get; set; }

    public int? CategoryId { get; set; }

    public int? MerchantId { get; set; }

    public string ServiceName { get; set; } = null!;

    public string? Description { get; set; }

    public int? ServicePrice { get; set; }

    public virtual ServiceCategory? Category { get; set; }

    public virtual Merchant? Merchant { get; set; }
}


public partial class SubCatPage
{
    public int? ServicePrice { get; set; }
    public int? CatId { get; set; }
    public string ServiceName { get; set; } = null!;
    public string Location { get; set; } = null!;
}