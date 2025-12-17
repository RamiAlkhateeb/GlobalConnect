using Domain.Models;
using GlobalConnect.Application.Modules.Availability.DTOs;
using GlobalConnect.Domain.Models;
using GlobalConnect.Infrastructure.Services;
using GlobalConnect.UnitTests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalConnect.UnitTests.Services
{
    public class AvailabilityServiceTests
    {
        [Fact]
        public async Task GenerateSlotsAsync_ShouldCreateUtcSlots_FromLocalWorkingHours()
        {
            // Arrange
            using var context = DbContextFactory.Create();
            var service = new AvailabilityService(context);

            // Setup: Provider in New York (EST is UTC-5 usually)
            var user = new User { Id = 1, Email = "doc@ny.com", TimezoneId = "America/New_York" };
            var provider = new Provider { UserId = 1, Name = "Dr. NY" };
            context.Users.Add(user);
            context.Providers.Add(provider);

            // Define Hours: Monday 9:00 AM Local
            context.WorkingHours.Add(new WorkingHour
            {
                ProviderId = 1,
                DayOfWeek = DateTime.UtcNow.DayOfWeek, // Make sure it matches today for the test loop
                StartTimeLocal = new TimeSpan(9, 0, 0), // 09:00 AM
                EndTimeLocal = new TimeSpan(11, 0, 0)   // 11:00 AM (2 hours = 2 slots)
            });
            await context.SaveChangesAsync();

            // Act
            await service.GenerateSlotsAsync(1, daysToGenerate: 1);

            // Assert
            var slots = context.GeneratedSlots.Where(s => s.ProviderId == 1).ToList();

            Assert.Equal(2, slots.Count); // Should generate 9-10 and 10-11

            // Check Timezone Conversion: 
            // If Local is 09:00 NY, UTC should be roughly 14:00 (assuming Standard Time)
            // We verify the offset exists.
            var firstSlot = slots.First();
            Assert.NotEqual(firstSlot.SlotStartUTC, DateTime.Today.AddHours(9)); // Should NOT be 9 AM UTC
        }

        [Fact]
        public async Task SearchProvidersAsync_ShouldConvertUtcToSeekerLocalTime()
        {
            // Arrange
            using var context = DbContextFactory.Create();
            var service = new AvailabilityService(context);

            // Data: Slot at 14:00 UTC
            var providerUser = new User { Id = 10, TimezoneId = "UTC" };
            var provider = new Provider { UserId = 10, Name = "Dr. Test", HourlyRateUSD = 100 };
            var slot = new GeneratedSlot
            {
                Id = 1,
                ProviderId = 10,
                SlotStartUTC = DateTime.Parse("2025-01-01T14:00:00Z"), // 2 PM UTC
                SlotEndUTC = DateTime.Parse("2025-01-01T15:00:00Z"),
                IsBooked = false
            };

            context.Users.Add(providerUser);
            context.Providers.Add(provider);
            context.GeneratedSlots.Add(slot);
            await context.SaveChangesAsync();

            // Seeker is in Berlin (UTC+1)
            var request = new SearchRequestDto { SeekerTimezoneId = "Europe/Berlin" };

            // Act
            var results = await service.SearchProvidersAsync(request);

            // Assert
            var resultSlot = results.First().AvailableSlots.First();

            // 14:00 UTC -> Should be 15:00 Berlin
            Assert.Equal(15, resultSlot.StartLocal.Hour);
        }
    }
}
