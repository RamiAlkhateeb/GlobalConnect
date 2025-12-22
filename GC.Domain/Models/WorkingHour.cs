using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    /*
    public class WorkingHour
    {
        [Key]
        public int Id { get; set; }

        public int ProviderId { get; set; }

        // Represents the day of the week (1=Monday, 7=Sunday)
        public DayOfWeek DayOfWeek { get; set; }

        // Time component only, stored in the provider's local timezone
        [Column(TypeName = "time")]
        public TimeSpan StartTimeLocal { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan EndTimeLocal { get; set; }

        // Navigation property
        public Provider Provider { get; set; }
    }
    */
}