using API.Interfaces;

namespace API.Contexts;

/// <summary>
///     A context for publishing events to all loaded extensions.
/// </summary>
public sealed class EventContext(IServiceScopeFactory scopes) : IEventPublisher
{
    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IEvent
    {
        await using var scope = scopes.CreateAsyncScope();
        var handlers = scope.ServiceProvider.GetServices<IEventHandler<TEvent>>();

        foreach (var handler in handlers) await handler.HandleAsync(@event, cancellationToken);
    }
}

/// <summary>
///     Triggers when a new environment is created & stored into the system.
/// </summary>
/// <param name="Id">The environment's ID.</param>
public record EnvironmentCreateEvent(Guid Id) : IEvent;