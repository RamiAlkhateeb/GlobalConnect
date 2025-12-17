using Application.Modules.Identity.DTOs;
using Application.Modules.Identity.Interfaces;
using GlobalConnect.Application.Modules.Identity.DTOs;
using GlobalConnect.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                // Calling the Login service logic we implemented in AuthService
                var response = await _authService.LoginAsync(request);

                // Return 200 OK with the User ID, Role, and the JWT Token
                return Ok(response);
            }
            catch (DomainException ex)
            {
                // Handle "Invalid email or password" or other business rule violations
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception)
            {
                // General error handling (logging would happen here)
                return StatusCode(500, new { message = "An internal error occurred." });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var response = await _authService.RegisterAsync(request);
                return CreatedAtAction(nameof(Register), new { id = response.UserId }, response);
            }
            catch (UserAlreadyExistsException ex)
            {
                // Specific HTTP status code for existing resource
                return Conflict(new { message = ex.Message });
            }
            catch (DomainException ex)
            {
                // Handle other business rule exceptions
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
