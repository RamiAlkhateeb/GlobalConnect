using Domain.Models;
using GlobalConnect.Application.Modules.Booking.DTOs;
using GlobalConnect.Application.Modules.Booking.Interfaces;
using GlobalConnect.Domain.Enums;
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

        // --- IMPLEMENTATION ---
        public async Task<List<BookingDto>> GetSeekerBookingsAsync(int seekerId)
        {
            // Fetch bookings + Provider details + Timezone
            var bookings = await _context.Appointments
                .Include(b => b.Slot)
                    .ThenInclude(s => s.Provider) // To get Provider Name
                .Where(b => b.SeekerId == seekerId)
                .OrderByDescending(b => b.Slot.SlotStartUTC)
                .ToListAsync();

            // In a real app, we would convert UTC to the Seeker's stored Timezone here
            // For simplicity, we are returning UTC
            return bookings.Select(b => new BookingDto
            {
                AppointmentId = b.Id,
                OtherPartyName = b.Slot.Provider.Name,
                StartTimeLocal = b.Slot.SlotStartUTC, // TODO: Convert to Local
                Status = b.Status.ToString(),
                //PricePaid = b.AmountPaidUSD
            }).ToList();
        }

        public async Task<List<BookingDto>> GetProviderAppointmentsAsync(int providerUserId)
        {
            // Complex Query: Find bookings where the Slot belongs to the Provider
            var bookings = await _context.Appointments
                .Include(b => b.Seeker) // To get Seeker Name
                .Include(b => b.Slot)
                .Where(b => b.Slot.Provider.UserId == providerUserId)
                .OrderByDescending(b => b.Slot.SlotStartUTC)
                .ToListAsync();

            return bookings.Select(b => new BookingDto
            {
                AppointmentId = b.Id,
                OtherPartyName = b.Seeker.Email, // Using Email as Name for now
                StartTimeLocal = b.Slot.SlotStartUTC,
                Status = b.Status.ToString(),
                //PricePaid = b.AmountPaidUSD
            }).ToList();
        }

        public async Task CompleteAppointmentAsync(int appointmentId, int requesterId)
        {
            var booking = await _context.Appointments
                .Include(b => b.Slot)
                    .ThenInclude(s => s.Provider)
                .FirstOrDefaultAsync(b => b.Id == appointmentId);

            if (booking == null) throw new DomainException("Appointment not found.");

            // Security: Only the Provider can mark it as complete
            if (booking.Slot.Provider.UserId != requesterId)
                throw new DomainException("Only the provider can complete this appointment.");

            booking.Status = AppointmentStatus.Completed.ToString();
            await _context.SaveChangesAsync();
        }
    }
}