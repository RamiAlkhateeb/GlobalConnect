
using Application.Modules.Admin.Interfaces;
using GlobalConnect.Application.Modules.Provider.DTOs;
using GlobalConnect.Infrastructure.Data;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GlobalConnect.Infrastructure.Services
{
    public class AdminService : IAdminService
    {
        private readonly GlobalConnectDbContext _context;

        public AdminService(GlobalConnectDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProviderDetailDto>> GetAllProvidersAsync()
        {
            return await _context.Users
                .Where(u => u.Role == "Provider")
                .Select(u => new ProviderDetailDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    MobileNumber = u.MobileNumber,
                    Specialty = u.Specialty,
                    IsActive = u.IsActive,
                    GoogleBookingUrl = u.GoogleBookingUrl,
                    PhotoUrl = u.PhotoUrl,
                    Bio = u.Bio,
                    Email = u.Email,
                })
                .ToListAsync();
        }

        public async Task ToggleProviderStatusAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new Exception("User not found");

            // Flip the status
            user.IsActive = !user.IsActive;
            await _context.SaveChangesAsync();
        }

        
    }
}
