using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalConnect.Application.Modules.Identity.DTOs
{
    public class AuthResponseDto
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty; // Your API's JWT
        public string Role { get; set; } = string.Empty;
    }
}
