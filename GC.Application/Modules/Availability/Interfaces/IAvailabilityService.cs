using GlobalConnect.Application.Modules.Availability.DTOs;
using GlobalConnect.Application.Modules.Provider.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalConnect.Application.Modules.Availability.Interfaces
{
    public interface IAvailabilityService
    {
        Task<ProviderDetailDto> GetProviderProfileAsync(int providerId);
        Task UpdateGoogleLinkAsync(int providerUserId, string bookingUrl);
    }
}
