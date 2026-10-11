using API.Contexts;
using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

using Models;

public sealed class ServerService(DockerClient docker, DatabaseContext database)
{
    public async Task<List<ServerModel>> ListServers()
    {
        return await database.Servers.ToListAsync();
    }

    public async Task CreateServer(string name)
    {
        var container = await docker.Containers.CreateContainerAsync(new CreateContainerParameters
            { Name = name, Image = "nginx:latest" });

        database.Add(new ServerModel { Name = name, ContainerId = container.ID });
        await database.SaveChangesAsync();
    }
}