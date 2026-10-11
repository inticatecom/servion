using API.Contexts;
using Docker.DotNet;

namespace API.Services;

using Models;

public sealed class ServerService(DockerClient docker, DatabaseContext database)
{
    public ServerModel GetServer(string id)
    {
        return new ServerModel { Id = "abc", Name = "Test Server" };
    }
}