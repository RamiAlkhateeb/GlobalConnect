using GlobalConnect.Application.Common.DTOs;
using GlobalConnect.Application.Modules.Booking.Interfaces;
using GlobalConnect.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IWebHostEnvironment _env; // For file saving
        private readonly GlobalConnectDbContext _context; // Direct DB access for Reference data (shortcut)

        public DashboardController(IBookingService bookingService, IWebHostEnvironment env, GlobalConnectDbContext context)
        {
            _bookingService = bookingService;
            _env = env;
            _context = context;
        }

        // --- 1. SEEKER DASHBOARD ---
        // GET /api/appointments/seeker
        [Authorize(Roles = "Seeker")]
        [HttpGet("appointments/seeker")]
        public async Task<IActionResult> GetMyBookings()
        {
            int userId = int.Parse(User.FindFirst("sub").Value);
            var results = await _bookingService.GetSeekerBookingsAsync(userId);
            return Ok(results);
        }

        // --- 2. PROVIDER DASHBOARD ---
        // GET /api/appointments/provider
        [Authorize(Roles = "Provider")]
        [HttpGet("appointments/provider")]
        public async Task<IActionResult> GetMyAppointments()
        {
            int userId = int.Parse(User.FindFirst("sub").Value);
            var results = await _bookingService.GetProviderAppointmentsAsync(userId);
            return Ok(results);
        }

        // --- 3. COMPLETE APPOINTMENT ---
        // POST /api/appointments/5/complete
        [Authorize(Roles = "Provider")]
        [HttpPost("appointments/{id}/complete")]
        public async Task<IActionResult> CompleteAppointment(int id)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("sub").Value);
                await _bookingService.CompleteAppointmentAsync(id, userId);
                return Ok(new { message = "Appointment marked as completed." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


    }
}
