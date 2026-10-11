using API.Services;
using DotNetEnv;

namespace API;

public static class Program
{
    public static void Main(string[] args)
    {
        // Load Environment Variables
        Env.Load("../../.env");

        // Construct Builder
        var builder = WebApplication.CreateBuilder(args);

        // Configure Services
        builder.Services.AddControllers();
        builder.Services.AddScoped<ServerService>();

        // Setup Authorization
        builder.Services.AddAuthorization();

        // Setup OpenAPI
        builder.Services.AddOpenApi();

        // Construct Application
        var app = builder.Build();

        // Register Controllers
        app.MapControllers();

        // Map OpenAPI in development mode.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        // HTTP Server Settings
        app.UseHttpsRedirection();
        app.UseAuthorization();

        // Start Application
        app.Run();
    }
}