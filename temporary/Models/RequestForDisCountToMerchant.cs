namespace temporary.Models;

public partial class RequestForDisCountToMerchant
{
    public int Rfdtm { get; set; }

    public int Sid { get; set; }

    public int Mid { get; set; }

    public int Uid { get; set; }

    public int? IsResponseSent { get; set; }

    public DateTime? RequestDateTime { get; set; }
}
