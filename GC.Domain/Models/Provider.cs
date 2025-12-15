using GlobalConnect.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Provider
    {
        // Primary Key is also a Foreign Key to the User table
        [Key, ForeignKey("User")]
        public int UserId { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }

        [Required, MaxLength(100)]
        public string Specialty { get; set; } // e.g., 'Psychotherapist', 'Legal Advisor'

        [Column(TypeName = "money")]
        public decimal HourlyRateUSD { get; set; }

        public string Description { get; set; }

        // Navigation properties
        public User User { get; set; } // Reference back to the User object
        public ICollection<ProviderLanguage> SupportedLanguages { get; set; }
        public ICollection<WorkingHour> WorkingHours { get; set; }
        public ICollection<GeneratedSlot> GeneratedSlots { get; set; }
    }
}
