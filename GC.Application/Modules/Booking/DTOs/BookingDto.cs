namespace GlobalConnect.Application.Modules.Booking.DTOs
{
    public class BookingDto
    {
        public int AppointmentId { get; set; }
        public string ProviderName { get; set; }
        public string ProviderSpecialty { get; set; }
        public string Title { get; set; }

        public DateTime EndTime { get; set; } // Provider Name or Seeker Name
        public DateTime StartTime { get; set; }
        public string Status { get; set; } // Confirmed, Completed, Cancelled
        public string GoogleEventId { get; set; } //The unique ID of the event created in Google Calendar.
        public string? MeetingLink { get; set; } // The Google Meet link

    }

    public class SyncRequest { public string AccessToken { get; set; } }
}
