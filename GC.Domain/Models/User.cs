using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace GlobalConnect.Domain.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, MaxLength(255)]
        public string Email { get; set; }

        [Required]
        // In a real app, this should be handled by IdentityUser (ASP.NET Identity), 
        // but for a custom model, we store the hash.
        public string PasswordHash { get; set; }

        public bool IsProvider { get; set; } = false;

        [Required, MaxLength(10)]
        public string PreferredLanguage { get; set; } // e.g., 'en', 'de', 'es'

        [Required, MaxLength(100)]
        // Stores IANA Timezone ID, e.g., 'Europe/Berlin'
        public string TimezoneId { get; set; }

        // Navigation properties
        public Provider ProviderProfile { get; set; }
        public ICollection<Appointment> SeekerAppointments { get; set; }
        public bool IsActive { get; set; } = false;

        public string Nationality { get; set; }
    }
}
