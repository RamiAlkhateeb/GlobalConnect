using GlobalConnect.Application.Modules.Identity.DTOs;

namespace Application.Modules.Identity.Interfaces
{
    public interface IAuthService
    {
        // Returns AuthResponse on success, throws Exception on failure
        Task<AuthResponseDto> LoginWithGoogleAsync(string googleIdToken);
    }
}
