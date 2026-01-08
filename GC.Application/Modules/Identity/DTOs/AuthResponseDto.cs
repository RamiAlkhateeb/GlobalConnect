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
