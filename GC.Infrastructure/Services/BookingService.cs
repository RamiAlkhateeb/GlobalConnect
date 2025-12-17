using Domain.Models;
using GlobalConnect.Application.Modules.Booking.DTOs;
using GlobalConnect.Application.Modules.Booking.Interfaces;
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
    public class BookingService : IBookingService
    {
        private readonly GlobalConnectDbContext _context;

        public BookingService(GlobalConnectDbContext context)
        {
            _context = context;
        }

        // API 4: Atomic Booking Transaction
        public async Task<int> CreateBookingAsync(int seekerId, CreateBookingDto request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Lock & Validate Slot
                // Use a proper locking strategy or check concurrency token in production
                var slot = await _context.GeneratedSlots
                    .FirstOrDefaultAsync(s => s.Id == request.SlotId);

                if (slot == null) throw new DomainException("Slot not found.");
                if (slot.IsBooked) throw new DomainException("Slot is already booked.");

                // 2. Mark as Booked
                slot.IsBooked = true;
                _context.GeneratedSlots.Update(slot);

                // 3. Create Appointment
                var appointment = new Appointment
                {
                    SlotId = slot.Id,
                    SeekerId = seekerId,
                    ProviderId = slot.ProviderId,
                    Status = "Confirmed",
                    PaymentTransactionId = "tok_" + Guid.NewGuid(), // Mock Payment
                    BookingTimestampUTC = DateTime.UtcNow
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                // 4. Commit
                await transaction.CommitAsync();

                return appointment.Id;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // API 5: Cancellation
        public async Task CancelBookingAsync(int userId, int bookingId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var appointment = await _context.Appointments
                    .Include(a => a.Slot)
                    .FirstOrDefaultAsync(a => a.Id == bookingId);

                if (appointment == null) throw new DomainException("Booking not found.");

                // Validate ownership
                if (appointment.SeekerId != userId && appointment.ProviderId != userId)
                    throw new DomainException("Unauthorized.");

                // 1. Update Status
                appointment.Status = "Cancelled";

                // 2. Free up the slot
                appointment.Slot.IsBooked = false;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}