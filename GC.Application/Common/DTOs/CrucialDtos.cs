using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalConnect.Application.Common.DTOs
{
    public class ReferenceDataDto
    {
        public List<ReferenceItemDto> Specialties { get; set; }
        public List<ReferenceItemDto> Languages { get; set; }
        public List<ReferenceItemDto> Nationalities { get; set; }
    }

    public class ReferenceItemDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
