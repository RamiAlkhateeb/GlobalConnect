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
        private readonly Mock<IConfiguration> _mockConfig; // 1. Create the Mock field

        public AuthServiceTests()
        {
            _mockHasher = new Mock<IPasswordHasher>();
            _mockHasher.Setup(x => x.HashPassword(It.IsAny<string>())).Returns("hashed_secret");
            _mockHasher.Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            // 2. Setup the Mock Configuration
            _mockConfig = new Mock<IConfiguration>();

            // This mocks: _configuration["JwtSettings:Key"]
            _mockConfig.Setup(c => c["JwtSettings:Key"]).Returns("test_key_must_be_32_characters_long_12345");
            _mockConfig.Setup(c => c["JwtSettings:Issuer"]).Returns("TestIssuer");
            _mockConfig.Setup(c => c["JwtSettings:Audience"]).Returns("TestAudience");
        }

        [Fact]
        public async Task RegisterAsync_ShouldCreateUserAndProvider_WhenRequestIsValidProvider()
        {
            // Arrange
            using var context = DbContextFactory.Create();
            var service = new AuthService(context, _mockHasher.Object, _mockConfig.Object);

            var request = new RegisterRequest
            {
                Email = "doctor@test.com",
                Password = "Pass123!",
                IsProvider = true, // Crucial
                PreferredLanguage = "en",
                Nationality = "Syria",
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
            context.Users.Add(new User {
                Email = "existing@test.com",
                Nationality = "USA",
                PasswordHash = "x", 
                PreferredLanguage = "en", 
                TimezoneId = "UTC" });
            await context.SaveChangesAsync();

            var service = new AuthService(context, _mockHasher.Object, _mockConfig.Object);
            var request = new RegisterRequest { Email = "existing@test.com", Password = "123", PreferredLanguage = "en", TimezoneId = "UTC" };

            // Act & Assert
            await Assert.ThrowsAsync<UserAlreadyExistsException>(() => service.RegisterAsync(request));
        }
    }
}