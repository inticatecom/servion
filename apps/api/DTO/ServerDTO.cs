namespace API.DTO;

public static class ServerDto
{
    public record Create(string Name, bool ShouldStart);

    public record Update(string? Name);

    public record Response(string Name);

    public record SetState(int State);
}