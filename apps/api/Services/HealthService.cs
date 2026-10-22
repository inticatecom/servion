using System.Collections.Concurrent;
using API.Contexts;
using JetBrains.Annotations;

namespace API.Services;

[UsedImplicitly]
public sealed class HealthService(DatabaseContext database, ILogger<HealthService> logger)
{
    public static readonly ConcurrentDictionary<string, bool> CurrentHealth = new();

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