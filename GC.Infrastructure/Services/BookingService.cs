using GlobalConnect.Application.Modules.Booking.DTOs;
using GlobalConnect.Application.Modules.Booking.Interfaces;
using GlobalConnect.Domain.Enums;
using GlobalConnect.Domain.Exceptions;
using GlobalConnect.Domain.Models;
using GlobalConnect.Infrastructure.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
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
       
        public async Task<List<BookingDto>> GetMyAppointments(int userId)
        {
            var appointmentList = new List<BookingDto>();
            appointmentList = await _context.Appointments
                .Include(a => a.Provider)
                .Where(a => a.SeekerId == userId)
                .OrderByDescending(a => a.ScheduledAt)
                .Select(a => new BookingDto{
                    AppointmentId = a.Id,
                    ProviderName = a.Provider.Name,
                    ProviderSpecialty = a.Provider.Specialty,
                    StartTime = a.ScheduledAt,
                    EndTime = a.EndTime,
                    Status = a.Status
                })
                .ToListAsync();

            return appointmentList;
        }

        public async Task<int> SyncWithGoogle(int userId, SyncRequest request)
        {
            // 1. Setup Google Client with the User's Token
            var credential = GoogleCredential.FromAccessToken(request.AccessToken);
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "RemoteConsultation"
            });

            // 2. Fetch Events (Next 3 months)
            var eventsRequest = service.Events.List("primary");
            eventsRequest.TimeMin = DateTime.UtcNow;
            eventsRequest.TimeMax = DateTime.UtcNow.AddMonths(3);
            eventsRequest.SingleEvents = true;
            eventsRequest.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            var events = await eventsRequest.ExecuteAsync();

            // 3. Filter & Save
            // We look for events where the Title or Description contains our App Name or Provider Name
            // OR ideally, check attendees if you have access to that.
            // For this workaround, we will save ALL events that look like consultations.

            int newCount = 0;
            foreach (var eventItem in events.Items)
            {
                // Check if we already have this event
                bool exists = await _context.Appointments.AnyAsync(a => a.GoogleEventId == eventItem.Id);
                if (!exists)
                {
                    // Simple logic: Is this a relevant meeting? 
                    // You can improve this by checking attendees emails against your Provider DB

                    var appt = new Appointment
                    {
                        SeekerId = userId,
                        ProviderId = 1, // HARDCODED for now: You need logic to match email to ProviderId
                        GoogleEventId = eventItem.Id,
                        Title = eventItem.Summary ?? "Consultation",
                        ScheduledAt = eventItem.Start.DateTime ?? DateTime.Parse(eventItem.Start.Date),
                        EndTime = eventItem.End.DateTime ?? DateTime.Parse(eventItem.End.Date),
                    };

                    _context.Appointments.Add(appt);
                    newCount++;
                }
            }

            await _context.SaveChangesAsync();
            return newCount;
        }
    }
}