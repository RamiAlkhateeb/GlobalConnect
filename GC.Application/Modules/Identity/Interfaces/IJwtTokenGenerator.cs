using GlobalConnect.Domain.Models;

namespace Application.Modules.Identity.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
