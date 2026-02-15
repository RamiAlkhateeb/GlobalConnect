using GlobalConnect.Application.Modules.Provider.DTOs;
using GlobalConnect.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicController : ControllerBase
    {
        private readonly GlobalConnectDbContext _context;

        public PublicController(GlobalConnectDbContext context)
        {
            _context = context;
        }

        [HttpGet("provider/{id}")]
        public async Task<IActionResult> GetProviderDetails(int id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id && u.Role == "Provider" && u.IsActive)
                .Select(u => new ProviderDetailDto
                {
                    Name = u.Name,
                    Specialty = u.Specialty,
                    Bio = u.Bio,
                    Nationality = u.Nationality, // Ensure this exists in your Entity/DTO
                    PhotoUrl = u.PhotoUrl,       // Ensure this exists
                    GoogleBookingUrl = u.GoogleBookingUrl
                })
                .FirstOrDefaultAsync();

            if (user == null) return NotFound(new { message = "Provider not found" });

            return Ok(user);
        }

        [HttpGet("providers")]
        public async Task<IActionResult> GetAllActiveProviders([FromQuery] string? search)
        {
            // Filter: Only Providers, Only Active (Approved)
            var query = _context.Users
                .Where(u => u.Role == "Provider" && u.IsActive);

            // Optional Search Logic
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(u => u.Name.Contains(search) || u.Specialty.Contains(search));
            }

            var providers = await query
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.Specialty,
                    u.PhotoUrl, // Ensure you return the photo URL
                    u.Nationality,
                    u.Bio
                })
                .ToListAsync();

            return Ok(providers);
        }
    }
}
