using System.Text.Json.Serialization;
using API.Contexts;
using API.DTO;
using API.Interfaces;
using API.Jobs;
using API.Models;
using API.Services;
using API.Utilities;
using API.Validators;
using Docker.DotNet;
using DotNetEnv;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Scalar.AspNetCore;

namespace API;

/// <summary>
///     The main entrypoint of the program.
/// </summary>
public static class Program
{
    /// <summary>
    ///     The method that is called when the program starts.
    /// </summary>
    public static void Main()
    {
        // Load Environment Variables
        Env.Load("../../.env");

        // Load Environment Variable Utility
        EnvironmentVariableUtility.Initialize();

        // Construct Application
        var builder = CreateBuilder();
        var app = CreateApplication(builder);

        // Start Application
        app.Run();
    }

    /// <summary>
    ///     Constructs the website application builder.
    /// </summary>
    /// <exception cref="Exception">An error if environment variable assets aren't present.</exception>
    /// <returns>The website application builder.</returns>
    private static WebApplicationBuilder CreateBuilder()
    {
        var builder = WebApplication.CreateBuilder();

        // Ensure HTTP server starts on the configured port.
        builder.WebHost.ConfigureKestrel(options =>
            options.ListenLocalhost(
                EnvironmentVariableUtility.GetEnvironmentVariable<int>(EnvironmentVariable.ServerPort)
            )
        );

        SetupJobs(builder);
        ConfigureServices(builder);

        return builder;
    }

    /// <summary>
    ///     Constructs the application.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <returns>The constructed web application.</returns>
    private static WebApplication CreateApplication(WebApplicationBuilder builder)
    {
        var app = builder.Build();

        // Register Controllers
        app.MapControllers();

        // Map OpenAPI in development mode.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference("/docs", options =>
                options.HideDarkModeToggle().ForceDarkMode().SortOperationsByMethod().HideDeveloperTools()
            );
        }

        // HTTP Server Settings
        app.UseHttpsRedirection();
        app.UseAuthorization();

        // Use Custom Exception Handler
        app.UseExceptionHandler();

        return app;
    }

    /// <summary>
    ///     Initializes all extensions into the builder by adding them as singleton's. The extensions lifecycle methods
    ///     get called in the 'ExtensionHostedService' service.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    private static void InitializeExtensions(WebApplicationBuilder builder)
    {
        var extensions = typeof(IExtension).Assembly.GetTypes().Where(type => type is { IsClass: true, IsAbstract: false } && typeof(IExtension).IsAssignableFrom(type));
        foreach (var extension in extensions) builder.Services.AddSingleton(typeof(IExtension), extension);

        builder.Services.AddSingleton<IEventPublisher, EventContext>();
    }

    /// <summary>
    ///     Configures all services into the system.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        // Configure Services
        builder.Services.AddControllers();

        // Attach Global Exception Handler
        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

        // Make database accessible as a service.
        builder.Services.AddDbContext<DatabaseContext>(options =>
            options.UseNpgsql(
                EnvironmentVariableUtility.GetEnvironmentVariable<string>(EnvironmentVariable.DatabaseUrl),
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
                    EnvironmentVariableUtility.GetEnvironmentVariable<string>(EnvironmentVariable.DockerPath)
                )
            ).CreateClient()
        );

        // Load Services
        builder.Services.AddScoped<ServerService>();
        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<EnvironmentService>();
        builder.Services.AddScoped<HealthService>();
        builder.Services.AddScoped<LocationService>();
        builder.Services.AddScoped<BackendService>();

        // Configure JSON Parsing
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(
                    new JsonStringEnumConverter()
                );
            });

        builder.Services
            .AddIdentityApiEndpoints<UserModel>()
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<DatabaseContext>();

        InitializeExtensions(builder);
        builder.Services.AddHostedService<ExtensionHostedService>();

        builder.Services.AddAuthorization();
        builder.Services.AddOpenApi();
    }

    /// <summary>
    ///     Configures & initializes concurrent jobs to be run incrementally.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
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