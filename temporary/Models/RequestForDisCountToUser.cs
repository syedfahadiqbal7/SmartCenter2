namespace temporary.Models;

public partial class RequestForDisCountToUser
{
    public int Sid { get; set; }

    public int Mid { get; set; }

    public int Finalprice { get; set; }

    public int Uid { get; set; }

    public int Rfdfu { get; set; }

    public DateTime? ResponseDateTime { get; set; }

    public int? IsMerchantSelected { get; set; }

    public int? IsPaymentDone { get; set; }
}
