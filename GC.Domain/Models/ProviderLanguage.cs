namespace GlobalConnect.Domain.Models
{
    public class ProviderLanguage
    {

        public int ProviderId { get; set; }
        public virtual Provider Provider { get; set; } = null!;
        public int LanguageId { get; set; }
        public virtual Language Language { get; set; } = null!;
    }
}
