using API.Services;
using Quartz;

namespace API.Jobs;

/// <summary>
///     A job to be run concurrently to update service statuses used in the health check controller.
/// </summary>
public class HealthJob(HealthService health) : IJob
{
    private readonly Dictionary<string, Func<CancellationToken, Task<bool>>> _checks = new()
    {
        ["database"] = health.CheckDatabaseHealthAsync
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