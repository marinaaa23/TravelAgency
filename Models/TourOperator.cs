using System;
using System.ComponentModel.DataAnnotations;

namespace TravelAgency.Models
{
    public class TourOperator
    {
        [Key] 
        public int OperatorId { get; set; }

        [Required]
        public string? Name { get; set; }
        public string? ContactPerson { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}