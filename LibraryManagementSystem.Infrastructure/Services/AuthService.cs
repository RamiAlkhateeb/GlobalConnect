using Application.Modules.Identity.DTOs;
using Application.Modules.Identity.Interfaces;
using Domain.Models;
using GlobalConnect.Domain.Models;
using GlobalConnect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly GlobalConnectDbContext _context;

        public AuthService(GlobalConnectDbContext context)
        {
            _context = context;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            // 1. Validation: Check if email already exists
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                throw new Exception("User with this email already exists.");
            }

            // 2. Create the User Entity
            var newUser = new User
            {
                Email = request.Email,
                // Security Note: In production, use BCrypt or Argon2. 
                // Using SHA256 here for simplicity in this demo.
                PasswordHash = HashPassword(request.Password),
                IsProvider = request.IsProvider,
                PreferredLanguage = request.PreferredLanguage,
                TimezoneId = request.TimezoneId
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

            // 5. Generate Response (Token logic usually goes here)
            return new AuthResponse
            {
                UserId = newUser.Id,
                Role = newUser.IsProvider ? "Provider" : "Seeker",
                Token = "simulated_jwt_token_" + Guid.NewGuid() // Replace with real JWT logic later
            };
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
