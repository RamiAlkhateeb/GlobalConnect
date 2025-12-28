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
        #region GenerateSlotsAsync Tests
        /*
        [Fact]
        public async Task GenerateSlotsAsync_ShouldCreateUtcSlots_FromLocalWorkingHours()
        {
            // Arrange
            using var context = DbContextFactory.Create();
            var service = new AvailabilityService(context);

            // Setup: Provider in New York (EST is UTC-5 usually)
            var user = new User { Id = 1, Email = "doc@ny.com", TimezoneId = "America/New_York" , PasswordHash= "123123123" , PreferredLanguage="en"};
            var provider = new Provider { UserId = 1, Name = "Dr. NY" , Description = "cool" , Specialty="test"};
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
        */
        #endregion


        
    }
}
