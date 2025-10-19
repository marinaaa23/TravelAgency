using System;
using System.ComponentModel.DataAnnotations;

namespace TravelAgency.Models
{
    public class Tour
    {
        [Key]
        public int TourId { get; set; }
        public int OperatorId { get; set; }

        [Required]
        public string? Title { get; set; }
        public string? Description { get; set; }

        [Required]
        public string? Destination { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        public int Capacity { get; set; }
        public int AvailableSpots { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public TourOperator? Operator { get; set; }
        public int Duration => (EndDate - StartDate).Days;
    }
}