using System.ComponentModel.DataAnnotations;

namespace GlobalConnect.Domain.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; } = string.Empty; // Acts as Username

        public string MobileNumber { get; set; } = string.Empty; 

        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;

        public string Role { get; set; } = "Provider"; // "Admin" or "Provider"
        public bool IsActive { get; set; } = false; // Default: Inactive until approved

        // Profile Data (Merged here since no Provider model)
        public string Name { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string GoogleBookingUrl { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;


    }
}
