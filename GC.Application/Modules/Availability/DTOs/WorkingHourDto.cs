using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalConnect.Application.Modules.Availability.DTOs
{
    public class WorkingHourDto
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTimeLocal { get; set; }
        public TimeSpan EndTimeLocal { get; set; }
    }
}
