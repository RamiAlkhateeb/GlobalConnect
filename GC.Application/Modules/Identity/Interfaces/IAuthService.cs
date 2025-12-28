using Application.Modules.Identity.DTOs;
using GlobalConnect.Application.Modules.Identity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modules.Identity.Interfaces
{
    public interface IAuthService
    {
        // Returns AuthResponse on success, throws Exception on failure
        Task<AuthResponseDto> LoginWithGoogleAsync(string googleIdToken);
    }
}
