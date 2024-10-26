namespace AFFZ_API.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; }
        public int CustomerId { get; set; }
        public User User { get; set; }
        public string ReviewText { get; set; }
        public int Rating { get; set; }
        public DateTime ReviewDate { get; set; }
        public Customers CUser { get; set; }
    }
}
