
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;

namespace GlobalConnect.Infrastructure.Services
{
    public class GoogleAuthService
    {
        private readonly string _clientId;
        public GoogleAuthService(IConfiguration configuration)
        {
            _clientId = configuration["Google:ClientId"]!;
        }
        public async Task<GoogleJsonWebSignature.Payload> ValidateAsync(string idToken)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>() { _clientId }
            };
            return await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
        }
    }
}
