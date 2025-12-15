using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ProviderLanguage
    {
        [Key]
        public int Id { get; set; }

        public int ProviderId { get; set; }

        [Required, MaxLength(10)]
        public string LanguageCode { get; set; }

        // Navigation property
        public Provider Provider { get; set; }
    }
}
