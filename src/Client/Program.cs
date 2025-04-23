using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using DotGPT.Client;
using DotGPT.Core.Interfaces;
using DotGPT.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClient to use the appropriate server URL based on environment
builder.Services.AddScoped(sp => 
{
    // Get the host environment
    var hostEnv = sp.GetService<IWebAssemblyHostEnvironment>();
    
    // Determine the server URL based on environment
    string serverUrl;
    
    if (hostEnv.IsEnvironment("Development") && !hostEnv.IsEnvironment("Docker"))
    {
        // Local development outside Docker
        serverUrl = builder.HostEnvironment.BaseAddress;
    }
    else if (hostEnv.IsEnvironment("Docker"))
    {
        // In Docker, use relative URL as nginx will handle proxying
        serverUrl = "/";
        Console.WriteLine("Running in Docker environment - using relative URL for API requests");
    }
    else 
    {
        // Production or other environment
        serverUrl = "http://dotgpt.server:8080";
    }
    
    Console.WriteLine($"Using server URL: {serverUrl}");
    return new HttpClient { BaseAddress = new Uri(serverUrl) };
});

// Register services
builder.Services.AddScoped<ISettingsService, LocalStorageSettingsService>();
builder.Services.AddScoped<OllamaClientService>();

await builder.Build().RunAsync();
