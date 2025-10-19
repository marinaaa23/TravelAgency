using System;
using System.ComponentModel.DataAnnotations;

namespace TravelAgency.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }
        public int ClientId { get; set; }
        public int TourId { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Confirmed";
        public int NumberOfPersons { get; set; } = 1;
        public decimal TotalAmount { get; set; }
        public string? Notes { get; set; }

        public Client? Client { get; set; }
        public Tour? Tour { get; set; }
    }
}