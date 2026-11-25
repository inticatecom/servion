using API.Contexts;
using API.Exceptions;
using API.Models;
using Docker.DotNet;
using Docker.DotNet.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

// TODO: Migrate server creation/storage to use backends instead of executing from Docker on host.

/// <summary>
///     Represents the state for a server.
/// </summary>
public enum ServerState
{
    Offline = 0,
    Online = 1,
    Restarting = 2
}

/// <summary>
///     The parameters required for creating a new server.
/// </summary>
public record ServerCreateParameters
{
    public required string Name { get; init; } = "";
    public required Guid OwnerId { get; init; }
    public required Guid EnvironmentId { get; init; }
    public required string ImageKey { get; init; } = "";
    public required Dictionary<string, ServerVariable> Variables { get; init; } = new();
}

/// <summary>
///     A service for managing servers.
/// </summary>
[UsedImplicitly]
public sealed class ServerService(DockerClient docker, DatabaseContext database, EnvironmentService environments)
{
    /// <summary>
    ///     Lists all existing servers regardless of the owner.
    /// </summary>
    /// <returns>The list of servers.</returns>
    public async Task<List<ServerModel>> ListServersAsync()
    {
        return await database.Servers.ToListAsync();
    }

    /// <summary>
    ///     Gets a specific server's information based on its identifier.
    /// </summary>
    /// <param name="id">The identifier of the server.</param>
    /// <returns>The found server.</returns>
    public async Task<ServerModel?> GetServerAsync(Guid id)
    {
        return await database.Servers.FindAsync(id);
    }

    /// <summary>
    ///     Creates a new server with the provided information.
    /// </summary>
    /// <param name="options">The options to apply to the newly created server.</param>
    /// <returns>The created server.</returns>
    public async Task<ServerModel> CreateServerAsync(ServerCreateParameters options)
    {
        var environment = await database.Environments.FindAsync(options.EnvironmentId) ?? throw new ServerException(400, "Invalid environment ID.");

        // TODO: Ensure provided variables match schema of environment parameters.
        var result = await environments.ValidateEnvironmentParametersAsync(options.Variables, environment.Id);

        var image = environment.Images[options.ImageKey];
        var imageParts = image.Split(":", 2);

        await docker.Images.CreateImageAsync(new ImagesCreateParameters
        {
            FromImage = imageParts[0],
            Tag = imageParts.Length > 1 ? imageParts[1] : ""
        }, null, new Progress<JSONMessage>(), CancellationToken.None);

        var container = await docker.Containers.CreateContainerAsync(new CreateContainerParameters
        {
            Name = options.Name,
            Image = image,
            Env = [.. options.Variables.Select(variable => $"{variable.Value.Name}={variable.Value.Value}")]
        });

        var server = new ServerModel
        {
            OwnerId = options.OwnerId,
            EnvironmentId = options.EnvironmentId,
            Name = options.Name,
            ContainerId = container.ID,
            Variables = options.Variables,
            BackendInstanceId = Guid.NewGuid() // TODO: Properly set this.
        };
        database.Add(server);

        await database.SaveChangesAsync();

        return server;
    }

    /// <summary>
    ///     Deletes a server and it's data.
    /// </summary>
    /// <param name="id">The ID of the server to delete.</param>
    /// <param name="forcefully">Whether the operation should be carried out forcefully.</param>
    /// <exception cref="ServerException">Errors if any of the tasks fail to complete.</exception>
    public async Task DeleteServerAsync(Guid id, bool? forcefully)
    {
        var server = await database.Servers.FindAsync(id) ?? throw new ServerException(404, "Server not found.");

        try
        {
            await docker.Containers.RemoveContainerAsync(
                server.ContainerId,
                new ContainerRemoveParameters
                {
                    Force = forcefully
                },
                CancellationToken.None
            );
        }
        catch
        {
            throw new ServerException(500, "Failed to remove Docker container.");
        }

        try
        {
            database.Servers.Remove(server);
            await database.SaveChangesAsync();
        }
        catch
        {
            throw new ServerException(500, "Failed to delete server from database.");
        }
    }

    /// <summary>
    ///     Changes the current state of the provided server.
    /// </summary>
    /// <param name="id">The identifier of the server.</param>
    /// <param name="state">The state to set for the server.</param>
    /// <exception cref="Exception">Errors if the server can't be found by the provided identifier.</exception>
    public async Task SetServerStateAsync(Guid id, ServerState state)
    {
        var server = await database.Servers.FindAsync(id) ?? throw new ServerException(404, "Server does not exist.");
        switch (state)
        {
            case ServerState.Online:
                {
                    await docker.Containers.StartContainerAsync(server.ContainerId, new ContainerStartParameters(),
                        CancellationToken.None);
                    break;
                }
            case ServerState.Offline:
                {
                    await docker.Containers.StopContainerAsync(server.ContainerId, new ContainerStopParameters(),
                        CancellationToken.None);
                    break;
                }
        }
    }
}