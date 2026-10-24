using API.Contexts;
using Docker.DotNet;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace API.Jobs;

[UsedImplicitly]
public sealed class CleanupJob(DatabaseContext database, DockerClient docker) : IJob
{
    public async ValueTask Execute(IJobExecutionContext context, CancellationToken cancellationToken)
    {
        var containers = await database.Containers.AsNoTracking().Include(container => container.Server).ToListAsync(cancellationToken);
        foreach (var container in containers)
        {
            if (container.Server is null)
            {
                continue;
            }

            await docker.Containers.RemoveContainerAsync(container.Id, null, cancellationToken);
            database.Containers.Remove(container);
        }

        await database.SaveChangesAsync(cancellationToken);
    }
}