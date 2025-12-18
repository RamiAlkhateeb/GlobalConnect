using Application.Modules.Provider.DTOs;
using AutoMapper;
using GlobalConnect.Application.Modules.Provider.DTOs;
using GlobalConnect.Application.Modules.Provider.Interfaces;
using GlobalConnect.Domain.Exceptions;
using GlobalConnect.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvidersController : ControllerBase
    {
        private readonly IProviderService _providerService;

        public ProvidersController(IProviderService providerService)
        {
            _providerService = providerService;
        }

        private int GetCurrentUserId()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier); // standard JWT 'sub' claim
            if (idClaim == null) throw new UnauthorizedAccessException("Invalid Token: Missing User ID.");
            return int.Parse(idClaim.Value);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetProvider(int id)
        {
            var providerDto = await _providerService.GetProviderByIdAsync(id);
            if (providerDto == null)
            {
                return NotFound(new { message = $"Provider with ID {id} not found." });
            }

            return Ok(providerDto);
        }

        [Authorize(Roles = "Provider")]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProviderDto dto)
        {
            try
            {
                // Get ID from the JWT token (No hardcoded IDs!)
                int providerId = GetCurrentUserId();

                await _providerService.UpdateProviderProfileAsync(providerId, dto);

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

        // Step 1: Browse List
        // GET: /api/providers?specialty=dentist&language=en
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] ProviderSearchQuery query)
        {
            var results = await _providerService.SearchProvidersAsync(query);
            return Ok(results);
        }


    }
}
