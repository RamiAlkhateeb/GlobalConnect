using Domain.Enums;
using GlobalConnect.Application.Common.DTOs;
using GlobalConnect.Application.Modules.Provider.DTOs;
using GlobalConnect.Application.Modules.Provider.Interfaces;
using GlobalConnect.Domain.Models;
using GlobalConnect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GlobalConnect.Infrastructure.Services
{
    public class ProviderService : IProviderService
    {
        private readonly GlobalConnectDbContext _context;

        public ProviderService(GlobalConnectDbContext context)
        {
            _context = context;
        }

        public async Task<List<LanguageDto>> GetAllLanguagesAsync()
        {
            return await _context.Languages
                .Select(l => new LanguageDto { Id = l.LanguageId, Name = l.Name })
                .ToListAsync();
        }

        public async Task<ProviderDetailDto> GetProviderByIdAsync(int id)
        {
            // 1. Query the database efficiently
            var providerEntity = await _context.Providers
                .Include(p => p.ProviderLanguages)  // Join with Languages table
                .FirstOrDefaultAsync(p => p.UserId == id);

            if (providerEntity == null) return null;

            // 2. Map Entity to DTO (Manual mapping for clarity)
            return new ProviderDetailDto
            {
                ProviderId = providerEntity.UserId,
                Name = providerEntity.Name,
                Specialty = providerEntity.Specialty,
                Nationality = providerEntity.Nationality,
                Bio = providerEntity.Bio,
                GoogleBookingUrl = !string.IsNullOrEmpty(providerEntity.GoogleBookingUrl) ? providerEntity.GoogleBookingUrl : "",
                HourlyRateUSD = providerEntity.HourlyRateUSD,
                LanguageIds = _context.ProviderLanguages?
                    .Where(l => l.Language != null)
                    .Select(l => l.Language.LanguageId)
                    .ToList() ?? new List<int>(),
            };
        }

        public async Task UpdateProviderProfileAsync(int providerId, UpdateProviderDto dto)
        {
            // 1. Fetch the existing provider profile
            var user = await _context.Users
                .Include(u => u.ProviderProfile)
                .ThenInclude(p => p.ProviderLanguages)
                .FirstOrDefaultAsync(u => u.Id == providerId);

            if (user == null)
                throw new Exception("Provider profile not found.");

            // 1. Ensure Profile Exists
            if (user.ProviderProfile == null)
            {
                user.ProviderProfile = new Provider { UserId = providerId };
                // Also upgrade role to Provider if not already
                if (user.Role == UserRole.Seeker) user.Role = UserRole.Provider;
            }
            // 2. Update fields
            user.ProviderProfile.Name = dto.Name;
            user.ProviderProfile.Specialty = dto.Specialty;
            user.ProviderProfile.HourlyRateUSD = dto.HourlyRateUSD;
            user.ProviderProfile.Bio = !String.IsNullOrEmpty(dto.Bio) ? dto.Bio : "";
            user.ProviderProfile.Nationality = dto.Nationality;
            user.ProviderProfile.GoogleBookingUrl = dto.GoogleBookingUrl;
            // 3. Update Languages (Wipe and Replace Strategy)
            // Remove existing
            var currentLangs = user.ProviderProfile.ProviderLanguages.ToList();
            _context.ProviderLanguages.RemoveRange(currentLangs);

            // Add new
            foreach (var langId in dto.LanguageIds)
            {
                _context.ProviderLanguages.Add(new ProviderLanguage
                {
                    ProviderId = providerId,
                    LanguageId = langId
                });
            }
            await _context.SaveChangesAsync();
        }

        // 1. SEARCH FOR PROVIDERS
        public async Task<List<ProviderDetailDto>> SearchProvidersAsync(string? query)
        {
            var allProviders = _context.Providers
        .Include(p => p.ProviderLanguages)
        .ThenInclude(pl => pl.Language)
        .AsQueryable();

            // Optional: Filter by name or specialty if query is provided
            if (!string.IsNullOrWhiteSpace(query))
            {
                query = query.ToLower();
                allProviders = allProviders.Where(p =>
                    p.Nationality.ToLower().Contains(query) ||
                    p.Specialty.ToLower().Contains(query));
            }


            return await allProviders
                .Select(p => new ProviderDetailDto
                {
                    ProviderId = p.UserId,
                    Name = p.Name,
                    Nationality = p.Nationality,
                    Specialty = p.Specialty,
                    Bio = p.Bio,
                    PhotoUrl = p.User.PhotoUrl,
                    GoogleBookingUrl = !string.IsNullOrEmpty(p.GoogleBookingUrl) ? p.GoogleBookingUrl : "",
                    HourlyRateUSD = p.HourlyRateUSD,
                    LanguageIds = p.ProviderLanguages.Select(l => l.Language.LanguageId).ToList()
                })
                .ToListAsync();
        }

    }
}
