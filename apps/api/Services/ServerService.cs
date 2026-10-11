namespace API.Services;

using Models;

public class ServerService
{
    public Server GetServer(string id)
    {
        return new Server(id, "Test Server");
    }
}