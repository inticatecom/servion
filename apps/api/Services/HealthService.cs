using System.Collections.Concurrent;
using API.Contexts;
using JetBrains.Annotations;

namespace API.Services;

/// <summary>
///     The service for getting the status of internal services.
/// </summary>
[UsedImplicitly]
public sealed class HealthService(DatabaseContext database)
{
    /// <summary>
    ///     The list of service entries and their statuses.
    /// </summary>
    public static readonly ConcurrentDictionary<string, bool> CurrentHealth = new();

    /// <summary>
    ///     Checks the database health.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The status of the database.</returns>
    public async Task<bool> CheckDatabaseHealth(CancellationToken cancellationToken = default)
    {
        try
        {
            return await database.Database.CanConnectAsync(cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch
        {
            return false;
        }
    }
}