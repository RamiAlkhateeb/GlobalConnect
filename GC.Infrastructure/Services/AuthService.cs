using Application.Modules.Identity.Interfaces;
using Domain.Enums;
using GlobalConnect.Application.Modules.Identity.DTOs;
using GlobalConnect.Domain.Models;
using GlobalConnect.Infrastructure.Data;
using GlobalConnect.Infrastructure.Services;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly GlobalConnectDbContext _context;
        private readonly GoogleAuthService _googleAuthService;
        private readonly IJwtTokenGenerator _jwtGenerator;

        public AuthService(GlobalConnectDbContext context, GoogleAuthService googleAuth, IJwtTokenGenerator jwtGenerator)
        {
            _context = context;
            _googleAuthService = googleAuth;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<AuthResponseDto> LoginWithGoogleAsync(string googleIdToken)
        {
            // 1. Validate Google Token
            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await _googleAuthService.ValidateAsync(googleIdToken);
            }
            catch
            {
                throw new Exception("Invalid Google Token");
            }

            // 2. Check if user exists
            var user = await _context.Users
                .Include(u => u.ProviderProfile)
                .FirstOrDefaultAsync(u => u.Email == payload.Email);

            // 3. Register if new
            if (user == null)
            {
                user = new User
                {
                    PublicId = Guid.NewGuid(),
                    Email = payload.Email,
                    GoogleId = payload.Subject,
                    Role = UserRole.Seeker, // Default to Client, can be changed later
                    PhotoUrl = payload.Picture
                };
                _context.Users.Add(user);
            }
            else
            {
                // OPTIONAL: Update photo if it changed on Google
                if (user.PhotoUrl != payload.Picture)
                {
                    user.PhotoUrl = payload.Picture;
                }
            }
            await _context.SaveChangesAsync();

            // 4. Generate JWT
            var token = _jwtGenerator.GenerateToken(user);

            return new AuthResponseDto
            {
                UserId = user.PublicId,
                Email = user.Email,
                Token = token,
                Role = user.Role.ToString()
            };
        }
    }
}
