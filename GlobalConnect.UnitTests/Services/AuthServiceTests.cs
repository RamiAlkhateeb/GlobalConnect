using GlobalConnect.Application.Common.Interfaces;
using GlobalConnect.Application.Modules.Identity.DTOs;
using GlobalConnect.Domain.Exceptions;
using GlobalConnect.Domain.Models;
using GlobalConnect.Infrastructure.Services;
using GlobalConnect.UnitTests.Helpers;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace GlobalConnect.UnitTests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IPasswordHasher> _mockHasher;
        private readonly IConfiguration _configuration; // Needed for JWT Secret Key


        public AuthServiceTests(IConfiguration configuration)
        {
            _mockHasher = new Mock<IPasswordHasher>();
            _mockHasher.Setup(x => x.HashPassword(It.IsAny<string>())).Returns("hashed_secret");
            _configuration = configuration;
        }

        [Fact]
        public async Task RegisterAsync_ShouldCreateUserAndProvider_WhenRequestIsValidProvider()
        {
            // Arrange
            using var context = DbContextFactory.Create();
            var service = new AuthService(context, _mockHasher.Object, _configuration);

            var request = new RegisterRequest
            {
                Email = "doctor@test.com",
                Password = "Pass123!",
                IsProvider = true, // Crucial
                PreferredLanguage = "en",
                TimezoneId = "UTC"
            };

            // Act
            var result = await service.RegisterAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Provider", result.Role);

            // Verify Database State
            var savedUser = context.Users.SingleOrDefault(u => u.Email == request.Email);
            Assert.NotNull(savedUser);
            Assert.True(savedUser.IsProvider);

            // Verify Implicit Provider Creation (The "Zombie State" Prevention check)
            var savedProvider = context.Providers.SingleOrDefault(p => p.UserId == savedUser.Id);
            Assert.NotNull(savedProvider); // Provider profile must exist!
            Assert.Equal("New Provider", savedProvider.Name);
        }

        [Fact]
        public async Task RegisterAsync_ShouldThrowException_WhenEmailAlreadyExists()
        {
            // Arrange
            using var context = DbContextFactory.Create();
            context.Users.Add(new User { Email = "existing@test.com", PasswordHash = "x", PreferredLanguage = "en", TimezoneId = "UTC" });
            await context.SaveChangesAsync();

            var service = new AuthService(context, _mockHasher.Object, _configuration);
            var request = new RegisterRequest { Email = "existing@test.com", Password = "123", PreferredLanguage = "en", TimezoneId = "UTC" };

            // Act & Assert
            await Assert.ThrowsAsync<UserAlreadyExistsException>(() => service.RegisterAsync(request));
        }
    }
}