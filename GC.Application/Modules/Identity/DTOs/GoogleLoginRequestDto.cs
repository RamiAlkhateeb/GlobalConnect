using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modules.Identity.DTOs
{
    public class GoogleLoginRequestDto
    {
        public string IdToken { get; set; } = string.Empty;
    }
}
