using GlobalConnect.Application.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IReferenceDataService
    {
        Task<ReferenceDataDto> GetReferenceDataAsync();

    }
}
