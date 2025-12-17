using GlobalConnect.Application.Modules.Booking.DTOs;
using GlobalConnect.Application.Modules.Booking.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        private int GetCurrentUserId()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (idClaim == null) throw new UnauthorizedAccessException("User ID not found in token.");
            return int.Parse(idClaim.Value);
        }

        // 4. Create Booking
        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto request)
        {
            int seekerId = GetCurrentUserId();
            try
            {
                var bookingId = await _service.CreateBookingAsync(seekerId, request);
                // Return 201 Created
                return CreatedAtAction(nameof(CreateBooking), new { id = bookingId }, new { bookingId, status = "Confirmed" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Cancel Booking
        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            int userId = GetCurrentUserId(); 
            try
            {
                await _service.CancelBookingAsync(userId, id);
                return Ok(new { message = "Booking cancelled successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
