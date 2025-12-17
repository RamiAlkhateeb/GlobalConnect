using AutoMapper;
using GlobalConnect.Application.Modules.Provider.Interfaces;
using GlobalConnect.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

    
    }
}
