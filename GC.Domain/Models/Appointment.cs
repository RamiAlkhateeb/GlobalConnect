using GlobalConnect.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalConnect.Domain.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        public int SeekerId { get; set; }
        public User Seeker { get; set; }

        public int ProviderId { get; set; }
        public Provider Provider { get; set; }


        public string GoogleEventId { get; set; } //The unique ID of the event created in Google Calendar.
        //public DateTime PaymentDeadline { get; set; }  //When the user must pay by (e.g., CreatedAt + 15 mins).

        [Required, MaxLength(50)]
        public string Status { get; set; } = "Confirmed"; // Confirmed, Cancelled, Completed

        public DateTime ScheduledAt { get; set; }
        public string? MeetingLink { get; set; } // The Google Meet link

    }
}