using GlobalConnect.Application.Modules.Booking.DTOs;
using GlobalConnect.Application.Modules.Booking.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GlobalConnect.API.Controllers
{
    [Authorize] // Requires a valid JWT token for ALL methods
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _service;

        public BookingsController(IBookingService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyAppointments()
        {
            var userId = int.Parse(User.FindFirst("id").Value);
            return Ok(_service.GetMyAppointments(userId));
        }

        [HttpPost("sync")]
        public async Task<IActionResult> SyncWithGoogle([FromBody] SyncRequest request)
        {
            var userId = int.Parse(User.FindFirst("id").Value);
            var newCount = await _service.SyncWithGoogle(userId, request);
            return Ok(new { message = $"Synced {newCount} new appointments." });
        }

    }
}
