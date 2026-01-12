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



        public async Task<ProviderDetailDto> GetProviderByIdAsync(int id)
        {
            // 1. Query the database efficiently
            var providerEntity = await _context.Users
                .FirstOrDefaultAsync(p => p.Id == id);

            if (providerEntity == null) return null;

            // 2. Map Entity to DTO (Manual mapping for clarity)
            return new ProviderDetailDto
            {
                ProviderId = providerEntity.Id,
                Name = providerEntity.Name,
                Specialty = providerEntity.Specialty,
                Nationality = providerEntity.Nationality,
                Bio = providerEntity.Bio,
                GoogleBookingUrl = !string.IsNullOrEmpty(providerEntity.GoogleBookingUrl) ? providerEntity.GoogleBookingUrl : "",
                
            };
        }

        public async Task UpdateProviderProfileAsync(int providerId, UpdateProviderDto dto)
        {
            // 1. Fetch the existing provider profile
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == providerId);

            if (user == null)
                throw new Exception("Provider profile not found.");

            // 1. Ensure Profile Exists
            if (user == null)
            {
                user = new User { Id = providerId };
            }
            // 2. Update fields
            user.Name = dto.Name;
            user.Specialty = dto.Specialty;
            user.Bio = !String.IsNullOrEmpty(dto.Bio) ? dto.Bio : "";
            user.Nationality = dto.Nationality;
            user.GoogleBookingUrl = dto.GoogleBookingUrl;
            // 3. Update Languages (Wipe and Replace Strategy)
            // Remove existing

            
            await _context.SaveChangesAsync();
        }

        // 1. SEARCH FOR PROVIDERS
        public async Task<List<ProviderDetailDto>> SearchProvidersAsync(string? query)
        {
            var allProviders = _context.Users
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
                    ProviderId = p.Id,
                    Name = p.Name,
                    Nationality = p.Nationality,
                    Specialty = p.Specialty,
                    Bio = p.Bio,
                    PhotoUrl = p.PhotoUrl,
                    GoogleBookingUrl = !string.IsNullOrEmpty(p.GoogleBookingUrl) ? p.GoogleBookingUrl : "",
                })
                .ToListAsync();
        }

    }
}
