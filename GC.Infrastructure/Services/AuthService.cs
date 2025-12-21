using Application.Modules.Identity.DTOs;
using Application.Modules.Identity.Interfaces;
using Domain.Models;
using GlobalConnect.Application.Common.Interfaces;
using GlobalConnect.Application.Modules.Identity.DTOs;
using GlobalConnect.Domain.Exceptions;
using GlobalConnect.Domain.Models;
using GlobalConnect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GlobalConnect.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly GlobalConnectDbContext _context;
        private readonly IPasswordHasher _passwordHasher; // Injected Dependency
        private readonly IConfiguration _configuration; // Needed for JWT Secret Key
        public AuthService(
            GlobalConnectDbContext context, 
            IPasswordHasher passwordHasher, 
            IConfiguration configuration
            )
        {
            _context = context;
            _passwordHasher = passwordHasher; // Dependency Inversion Principle applied
            _configuration = configuration;
        }

        // --- 1. LOGIN ---
        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            // 1. Find User
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                throw new DomainException("Invalid email or password.");
            }

            // 2. Verify Password
            bool isValid = _passwordHasher.VerifyPassword(request.Password, user.PasswordHash);

            if (!isValid)
            {
                throw new DomainException("Invalid email or password.");
            }

            // 3. Generate Token & Return
            var token = GenerateJwtToken(user);

            return new AuthResponse
            {
                UserId = user.Id,
                Email = user.Email,
                Role = user.IsProvider ? "Provider" : "Seeker",
                Token = token
            };
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            // 1. Validation: Check if email already exists
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                throw new UserAlreadyExistsException(request.Email);
            }

            // 2. Create the User Entity
            var newUser = new User
            {
                Email = request.Email,
                // Security Note: In production, use BCrypt or Argon2. 
                // Using SHA256 here for simplicity in this demo.
                PasswordHash = _passwordHasher.HashPassword(request.Password),
                IsProvider = request.IsProvider,
                PreferredLanguage = request.PreferredLanguage,
                TimezoneId = request.TimezoneId,
                Nationality = request.Nationality
            };

            // 3. Transaction: Ensure both User and Provider profile are created together
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync(); // Save to get the generated Id

                // 4. If Provider, create the placeholder Profile (The implicit creation we discussed)
                if (request.IsProvider)
                {
                    var providerProfile = new Provider
                    {
                        UserId = newUser.Id, // Link to the new user
                        Name = "New Provider", // Placeholder
                        Specialty = "Unspecified",
                        Description = "",
                        HourlyRateUSD = 0
                    };
                    _context.Providers.Add(providerProfile);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            // 4. Auto-Login (Generate Token)
            var token = GenerateJwtToken(newUser);

            // 5. Generate Response (Token logic usually goes here)
            return new AuthResponse
            {
                UserId = newUser.Id,
                Email = newUser.Email,
                Role = newUser.IsProvider ? "Provider" : "Seeker",
                Token = token // Replace with real JWT logic later
            };
        }


        // --- HELPER: JWT GENERATION ---
        private string GenerateJwtToken(User user)
        {
            var jwtKey = _configuration["JwtSettings:Key"] ?? "super_secret_key_must_be_long_enough_12345";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // IMPORTANT: Used by GetCurrentUserId()
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("role", user.IsProvider ? "Provider" : "Seeker"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"] ?? "GlobalConnectAPI",
                audience: _configuration["JwtSettings:Audience"] ?? "GlobalConnectClient",
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7), // Token valid for 1 week
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
