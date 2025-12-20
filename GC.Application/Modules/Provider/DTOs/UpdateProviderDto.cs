using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modules.Provider.DTOs
{
    public class UpdateProviderDto
    {
        public string Name { get; set; }
        public string Specialty { get; set; }
        public string? Description { get; set; }
        public string? Email { get; set; } // Coming from the User table

        public decimal HourlyRateUSD { get; set; }
        public List<string> Languages { get; set; }
        public string? Timezone { get; set; } // Coming from the User table
        public string? PhotoUrl { get; set; }
    }
}
