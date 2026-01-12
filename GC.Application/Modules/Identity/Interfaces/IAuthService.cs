using GlobalConnect.Application.Modules.Identity.DTOs;

namespace Application.Modules.Identity.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterRequest dto);
        Task<AuthResponseDto> LoginAsync(LoginRequest dto); // Returns Token + Role
    }
}
