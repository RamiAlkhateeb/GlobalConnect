using Domain.Models;
using GlobalConnect.Application.Modules.Availability.DTOs;
using GlobalConnect.Application.Modules.Availability.Interfaces;
using GlobalConnect.Domain.Exceptions;
using GlobalConnect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalConnect.Infrastructure.Services
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly GlobalConnectDbContext _context;

        public AvailabilityService(GlobalConnectDbContext context)
        {
            _context = context;
        }

        // API 1: Set Working Hours
        public async Task SetWorkingHoursAsync(int providerId, List<WorkingHourDto> workingHoursDto)
        {
            // 1. Remove existing hours (Simplest approach for update)
            var existing = _context.WorkingHours.Where(w => w.ProviderId == providerId);
            _context.WorkingHours.RemoveRange(existing);

            // 2. Add new hours
            var newHours = workingHoursDto.Select(dto => new WorkingHour
            {
                ProviderId = providerId,
                DayOfWeek = dto.DayOfWeek,
                StartTimeLocal = dto.StartTimeLocal,
                EndTimeLocal = dto.EndTimeLocal
            });

            _context.WorkingHours.AddRange(newHours);
            await _context.SaveChangesAsync();
        }

        // API 2: Generate Slots (The Timezone Logic)
        public async Task GenerateSlotsAsync(int providerId, int daysToGenerate = 30)
        {
            var provider = await _context.Providers
                .Include(p => p.User)
                .Include(p => p.WorkingHours)
                .FirstOrDefaultAsync(p => p.UserId == providerId);

            if (provider == null) throw new Exception("Provider not found");

            var providerTz = TimeZoneInfo.FindSystemTimeZoneById(provider.User.TimezoneId);
            var today = DateTime.UtcNow.Date; // Start generation from today

            // Clear future unbooked slots to avoid duplicates
            var oldSlots = _context.GeneratedSlots
                .Where(s => s.ProviderId == providerId && s.SlotStartUTC >= today && !s.IsBooked);
            _context.GeneratedSlots.RemoveRange(oldSlots);

            var newSlots = new List<GeneratedSlot>();

            for (int i = 0; i < daysToGenerate; i++)
            {
                var currentDate = today.AddDays(i);
                var dayOfWeek = currentDate.DayOfWeek;

                // Find working hours for this specific day
                var dayConfigs = provider.WorkingHours.Where(wh => wh.DayOfWeek == dayOfWeek);

                foreach (var config in dayConfigs)
                {
                    // Combine Date + Local Time
                    var rawStart = currentDate.Add(config.StartTimeLocal);
                    var rawEnd = currentDate.Add(config.EndTimeLocal);

                    // 2. STRIP THE KIND: Tell .NET this is "Unspecified" 
                    // This allows ConvertTimeToUtc to apply the provider's timezone rules correctly.
                    var startLocal = DateTime.SpecifyKind(rawStart, DateTimeKind.Unspecified);
                    var endLocal = DateTime.SpecifyKind(rawEnd, DateTimeKind.Unspecified);

                    // Convert to UTC (Handling DST automatically)
                    var startUtc = TimeZoneInfo.ConvertTimeToUtc(startLocal, providerTz);
                    var endUtc = TimeZoneInfo.ConvertTimeToUtc(endLocal, providerTz);

                    // Create 1-hour slots
                    var currentSlotStart = startUtc;
                    while (currentSlotStart < endUtc)
                    {
                        newSlots.Add(new GeneratedSlot
                        {
                            ProviderId = providerId,
                            SlotStartUTC = currentSlotStart,
                            SlotEndUTC = currentSlotStart.AddHours(1),
                            IsBooked = false
                        });
                        currentSlotStart = currentSlotStart.AddHours(1);
                    }
                }
            }

            _context.GeneratedSlots.AddRange(newSlots);
            await _context.SaveChangesAsync();
        }

        // API 3: Search
        public async Task<List<SearchResultDto>> SearchProvidersAsync(SearchRequestDto request)
        {
            // 1. Build Query
            var query = _context.GeneratedSlots
                .Include(s => s.Provider).ThenInclude(p => p.User)
                .Where(s => !s.IsBooked && s.SlotStartUTC > DateTime.UtcNow);

            if (request.AvailableDate.HasValue)
            {
                var date = request.AvailableDate.Value.Date;
                query = query.Where(s => s.SlotStartUTC >= date && s.SlotStartUTC < date.AddDays(1));
            }

            // 2. Fetch Data
            var slots = await query.ToListAsync();

            // 3. Transform & Group
            var seekerTz = TimeZoneInfo.FindSystemTimeZoneById(request.SeekerTimezoneId);

            var grouped = slots.GroupBy(s => s.Provider)
                .Select(g => new SearchResultDto
                {
                    ProviderId = g.Key.UserId,
                    ProviderName = g.Key.Name,
                    HourlyRate = g.Key.HourlyRateUSD,
                    AvailableSlots = g.Select(s => new SlotDto
                    {
                        SlotId = s.Id,
                        StartUTC = s.SlotStartUTC,
                        EndUTC = s.SlotEndUTC,
                        // Convert UTC back to Seeker's Local Time
                        StartLocal = TimeZoneInfo.ConvertTimeFromUtc(s.SlotStartUTC, seekerTz)
                    }).ToList()
                }).ToList();

            return grouped;
        }

        // 2. GET SLOTS FOR ONE PROVIDER
        public async Task<List<SlotDto>> GetProviderSlotsAsync(SearchRequestDto request)
        {
            // Find the provider's timezone to calculate availability correctly
            var providerTzId = await _context.Providers
                .Where(p => p.UserId == request.ProviderId)
                .Select(p => p.User.TimezoneId)
                .FirstOrDefaultAsync();

            if (providerTzId == null) throw new DomainException("Provider not found.");

            // Calculate UTC range for the requested day
            var seekerTz = TimeZoneInfo.FindSystemTimeZoneById(request.SeekerTimezoneId);
            // (Logic to fetch slots similar to previous implementation, but filtered by ProviderId)

            var slots = await _context.GeneratedSlots
                .Where(s => s.ProviderId == request.ProviderId
                            //&& !s.IsBooked
                            && s.SlotStartUTC >= DateTime.UtcNow) // Only future slots
                .ToListAsync();

            // Convert to Seeker's Time
            return slots.Select(s => new SlotDto
            {
                SlotId = s.Id,
                StartLocal = TimeZoneInfo.ConvertTimeFromUtc(s.SlotStartUTC, seekerTz),
                EndLocal = TimeZoneInfo.ConvertTimeFromUtc(s.SlotEndUTC, seekerTz),
                StartUTC = s.SlotStartUTC,
                EndUTC = s.SlotEndUTC,
                IsBooked = s.IsBooked
            }).ToList();
        }
    }
}