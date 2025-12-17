using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalConnect.Application.Modules.Availability.DTOs
{
    public class SearchRequestDto
    {
        public string? Language { get; set; }
        public string? Specialty { get; set; }
        public DateTime? AvailableDate { get; set; }
        public string SeekerTimezoneId { get; set; } // Required for converting results back
    }
}