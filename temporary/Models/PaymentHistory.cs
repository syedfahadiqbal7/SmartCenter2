namespace temporary.Models;

public partial class PaymentHistory
{
    public int Id { get; set; }

    public string? Paymenttype { get; set; }

    public string? Amount { get; set; }

    public int? Payerid { get; set; }

    public int? Merchantid { get; set; }

    public int? Ispaymentsuccess { get; set; }

    public int? Serviceid { get; set; }

    public DateTime? Paymentdatetime { get; set; }
}
