using Application.Modules.Provider.DTOs;
using Domain.Models;
using GlobalConnect.Application.Modules.Provider.DTOs;
using GlobalConnect.Application.Modules.Provider.Interfaces;
using GlobalConnect.Domain.Exceptions;
using GlobalConnect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalConnect.Infrastructure.Services
{
    public class ProviderService : IProviderService
    {
        private readonly GlobalConnectDbContext _context;

        public ProviderService(GlobalConnectDbContext context)
        {
            _context = context;
        }

        public async Task<ProviderDetailDto?> GetProviderByIdAsync(int id)
        {
            // 1. Query the database efficiently
            var providerEntity = await _context.Providers
                .Include(p => p.User)                // Join with User table for Email/Timezone
                .Include(p => p.SupportedLanguages)  // Join with Languages table
                .FirstOrDefaultAsync(p => p.UserId == id);

            if (providerEntity == null) return null;

            // 2. Map Entity to DTO (Manual mapping for clarity)
            return new ProviderDetailDto
            {
                ProviderId = providerEntity.UserId,
                Name = providerEntity.Name,
                Email = providerEntity.User.Email, // Accessing joined User data
                Specialty = providerEntity.Specialty,
                Description = providerEntity.Description,
                HourlyRateUSD = providerEntity.HourlyRateUSD,
                Timezone = providerEntity.User.TimezoneId,
                
                // 3. Transform complex relation to simple list of strings
                Languages = providerEntity.SupportedLanguages
                    .Select(l => l.LanguageCode)
                    .ToList()
            };
        }

        public async Task UpdateProviderProfileAsync(int providerId, UpdateProviderDto dto)
        {
            // 1. Fetch the existing provider profile
            var provider = await _context.Providers
                .FirstOrDefaultAsync(p => p.UserId == providerId);

            if (provider == null)
                throw new DomainException("Provider profile not found.");

            // 2. Update fields
            provider.Name = dto.Name;
            provider.Specialty = dto.Specialty;
            provider.Description = dto.Description;
            provider.HourlyRateUSD = dto.HourlyRateUSD;
            
            foreach (var lang in dto.Languages)
            {
                if (string.IsNullOrWhiteSpace(lang) || lang.Length != 2)
                    throw new DomainException($"Invalid language code: {lang}");
                var existingLang = _context.ProviderLanguages.FirstOrDefault(l => l.LanguageCode == lang);
                if (existingLang == null)
                {
                    existingLang = new ProviderLanguage { 
                        LanguageCode = lang,
                        Provider = provider,
                        ProviderId = provider.UserId

                    };
                    _context.ProviderLanguages.Add(existingLang);
                    await _context.SaveChangesAsync();
                }
                //provider.SupportedLanguages.Add(existingLang);
            }

            // 3. Save changes
            _context.Providers.Update(provider);
            await _context.SaveChangesAsync();
        }

        // 1. SEARCH FOR PROVIDERS
        public async Task<List<ProviderDetailDto>> SearchProvidersAsync(ProviderSearchQuery query)
        {
            var allProviders = _context.Providers
                .Include(p => p.User) // Include User to get the Language
                .AsQueryable();

            if (!string.IsNullOrEmpty(query.Specialty))
            {
                allProviders = allProviders.Where(p => p.Specialty.Contains(query.Specialty));
            }

            if (!string.IsNullOrEmpty(query.Language))
            {
                allProviders = allProviders.Where(p => p.User.PreferredLanguage == query.Language);
            }

            return await allProviders
                .Select(p => new ProviderDetailDto
                {
                    ProviderId = p.UserId,
                    Name = p.Name,
                    Nationality = p.User.Nationality,
                    PhotoUrl = p.PhotoUrl,
                    Specialty = p.Specialty,
                    HourlyRateUSD = p.HourlyRateUSD,
                    Languages = p.SupportedLanguages.Select(l => l.LanguageCode).ToList()
                })
                .ToListAsync();
        }
    }
}
