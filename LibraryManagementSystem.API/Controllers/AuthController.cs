using Application.Modules.Identity.DTOs;
using Application.Modules.Identity.Interfaces;
using Domain.Exceptions;
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
