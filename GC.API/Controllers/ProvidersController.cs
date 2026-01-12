using GlobalConnect.Application.Modules.Provider.DTOs;
using GlobalConnect.Application.Modules.Provider.Interfaces;
using GlobalConnect.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GlobalConnect.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProvidersController : ControllerBase
    {
        private readonly IProviderService _providerService;

        public ProvidersController(IProviderService providerService)
        {
            _providerService = providerService;
        }

        // Helper to get User ID from JWT
        private int GetUserId()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier); // standard 'sub' claim
            if (idClaim == null) throw new UnauthorizedAccessException();
            return int.Parse(idClaim.Value);
        }


        [HttpGet("my-profile")]
        public async Task<ActionResult> GetProvider()
        {
            var providerDto = await _providerService.GetProviderByIdAsync(GetUserId());

            return Ok(providerDto);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProviderDto dto)
        {
            try
            {

                await _providerService.UpdateProviderProfileAsync(GetUserId(), dto);

                return Ok(new { message = "Profile updated successfully." });
            }
            catch (DomainException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error during profile update." });
            }
        }



    }
}
