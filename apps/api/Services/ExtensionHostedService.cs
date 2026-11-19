using API.Interfaces;

namespace API.Services;

/// <summary>
///     A hosted service to automatically trigger extension start & stop sequences.
/// </summary>
public sealed class ExtensionHostedService(IEnumerable<IExtension> extensions) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        foreach (var extension in extensions) extension.StartAsync(CancellationToken.None);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        foreach (var extension in extensions.Reverse()) extension.StopAsync(CancellationToken.None);
        return Task.CompletedTask;
    }
}