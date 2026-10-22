using JetBrains.Annotations;
using Quartz;

namespace API.Jobs;

[UsedImplicitly]
public sealed class CleanupJob(ILogger<CleanupJob> logger) : IJob
{
    public ValueTask Execute(IJobExecutionContext context, CancellationToken cancellationToken)
    {
        logger.LogWarning("TODO: Implement cleanup logic here, log is just to demonstrate Cron functionality.");
        return ValueTask.CompletedTask;
    }
}