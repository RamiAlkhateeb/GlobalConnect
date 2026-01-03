using GlobalConnect.Application.Common.DTOs;
using GlobalConnect.Application.Modules.Booking.Interfaces;
using GlobalConnect.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IWebHostEnvironment _env; // For file saving
        private readonly GlobalConnectDbContext _context; // Direct DB access for Reference data (shortcut)

        public DashboardController(IBookingService bookingService, IWebHostEnvironment env, GlobalConnectDbContext context)
        {
            _bookingService = bookingService;
            _env = env;
            _context = context;
        }

        


    }
}
