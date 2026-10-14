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

public record ServerCreateParameters
{
    public string Name { get; init; } = "";
    public Guid EnvironmentId { get; init; }
    public string ImageKey { get; init; } = "";
    public Dictionary<string, ServerVariable> Variables { get; init; } = new();
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
    /// <param name="options">The name to set for the new server.</param>
    /// <returns>The created server.</returns>
    public async Task<ServerModel> CreateServer(ServerCreateParameters options)
    {
        var environment = await database.Environments.FindAsync(options.EnvironmentId);
        if (environment is null)
        {
            throw new Exception("Invalid environment ID.");
        }

        // TODO: Ensure provided variables match schema of environment parameters.

        var image = environment.Images[options.ImageKey];
        var imageParts = image.Split(":", 2);

        await docker.Images.CreateImageAsync(new ImagesCreateParameters
        {
            FromImage = imageParts[0],
            Tag = imageParts.Length > 1 ? imageParts[1] : ""
        }, null, new Progress<JSONMessage>(), CancellationToken.None);

        var container = await docker.Containers.CreateContainerAsync(new CreateContainerParameters
            { Name = options.Name, Image = image });

        var server = new ServerModel
            { OwnerId = Guid.NewGuid(), EnvironmentId = Guid.NewGuid(), Name = options.Name, ContainerId = container.ID, Variables = options.Variables };
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
                var environmentVariables = server.Variables.Select(variable => $"{variable.Value.Name}={variable.Value.Value}").ToList();
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