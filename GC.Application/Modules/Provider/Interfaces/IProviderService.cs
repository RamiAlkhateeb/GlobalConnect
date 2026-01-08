using GlobalConnect.Application.Common.DTOs;
using GlobalConnect.Application.Modules.Provider.DTOs;

namespace GlobalConnect.Application.Modules.Provider.Interfaces
{
    public interface IProviderService
    {
        // Returns null if not found
        Task<ProviderDetailDto?> GetProviderByIdAsync(int id);
        Task UpdateProviderProfileAsync(int providerId, UpdateProviderDto dto);
        Task<List<ProviderDetailDto>> SearchProvidersAsync(string? query);
        Task<List<LanguageDto>> GetAllLanguagesAsync();
    }
}
