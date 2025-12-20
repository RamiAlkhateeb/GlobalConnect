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
        public string OtherPartyName { get; set; } // Provider Name or Seeker Name
        public DateTime StartTimeLocal { get; set; }
        public string Status { get; set; } // Confirmed, Completed, Cancelled
        public decimal PricePaid { get; set; }
    }
}
