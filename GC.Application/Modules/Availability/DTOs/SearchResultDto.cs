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
    }
    
}