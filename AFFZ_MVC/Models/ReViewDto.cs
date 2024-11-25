﻿namespace AFFZ_Customer.Models
{
    public class ReViewDto
    {
        public int ReviewId { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; }
        public int CustomerId { get; set; }
        public string ReviewText { get; set; }
        public int Rating { get; set; }
        public DateTime ReviewDate { get; set; }
        public int merchantId { get; set; }
        public int RFDFU { get; set; }
        public Customers CUser { get; set; }
    }
}
