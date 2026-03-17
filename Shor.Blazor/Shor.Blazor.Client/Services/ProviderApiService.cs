using System.Net.Http.Json;
using Shor.Blazor.Client.Models;

namespace Shor.Blazor.Client.Services;

public class ProviderApiService(HttpClient http)
{
    public async Task<List<ProviderSummaryDto>> GetProvidersAsync(string? search = null)
    {
        var url = string.IsNullOrWhiteSpace(search) 
            ? "api/public/providers" 
            : $"api/public/providers?search={search}";
            
        return await http.GetFromJsonAsync<List<ProviderSummaryDto>>(url) ?? new();
    }
}