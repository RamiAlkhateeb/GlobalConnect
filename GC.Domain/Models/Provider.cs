using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalConnect.Domain.Models
{
    public class Provider
    {
        [Key, ForeignKey("User")]
        public int UserId { get; set; }


        public string? GoogleRefreshToken { get; set; } //Used to get a new access token whenever your API needs to talk to Google.
        public string? GoogleBookingUrl { get; set; } = string.Empty; //The link to the provider’s public Google Appointment Schedule page.
        public string? GoogleCalendarId { get; set; } = "primary"; //Usually "primary", but used to identify which calendar to "Watch."

        [Required, MaxLength(255)]
        public string Name { get; set; }

        [Required, MaxLength(100)]
        public string Specialty { get; set; } // e.g., 'Psychotherapist', 'Legal Advisor'

        [Column(TypeName = "money")]
        public decimal HourlyRateUSD { get; set; }
        [MaxLength(1000)]
        public string Bio { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Nationality { get; set; } = string.Empty;

        // Navigation back to Parent
        public virtual User User { get; set; } = null!;

        // Many-to-Many Relationship with Languages
        public virtual ICollection<ProviderLanguage> ProviderLanguages { get; set; } = new List<ProviderLanguage>();

    }
}
