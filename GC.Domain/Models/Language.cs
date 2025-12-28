using System.ComponentModel.DataAnnotations;

namespace GlobalConnect.Domain.Models
{
    public class Language
    {
        [Key]
        public int LanguageId { get; set; }
        public string Name { get; set; } = string.Empty; // e.g., "English"
        public virtual ICollection<ProviderLanguage> ProviderLanguages { get; set; } = new List<ProviderLanguage>();
    }
}