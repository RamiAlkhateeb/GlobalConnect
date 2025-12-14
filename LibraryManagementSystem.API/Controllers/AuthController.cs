using Application.Modules.Identity.DTOs;
using Application.Modules.Identity.Interfaces;
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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var response = await _authService.RegisterAsync(request);
                return CreatedAtAction(nameof(Register), new { id = response.UserId }, response);
            }
            catch (Exception ex)
            {
                // In a real app, use a global exception handler. 
                // For now, we manually return BadRequest for simplicity.
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
