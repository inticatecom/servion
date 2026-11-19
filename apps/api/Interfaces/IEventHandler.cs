namespace API.Interfaces;

/// <summary>
///     An empty interface to be implemented into records that represent events for extensions.
/// </summary>
public interface IEvent;

/// <summary>
///     An event handler method based on the provided event.
/// </summary>
/// <typeparam name="TEvent">The event.</typeparam>
public interface IEventHandler<in TEvent> where TEvent : IEvent
{
    /// <summary>
    ///     Handles an events callback.
    /// </summary>
    /// <param name="event">The event and it's data.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public Task HandleAsync(TEvent @event, CancellationToken cancellationToken);
}

/// <summary>
///     An event publisher. Used for internal code triggering extension events.
/// </summary>
public interface IEventPublisher
{
    /// <summary>
    ///     Publishes an event to all loaded extensions.
    /// </summary>
    /// <param name="event">The event.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <typeparam name="TEvent">The event type.</typeparam>
    public Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IEvent;
}