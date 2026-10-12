using API.Contexts;
using API.Services;
using Docker.DotNet;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

namespace API;

/// <summary>
/// The main entrypoint of the program.
/// </summary>
public static class Program
{
    /// <summary>
    /// The method that is called when the program starts.
    /// </summary>
    public static void Main(string[] args)
    {
        // Load Environment Variables
        Env.Load("../../.env");

        // Construct Application
        var builder = CreateBuilder();
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

    /// <summary>
    /// Constructs the website application builder.
    /// </summary>
    /// <exception cref="Exception">An error if environment variable assets aren't present.</exception>
    /// <returns>The website application builder.</returns>
    private static WebApplicationBuilder CreateBuilder()
    {
        var builder = WebApplication.CreateBuilder();

        // Configure Services
        builder.Services.AddControllers();

        // Make database accessible as a service.
        builder.Services.AddDbContext<DatabaseContext>(options =>
            options.UseNpgsql(Environment.GetEnvironmentVariable("DATABASE_URL") ??
                              throw new Exception("DATABASE_URL not configured.")));

        // Make Docker client accessible as a service.
        builder.Services.AddSingleton(_ =>
            new DockerClientConfiguration(new Uri(Environment.GetEnvironmentVariable("DOCKER_PATH") ??
                                                  throw new Exception("DOCKER_PATH not configured."))).CreateClient());

        // Add server service as a service.
        builder.Services.AddScoped<ServerService>();

        // Setup Authorization
        builder.Services.AddAuthorization();

        // Setup OpenAPI
        builder.Services.AddOpenApi();

        return builder;
    }
}