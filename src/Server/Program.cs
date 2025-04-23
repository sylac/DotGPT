var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register Core and Infrastructure services
builder.Services.AddHttpClient("ollama", client =>
{
    // When running in Docker, use the service name as the base address
    client.BaseAddress = new Uri("http://ollama:11434");
});

builder.Services.AddScoped<DotGPT.Core.Interfaces.IOllamaService, DotGPT.Infrastructure.Services.OllamaService>(sp => {
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient("ollama");
    return new DotGPT.Infrastructure.Services.OllamaService(httpClient);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseWebAssemblyDebugging(); // Add this for Blazor debugging
}

// Remove HTTPS redirection if not needed (--no-https was used)
// app.UseHttpsRedirection(); 

app.UseBlazorFrameworkFiles(); // Add this to serve Blazor files
app.UseStaticFiles();          // Add this to serve static files (like CSS, JS from wwwroot)

app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("index.html"); // Add this to handle Blazor routing

app.Run();
