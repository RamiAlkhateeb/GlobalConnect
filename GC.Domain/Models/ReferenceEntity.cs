using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ReferenceEntity
    {
        public int Id { get; set; }
        public string Code { get; set; } // e.g., "en-US" or "CARDIO"
        public string Name { get; set; } // e.g., "English" or "Cardiology"
    }

    public class Specialty : ReferenceEntity { }
}
