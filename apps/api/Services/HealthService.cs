using System.Collections.Concurrent;
using API.Contexts;
using JetBrains.Annotations;

namespace API.Services;

[UsedImplicitly]
public sealed class HealthService(DatabaseContext database, ILogger<HealthService> logger)
{
    public static readonly ConcurrentDictionary<string, bool> CurrentHealth = new();

    public async Task<bool> CheckDatabaseHealth()
    {
        try
        {
            return await database.Database.CanConnectAsync();
        }
        catch
        {
            return false;
        }
    }
}