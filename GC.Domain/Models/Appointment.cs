using GlobalConnect.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        // Foreign key to the slot being booked (Crucial for concurrency)
        public int SlotId { get; set; }

        public int SeekerId { get; set; }

        public int ProviderId { get; set; }

        public string GoogleEventId { get; set; } //The unique ID of the event created in Google Calendar.
        //public DateTime PaymentDeadline { get; set; }  //When the user must pay by (e.g., CreatedAt + 15 mins).

        [Required, MaxLength(50)]
        public string Status { get; set; } = "Confirmed"; // Confirmed, Cancelled, Completed

        //[Required]
        //public string PaymentTransactionId { get; set; }

        [ForeignKey("SeekerId")]
        public User Seeker { get; set; }
        [ForeignKey("ProviderId")]
        public Provider Provider { get; set; }


        [Required]
        public DateTime BookingTimeUtc { get; set; } = DateTime.UtcNow;

        // Navigation properties
        //public GeneratedSlot Slot { get; set; }
    }
}