namespace Shor.Blazor.Client.Models;

public class ProviderSummaryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
}