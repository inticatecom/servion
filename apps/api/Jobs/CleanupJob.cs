using API.Contexts;
using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace API.Jobs;

/// <summary>
///     A job to be run concurrently to clean up outdated assets.
/// </summary>
public sealed class CleanupJob(DatabaseContext database, DockerClient docker) : IJob
{
    public async ValueTask Execute(IJobExecutionContext context, CancellationToken cancellationToken)
    {
        var dockerContainers = await docker.Containers.ListContainersAsync(new ContainersListParameters(), cancellationToken);
        var containers = await database.Containers.AsNoTracking().Include(container => container.Server).ToListAsync(cancellationToken);

        foreach (var container in containers)
            if (container.Server is null)
            {
                // If the server no longer exists, remove the container.
                await docker.Containers.RemoveContainerAsync(container.Id, null, cancellationToken);
                database.Containers.Remove(container);
            }
            else
            {
                // If the container no longer exists, delete the server.
                if (!dockerContainers.Any(item => item.ID == container.Id))
                {
                    database.Servers.Remove(container.Server);
                    database.Containers.Remove(container);
                }
            }

        await database.SaveChangesAsync(cancellationToken);
    }
}