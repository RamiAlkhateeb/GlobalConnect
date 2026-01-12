using Application.Modules.Identity.Interfaces;
using GlobalConnect.Application.Modules.Identity.DTOs;
using GlobalConnect.Domain.Models;
using GlobalConnect.Infrastructure.Data;
using GlobalConnect.Infrastructure.Services;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly GlobalConnectDbContext _context;
        private readonly IJwtTokenGenerator _tokenGenerator;

        public AuthService(GlobalConnectDbContext context, IJwtTokenGenerator tokenGenerator)
        {
            _context = context;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<string> RegisterAsync(RegisterRequest dto)
        {
            if (_context.Users.Any(u => u.Email == dto.Email))
                throw new Exception("Email already exists.");

            var user = new User
            {
                Email = dto.Email,
                Name = dto.Name,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "Provider",
                IsActive = false // Explicitly inactive
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return "Registration successful. Please wait for Admin approval.";
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequest dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Invalid credentials.");

            if (!user.IsActive && user.Role != "Admin")
                throw new Exception("Account is not active yet. Contact Admin.");

            // 4. USE YOUR GENERATOR HERE
            var token = _tokenGenerator.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                Role = user.Role,
                Email = user.Email
            };
        }

     
    }
}
