using API.Contexts;
using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

using Models;

public enum ServerState
{
    Offline = 0,
    Online = 1,
    Restarting = 2
}

public sealed class ServerService(DockerClient docker, DatabaseContext database)
{
    public async Task<List<ServerModel>> ListServers()
    {
        return await database.Servers.ToListAsync();
    }

    public async Task<ServerModel?> GetServer(Guid id)
    {
        return await database.Servers.FindAsync(id);
    }

    public async Task CreateServer(string name)
    {
        const string image = "nginx:stable-trixie-perl";
        var imageParts = image.Split(":", 2);

        await docker.Images.CreateImageAsync(new ImagesCreateParameters
        {
            FromImage = imageParts[0],
            Tag = imageParts.Length > 1 ? imageParts[1] : "latest"
        }, null, new Progress<JSONMessage>(), CancellationToken.None);

        var container = await docker.Containers.CreateContainerAsync(new CreateContainerParameters
            { Name = name, Image = image });

        database.Add(new ServerModel { OwnerId = Guid.NewGuid(), Name = name, ContainerId = container.ID });
        await database.SaveChangesAsync();
    }

    public async Task SetServerState(Guid id, ServerState state)
    {
        var server = await database.Servers.FindAsync(id);
        if (server is null)
        {
            throw new Exception("Server does not exist.");
        }

        switch (state)
        {
            case ServerState.Online:
            {
                await docker.Containers.StartContainerAsync(server.ContainerId, new ContainerStartParameters { },
                    CancellationToken.None);
                break;
            }
            case ServerState.Offline:
            {
                await docker.Containers.StopContainerAsync(server.ContainerId, new ContainerStopParameters { },
                    CancellationToken.None);
                break;
            }
        }
    }
}