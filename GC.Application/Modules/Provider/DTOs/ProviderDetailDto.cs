using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalConnect.Application.Modules.Provider.DTOs
{
    public class ProviderDetailDto
    {
        public int ProviderId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; } // Coming from the User table
        public string PhotoUrl { get; set; }
        public string GoogleBookingUrl { get; set; }
        public string Specialty { get; set; }
        public string Description { get; set; }
        public string Nationality { get; set; }
        public decimal HourlyRateUSD { get; set; }
        public string Timezone { get; set; } // Coming from the User table
        public List<string> Languages { get; set; } // Simplified list of codes
    }

    // Input for the search bar
    public class ProviderSearchQuery
    {
        public string? Specialty { get; set; }
        public string? Language { get; set; }
    }
}
