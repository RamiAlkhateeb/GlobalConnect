using System.ComponentModel.DataAnnotations;

namespace GlobalConnect.Application.Modules.Identity.DTOs
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        public bool IsProvider { get; set; }

        [Required]
        public string PreferredLanguage { get; set; } // e.g., "en", "de"

        [Required]
        public string TimezoneId { get; set; } // e.g., "Europe/Berlin"
        [Required]
        public string Nationality { get; set; }
    }
}
