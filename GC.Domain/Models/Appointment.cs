using GlobalConnect.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalConnect.Domain.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        public int SeekerId { get; set; }
        public virtual User Seeker { get; set; }

        public int ProviderId { get; set; }
        public virtual Provider Provider { get; set; }


        public string GoogleEventId { get; set; } //The unique ID of the event created in Google Calendar.
        public string Title { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Status { get; set; } = "Confirmed"; // Confirmed, Cancelled, Completed

        public DateTime ScheduledAt { get; set; }
        public DateTime EndTime { get; set; }
        public string? MeetingLink { get; set; } // The Google Meet link

    }
}