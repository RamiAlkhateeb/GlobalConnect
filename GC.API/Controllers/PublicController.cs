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
    }
}
