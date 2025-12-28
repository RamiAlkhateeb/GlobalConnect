using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public bool IsProvider { get; set; } = false;

        [Required]
        public string GoogleId { get; set; } = string.Empty;
        public UserRole Role { get; set; }

        // Navigation properties
        public Provider? ProviderProfile { get; set; }
        public bool IsActive { get; set; } = false;

    }
}
