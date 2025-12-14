using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modules.Identity.DTOs
{
    public class AuthResponse
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public string Role { get; set; } // "Provider" or "Seeker"
    }
}
