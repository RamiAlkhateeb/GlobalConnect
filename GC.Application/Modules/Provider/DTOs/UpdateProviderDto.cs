namespace GlobalConnect.Application.Modules.Provider.DTOs
{
    public class UpdateProviderDto
    {
        public string Name { get; set; }
        public string Specialty { get; set; }
        public string? Bio { get; set; }
        public string GoogleBookingUrl { get; set; }
        public string Nationality { get; set; } = string.Empty;
        public decimal HourlyRateUSD { get; set; }
        public List<int> LanguageIds { get; set; } = new();
    }
}
