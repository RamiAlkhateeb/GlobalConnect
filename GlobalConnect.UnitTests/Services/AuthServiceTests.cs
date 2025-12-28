using Application.Modules.Identity.Interfaces;
using GlobalConnect.Application.Common.Interfaces;
using GlobalConnect.Application.Modules.Identity.DTOs;
using GlobalConnect.Domain.Exceptions;
using GlobalConnect.Domain.Models;
using GlobalConnect.Infrastructure.Data;
using GlobalConnect.Infrastructure.Services;
using GlobalConnect.UnitTests.Helpers;
using Google.Apis.Auth;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using Xunit;

namespace GlobalConnect.UnitTests.Services
{
    public class AuthServiceTests
    {
        [Fact]
        public async Task LoginWithGoogle_ShouldCreateUser_WhenUserDoesNotExist()
        {
            // Arrange
            var mockSet = new Mock<DbSet<User>>();
            var mockContext = new Mock<GlobalConnectDbContext>();
            mockContext.Setup(m => m.Users).Returns(mockSet.Object);

            var mockGoogle = new Mock<GoogleAuthService>(null); // Mocking dependencies
            var mockJwt = new Mock<IJwtTokenGenerator>();

            // Simulate Google validating successfully
            mockGoogle.Setup(x => x.ValidateAsync(It.IsAny<string>()))
                      .ReturnsAsync(new GoogleJsonWebSignature.Payload
                      {
                          Email = "newuser@test.com",
                          Subject = "12345"
                      });

            // Simulate JWT generation
            mockJwt.Setup(x => x.GenerateToken(It.IsAny<User>())).Returns("fake-jwt-token");

            var service = new AuthService(mockContext.Object, mockGoogle.Object, mockJwt.Object);

            // Act
            var result = await service.LoginWithGoogleAsync("valid-google-token");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("newuser@test.com", result.Email);
            Assert.Equal("fake-jwt-token", result.Token);

            // Verify that Add was called on the DbSet to save the new user
            mockSet.Verify(m => m.Add(It.IsAny<User>()), Times.Once());
        }


    }
}