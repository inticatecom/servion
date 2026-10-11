using API.Contexts;
using API.Services;
using Docker.DotNet;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

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
        builder.Services.AddDbContext<DatabaseContext>(options =>
            options.UseNpgsql(Environment.GetEnvironmentVariable("DATABASE_URL") ??
                              throw new Exception("DATABASE_URL not configured.")));

        // Make Docker client accessible as a service.
        builder.Services.AddSingleton(_ =>
            new DockerClientConfiguration(new Uri(Environment.GetEnvironmentVariable("DOCKER_PATH") ??
                                                  throw new Exception("DOCKER_PATH not configured."))).CreateClient());

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