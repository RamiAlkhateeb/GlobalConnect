using Application.Common.Interfaces;
using GlobalConnect.Application.Common.DTOs;
using GlobalConnect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ReferenceDataService : IReferenceDataService
    {
        private readonly GlobalConnectDbContext _context;

        public ReferenceDataService(GlobalConnectDbContext context)
        {
            _context = context;
        }
        public async Task<ReferenceDataDto> GetReferenceDataAsync()
        {
            // Fetch directly from DB using EF Core
            var specialties = LoadJson("Specialties.json");

            var languages = LoadJson("Languages.json");

            var nationalities = LoadJson("Nationalities.json");

            var referenceDataDto = new ReferenceDataDto
            {
                Specialties = specialties,
                Languages = languages,
                Nationalities = nationalities
            };

            return referenceDataDto;
        }

        private List<ReferenceItemDto> LoadJson(string fileName)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Data", "Static", fileName);
            if (!File.Exists(path)) return new List<ReferenceItemDto>();

            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<ReferenceItemDto>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        }
    }
}
