using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalConnect.Application.Modules.Availability.DTOs
{
    public class SearchResultDto
    {
        public int ProviderId { get; set; }
        public string ProviderName { get; set; }
        public decimal HourlyRate { get; set; }
        //public List<SlotDto> AvailableSlots { get; set; }
    }

    public class SlotDto
    {
        public int SlotId { get; set; }
        public DateTime StartUTC { get; set; }
        public DateTime EndUTC { get; set; }
        public DateTime StartLocal { get; set; } // Converted for the seeker
        public DateTime EndLocal { get; set; } // Converted for the seeker
        public bool IsBooked { get; set; }  

    }
}