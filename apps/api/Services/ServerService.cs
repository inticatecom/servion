using API.Contexts;
using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

using Models;

/// <summary>
/// Represents the state for a server.
/// </summary>
public enum ServerState
{
    Offline = 0,
    Online = 1,
    Restarting = 2
}

/// <summary>
/// A service for managing servers.
/// </summary>
/// <param name="docker">The Docker client SDK.</param>
/// <param name="database">The database SDK.</param>
public sealed class ServerService(DockerClient docker, DatabaseContext database)
{
    /// <summary>
    /// Lists all existing servers regardless of the owner.
    /// </summary>
    /// <returns>The list of servers.</returns>
    public async Task<List<ServerModel>> ListServers()
    {
        return await database.Servers.ToListAsync();
    }

    /// <summary>
    /// Gets a specific server's information based on its identifier.
    /// </summary>
    /// <param name="id">The identifier of the server.</param>
    /// <returns>The found server.</returns>
    public async Task<ServerModel?> GetServer(Guid id)
    {
        return await database.Servers.FindAsync(id);
    }

    /// <summary>
    /// Creates a new server with the provided information.
    /// </summary>
    /// <param name="name">The name to set for the new server.</param>
    /// <returns>The created server.</returns>
    public async Task<ServerModel> CreateServer(string name)
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

        var server = new ServerModel
            { OwnerId = Guid.NewGuid(), EnvironmentId = Guid.NewGuid(), Name = name, ContainerId = container.ID };
        database.Add(server);

        await database.SaveChangesAsync();

        return server;
    }

    /// <summary>
    /// Changes the current state of the provided server.
    /// </summary>
    /// <param name="id">The identifier of the server.</param>
    /// <param name="state">The state to set for the server.</param>
    /// <exception cref="Exception">Errors if the server can't be found by the provided identifier.</exception>
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