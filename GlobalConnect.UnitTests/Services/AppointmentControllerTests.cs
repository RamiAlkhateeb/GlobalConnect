using GlobalConnect.API.Controllers;
using GlobalConnect.Application.Modules.Booking.DTOs;
using GlobalConnect.Application.Modules.Booking.Interfaces;
using GlobalConnect.Domain.Models;
using GlobalConnect.UnitTests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GlobalConnect.UnitTests.Services
{
    public class AppointmentControllerTests
    {
        private readonly Mock<IBookingService> _mockService;
        private readonly BookingsController _controller;
        private readonly ClaimsPrincipal _mockUser;

        public AppointmentControllerTests()
        {
            // 1. Setup the Mock Service (The "Fake" Logic)
            _mockService = new Mock<IBookingService>();

            // 2. Setup the Controller with the Mock
            _controller = new BookingsController(_mockService.Object);

            // 3. Setup the Fake User (Logged in as ID 10)
            _mockUser = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim("id", "10"),
            new Claim(ClaimTypes.Name, "TestUser")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _mockUser }
            };
        }

        [Fact]
        public async Task GetMyAppointments_CallsServiceWithCorrectUserId()
        {
            // Arrange
            var fakeList = new List<BookingDto>
        {
            new BookingDto { AppointmentId = 1, Title = "Test Appt" }
        };

            // Tell the mock: "When GetMyAppointments(10) is called, return fakeList"
            _mockService.Setup(s => s.GetMyAppointments(10))
                        .ReturnsAsync(fakeList);

            // Act
            var result = await _controller.GetMyAppointments();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedList = Assert.IsType<Task<List<BookingDto>>>(okResult.Value);

            Assert.Single(returnedList.Result);
            Assert.Equal("Test Appt", returnedList.Result[0].Title);

            // Verify the service was actually called with ID 10
            _mockService.Verify(s => s.GetMyAppointments(10), Times.Once);
        }

        [Fact]
        public async Task SyncWithGoogle_ReturnsCorrectMessage()
        {
            // Arrange
            var request = new SyncRequest { AccessToken = "fake-token" };

            // Tell mock: "When Sync runs, pretend we found 5 new events"
            _mockService.Setup(s => s.SyncWithGoogle(10, request))
                        .ReturnsAsync(5);

            // Act
            var result = await _controller.SyncWithGoogle(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            // Use Reflection or dynamic to check anonymous object property { message = ... }
            // Or simply check the JSON string serialization if preferred.
            // Here we assume a simple dynamic check for the anonymous type:
            object value = okResult.Value;
            string message = (string)value.GetType().GetProperty("message").GetValue(value, null);
            Assert.Equal("Synced 5 new appointments.", message);
        }
    }
}
