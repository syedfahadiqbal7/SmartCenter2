using System.ComponentModel.DataAnnotations;

namespace AFFZ_Customer.Models;

public partial class PaymentHistory
{
    [Key]
    public int ID { get; set; }
    public string PAYMENTTYPE { get; set; }
    public string AMOUNT { get; set; }
    public string PAYERID { get; set; }
    public string MERCHANTID { get; set; }
    public int ISPAYMENTSUCCESS { get; set; }
    public string SERVICEID { get; set; }

    public DateTime PAYMENTDATETIME { get; set; }
}
