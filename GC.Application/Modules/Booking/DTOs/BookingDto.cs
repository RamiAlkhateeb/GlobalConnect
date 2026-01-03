using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalConnect.Application.Modules.Booking.DTOs
{
    public class BookingDto
    {
        public int AppointmentId { get; set; }
        public string ProviderName { get; set; }
        public string ProviderSpecialty { get; set; }

        public DateTime EndTime { get; set; } // Provider Name or Seeker Name
        public DateTime StartTime { get; set; }
        public string Status { get; set; } // Confirmed, Completed, Cancelled
    }

    public class SyncRequest { public string AccessToken { get; set; } }
}
