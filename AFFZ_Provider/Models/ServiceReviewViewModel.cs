namespace AFFZ_Provider.Models
{
    public class ServiceReviewViewModel
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public List<ReviewViewModel> Reviews { get; set; }
    }
}
