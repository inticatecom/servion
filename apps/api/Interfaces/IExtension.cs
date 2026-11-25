namespace API.Interfaces;

/// <summary>
///     An interface representing the structure of an extension.
/// </summary>
public interface IExtension
{
    /// <summary>
    ///     Logic that runs upon starting the extension.
    /// </summary>
    public Task StartAsync(CancellationToken cancellationToken);

    /// <summary>
    ///     Logic that runs before the application shuts down.
    /// </summary>
    public Task StopAsync(CancellationToken cancellationToken);
}