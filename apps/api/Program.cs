using API.Contexts;
using API.Services;
using Docker.DotNet;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using API.DTO;
using API.Jobs;
using API.Models;
using API.Utilities;
using API.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Quartz;

namespace API;

/// <summary>
/// The main entrypoint of the program.
/// </summary>
public static class Program
{
    /// <summary>
    /// The method that is called when the program starts.
    /// </summary>
    public static void Main()
    {
        // Load Environment Variables
        Env.Load("../../.env");

        // Load Environment Variable Utility
        EnvironmentVariableUtility.Initialize();

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
            options.UseNpgsql(
                EnvironmentVariableUtility.GetEnvironmentVariable<string>(EnvironmentVariableKey.DatabaseUrl),
                npgsql => npgsql.ConfigureDataSource(datasource => datasource.EnableDynamicJson())
            )
        );

        // Register Validators
        builder.Services.AddScoped<IValidator<ServerDto.Create>, ServerCreateValidator>();
        builder.Services.AddScoped<IValidator<ServerDto.SetState>, ServerSetStateValidator>();

        // Make Docker client accessible as a service.
        builder.Services.AddSingleton(_ =>
            new DockerClientConfiguration(
                new Uri(
                    EnvironmentVariableUtility.GetEnvironmentVariable<string>(EnvironmentVariableKey.DockerPath)
                )
            ).CreateClient()
        );

        // Make exception utility accessible as a service.
        builder.Services.AddSingleton<ExceptionUtility>();

        // Load Services
        builder.Services.AddScoped<ServerService>();
        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<EnvironmentService>();
        builder.Services.AddScoped<HealthService>();

        builder.WebHost.ConfigureKestrel(options =>
            options.ListenLocalhost(
                EnvironmentVariableUtility.GetEnvironmentVariable<int>(EnvironmentVariableKey.ServerPort)));

        // Configure JSON Parsing
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(
                    new JsonStringEnumConverter()
                );
            });

        builder.Services.AddIdentityApiEndpoints<UserModel>().AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<DatabaseContext>();

        // Initialize job schedules.
        SetupJobs(builder);

        // Setup Authorization
        builder.Services.AddAuthorization();

        // Setup OpenAPI
        builder.Services.AddOpenApi();

        return builder;
    }

    private static void SetupJobs(WebApplicationBuilder builder)
    {
        builder.Services.AddQuartz(options =>
        {
            var cleanupJobKey = new JobKey("CleanupJob");
            options.AddJob<CleanupJob>(job => job.WithIdentity(cleanupJobKey));
            options.AddTrigger(trigger =>
                trigger
                    .ForJob(cleanupJobKey)
                    .WithIdentity("CleanupJobTrigger")
                    // TODO: Make time san configurable from environment options.
                    .WithSimpleSchedule(schedule => schedule.WithInterval(TimeSpan.FromMinutes(15)).RepeatForever())
            );

            var healthJobKey = new JobKey("HealthJob");
            options.AddJob<HealthJob>(job => job.WithIdentity(healthJobKey));
            options.AddTrigger(trigger =>
                trigger
                    .ForJob(healthJobKey)
                    .WithIdentity("HealthJobTrigger")
                    .StartNow()
                    .WithSimpleSchedule(schedule => schedule.WithInterval(TimeSpan.FromSeconds(30)).RepeatForever())
            );
        });

        builder.Services.AddQuartzHostedService(options => { options.WaitForJobsToComplete = true; });
    }
}