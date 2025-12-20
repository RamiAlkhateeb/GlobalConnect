using GlobalConnect.Application.Modules.Availability.DTOs;
using GlobalConnect.Application.Modules.Availability.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GlobalConnect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvailabilityController : ControllerBase
    {
        private readonly IAvailabilityService _service;

        public AvailabilityController(IAvailabilityService service)
        {
            _service = service;
        }

        // Helper to extract the User ID from the JWT 'sub' or 'nameid' claim
        private int GetCurrentUserId()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier); // standard JWT 'sub' claim
            if (idClaim == null) throw new UnauthorizedAccessException("Invalid Token: Missing User ID.");
            return int.Parse(idClaim.Value);
        }

        // 1. Set Working Hours
        // Only a logged-in Provider can do this
        [Authorize(Roles = "Provider")]
        [HttpPut("working-hours")]
        public async Task<IActionResult> SetWorkingHours([FromBody] List<WorkingHourDto> hours)
        {
            try
            {
                int providerId = GetCurrentUserId(); // Secured ID
                                                     // Note: In our 1:1 schema, ProviderId IS the UserId.
                await _service.SetWorkingHoursAsync(providerId, hours);
                return Ok(new { message = "Schedule updated successfully." });
            }
            catch (Exception ex)
            {
                // In production, log this exception
                return BadRequest(new { error = ex.Message });
            }
            
        }

        // 2. Generate Slots
        // Only a logged-in Provider can trigger their own slot generation
        [Authorize(Roles = "Provider")]
        [HttpPost("generate")]
        public async Task<IActionResult> GenerateSlots()
        {
            int providerId = GetCurrentUserId(); // Secured ID
            await _service.GenerateSlotsAsync(providerId);
            return Ok(new { message = "Slots generated successfully." });
        }

        // 3. Search Providers
        // Allow Anonymous users? Or only Seekers? usually public is fine for search.
        [AllowAnonymous]
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] SearchRequestDto request)
        {
            var results = await _service.SearchProvidersAsync(request);
            return Ok(results);
        }

        // Step 2: Click & View Details
        // GET: /api/providers/5/slots?date=2025-10-10&timezone=Europe/Berlin
        [AllowAnonymous]
        [HttpGet("{id}/slots")]
        public async Task<IActionResult> GetSlots(int id, [FromQuery] SearchRequestDto request)
        {
            request.ProviderId = id;
            var slots = await _service.GetProviderSlotsAsync(request);
            return Ok(slots);
        }
    }
}
