using System;
using System.ComponentModel.DataAnnotations;

namespace TravelAgency.Models
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? PassportNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        public string FullName => $"{FirstName} {LastName}";
    }
}