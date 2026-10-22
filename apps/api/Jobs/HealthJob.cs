using API.Services;
using JetBrains.Annotations;
using Quartz;

namespace API.Jobs;

[UsedImplicitly]
public class HealthJob(HealthService health) : IJob
{
    private readonly Dictionary<string, Func<CancellationToken, Task<bool>>> _checks = new()
    {
        ["database"] = health.CheckDatabaseHealth
    };

    public async ValueTask Execute(IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        foreach (var (name, check) in _checks)
        {
            cancellationToken.ThrowIfCancellationRequested();
            HealthService.CurrentHealth[name] = await check(cancellationToken);
        }
    }
}