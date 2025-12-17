using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalConnect.Application.Modules.Booking.DTOs
{
    public class CreateBookingDto
    {
        public int SlotId { get; set; }
        public string PaymentToken { get; set; }
    }
}