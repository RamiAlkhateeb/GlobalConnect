using GlobalConnect.Application.Modules.Provider.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalConnect.Application.Modules.Provider.Interfaces
{
    public interface IProviderService
    {
        // Returns null if not found
        Task<ProviderDetailDto?> GetProviderByIdAsync(int id);
    }
}
