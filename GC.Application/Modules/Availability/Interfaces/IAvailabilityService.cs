using GlobalConnect.Application.Modules.Availability.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalConnect.Application.Modules.Availability.Interfaces
{
    public interface IAvailabilityService
    {
        Task SetWorkingHoursAsync(int providerId, List<WorkingHourDto> workingHours);
        Task GenerateSlotsAsync(int providerId, int daysToGenerate = 30);
        Task<List<SearchResultDto>> SearchProvidersAsync(SearchRequestDto request);
    }
}
