using System.ComponentModel.DataAnnotations;

namespace AFFZ_Provider.Models
{
    public class Service
    {
        [Key]
        public int ServiceId { get; set; }

        public int? CategoryID { get; set; }

        public int? MerchantID { get; set; }

        public int SID { get; set; }
        public string? ServiceName { get; set; }

        public string? Description { get; set; }

        public int? ServicePrice { get; set; }

        public virtual ServiceCategory? Category { get; set; }

        public virtual Merchant? Merchant { get; set; }
        public int ServiceAmountPaidToAdmin { get; set; }
        public string? SelectedDocumentIds { get; set; }
    }

}
