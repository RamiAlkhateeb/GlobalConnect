using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Add these lines before builder.Build()
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7143/") });
builder.Services.AddScoped<Shor.Blazor.Client.Services.ProviderApiService>();

await builder.Build().RunAsync();
