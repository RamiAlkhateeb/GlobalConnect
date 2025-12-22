using Domain.Models;
using GlobalConnect.Application.Common.Interfaces;
using GlobalConnect.Application.Modules.Booking.Interfaces;
using GlobalConnect.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhooksController : ControllerBase
    {
        private readonly IGoogleCalendarService _googleService;
        private readonly GlobalConnectDbContext _context;

        public WebhooksController(IGoogleCalendarService googleService, GlobalConnectDbContext context)
        {
            _googleService = googleService;
            _context = context;
        }

        [HttpPost("google")]
        public async Task<IActionResult> GoogleNotify()
        {
            // 1. Find the provider associated with this webhook resource
            var provider = await _context.Providers.FirstOrDefaultAsync(p => p.GoogleRefreshToken != null);

            // 2. Fetch the newly created event from Google
            var googleEvent = await _googleService.GetLatestEventAsync(provider.GoogleRefreshToken, "primary");

            if (googleEvent != null && !_context.Appointments.Any(b => b.GoogleEventId == googleEvent.Id))
            {
                // 3. Create a Pending Booking
                var booking = new Appointment
                {
                    ProviderId = provider.UserId,
                    //SeekerEmail = googleEvent.Attendees.FirstOrDefault(a => !a.ResponseStatus.Equals("declined"))?.Email ?? "Unknown",
                    GoogleEventId = googleEvent.Id,
                    Status = "Confirmed",
                    BookingTimeUtc = DateTime.UtcNow,
                };

                _context.Appointments.Add(booking);
                await _context.SaveChangesAsync();

                // 4. Send payment link to Seeker (Logic here)
            }

            return Ok();
        }
    }
}
