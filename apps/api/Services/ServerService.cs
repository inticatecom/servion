using Docker.DotNet;

namespace API.Services;

using Models;

public sealed class ServerService(DockerClient docker)
{
    public Server GetServer(string id)
    {
        return new Server(id, "Test Server");
    }
}