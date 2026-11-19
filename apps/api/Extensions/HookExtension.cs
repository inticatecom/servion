using API.Contexts;
using API.Interfaces;

namespace API.Extensions;

public sealed class HookExtension(ILogger<HookExtension> logger) : IExtension, IEventHandler<EnvironmentCreateEvent>
{
    public Task HandleAsync(EnvironmentCreateEvent @event, CancellationToken cancellationToken)
    {
        logger.LogInformation("Environment created: {id}", @event.id);
        return Task.CompletedTask;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Successfully loaded hook extension.");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Successfully unloaded hook extension.");
        return Task.CompletedTask;
    }
}