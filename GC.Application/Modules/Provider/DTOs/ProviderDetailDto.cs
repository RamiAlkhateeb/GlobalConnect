namespace GlobalConnect.Application.Modules.Provider.DTOs
{
    public class ProviderDetailDto
    {
        public int ProviderId { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string GoogleBookingUrl { get; set; }
        public string Specialty { get; set; }
        public string? PhotoUrl { get; set; }
        public string Bio { get; set; }
        public string Nationality { get; set; }
        public decimal HourlyRateUSD { get; set; }
        public List<string> Languages { get; set; } = new();
        public bool IsActive { get; set; }
    }

    // Input for the search bar
    public class ProviderSearchQuery
    {
        public string? Specialty { get; set; }
        public string? Language { get; set; }
    }
}
