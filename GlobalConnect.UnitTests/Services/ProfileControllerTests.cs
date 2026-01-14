using GlobalConnect.API.Controllers;
using GlobalConnect.Application.Modules.Provider.DTOs;
using GlobalConnect.Application.Modules.Provider.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace GlobalConnect.UnitTests.Services
{
    public class ProfileControllerTests
    {
        private readonly Mock<IProviderService> _mockService;
        private readonly ProvidersController _controller;

        public ProfileControllerTests()
        {
            _mockService = new Mock<IProviderService>();
            _controller = new ProvidersController(_mockService.Object);

            // 1. Create a Robust Mock User
            var claims = new List<Claim>
    {
        new Claim("id", "55"),                        // Your custom ID used in Controller
        new Claim(ClaimTypes.NameIdentifier, "55"),   // Standard ASP.NET ID (Good practice)
        new Claim(ClaimTypes.Name, "Test Doctor"),    // User Name
        new Claim(ClaimTypes.Role, "Provider")        // Role (in case you check it)
    };

            // 2. IMPORTANT: The second string argument "TestAuthType" sets IsAuthenticated = true
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var user = new ClaimsPrincipal(identity);

            // 3. Attach to Controller Context
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        [Fact]
        public async Task UpdateProfile_ValidData_ReturnsOk()
        {
            // Arrange
            var updateDto = new UpdateProviderDto
            {
                Name = "Dr. Updated",
                Specialty = "Surgery"
            };

            // Setup mock to simply complete successfully
            _mockService.Setup(s => s.UpdateProviderProfileAsync(55, updateDto))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateProfile(updateDto);

            // Assert
            Assert.IsType<OkObjectResult>(result); // Or OkObjectResult if you return data

            // Verify service received ID 55 and the correct DTO
            _mockService.Verify(s => s.UpdateProviderProfileAsync(55, updateDto), Times.Once);
        }

        [Fact]
        public async Task GetMyProfile_ReturnsProfileData()
        {
            // Arrange
            var fakeProfile = new ProviderDetailDto
            {
                Id = 55,
                Name = "Dr. Original"
            };

            _mockService.Setup(s => s.GetProviderByIdAsync(55))
                        .ReturnsAsync(fakeProfile);

            // Act
            var result = await _controller.GetProvider();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<ProviderDetailDto>(okResult.Value);
            Assert.Equal("Dr. Original", dto.Name);
        }
    }
}
