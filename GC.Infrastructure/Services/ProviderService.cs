using GlobalConnect.Application.Modules.Provider.DTOs;
using GlobalConnect.Application.Modules.Provider.Interfaces;
using GlobalConnect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
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
    }
}
