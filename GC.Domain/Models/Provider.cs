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

        public string? PhotoUrl { get; set; }
        public string? GoogleRefreshToken { get; set; } //Used to get a new access token whenever your API needs to talk to Google.
        public string? GoogleBookingUrl { get; set; } //The link to the provider’s public Google Appointment Schedule page.
        public string? GoogleCalendarId { get; set; } = "primary"; //Usually "primary", but used to identify which calendar to "Watch."

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
        //public ICollection<WorkingHour> WorkingHours { get; set; }
        //public ICollection<GeneratedSlot> GeneratedSlots { get; set; }

    }
}
