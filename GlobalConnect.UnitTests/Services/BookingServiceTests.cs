using Domain.Models;
using GlobalConnect.Application.Modules.Booking.DTOs;
using GlobalConnect.Domain.Exceptions;
using GlobalConnect.Infrastructure.Services;
using GlobalConnect.UnitTests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalConnect.UnitTests.Services
{
    public class BookingServiceTests
    {
        [Fact]
        public async Task CreateBookingAsync_ShouldFail_WhenSlotIsAlreadyBooked()
        {
            // Arrange
            using var context = DbContextFactory.Create();
            var service = new BookingService(context);

            // Setup: A slot that is ALREADY booked
            var slot = new GeneratedSlot { Id = 50, ProviderId = 1, IsBooked = true };
            context.GeneratedSlots.Add(slot);
            await context.SaveChangesAsync();

            var request = new CreateBookingDto { SlotId = 50, PaymentToken = "tok_123" };

            // Act & Assert
            // Expect a DomainException saying "Slot is already booked"
            await Assert.ThrowsAsync<DomainException>(() => service.CreateBookingAsync(99, request));
        }

        [Fact]
        public async Task CreateBookingAsync_ShouldSucceed_WhenSlotIsFree()
        {
            // Arrange
            using var context = DbContextFactory.Create();
            var service = new BookingService(context);

            var slot = new GeneratedSlot { Id = 60, ProviderId = 1, IsBooked = false };
            context.GeneratedSlots.Add(slot);
            await context.SaveChangesAsync();

            var request = new CreateBookingDto { SlotId = 60, PaymentToken = "tok_123" };

            // Act
            var bookingId = await service.CreateBookingAsync(99, request);

            // Assert
            // 1. Booking ID returned
            Assert.True(bookingId > 0);

            // 2. Slot is now locked
            var dbSlot = context.GeneratedSlots.Find(60);
            Assert.True(dbSlot.IsBooked);

            // 3. Appointment created
            var appointment = context.Appointments.Find(bookingId);
            Assert.NotNull(appointment);
            Assert.Equal("Confirmed", appointment.Status);
        }
    }
}
