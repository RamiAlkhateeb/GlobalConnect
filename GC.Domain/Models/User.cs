using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace GlobalConnect.Domain.Models
{
    public class User
    {
        // 1. Internal Primary Key (Database Optimization)
        [Key]
        public int Id { get; set; }

        // 2. External ID (API Security - previously "Id")
        [Required]
        public Guid PublicId { get; set; } = Guid.NewGuid();


        [Required, MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? PhotoUrl { get; set; }

        [Required]
        public string GoogleId { get; set; } = string.Empty;
        public UserRole Role { get; set; }

        // Navigation properties
        public Provider? ProviderProfile { get; set; }
        public bool IsActive { get; set; } = false;

    }
}
