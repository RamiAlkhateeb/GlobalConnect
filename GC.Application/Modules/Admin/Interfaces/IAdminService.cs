using GlobalConnect.Application.Modules.Provider.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modules.Admin.Interfaces
{
    public interface IAdminService
    {
        Task<List<ProviderDetailDto>> GetAllProvidersAsync();
        Task ToggleProviderStatusAsync(int userId);
    }
}
