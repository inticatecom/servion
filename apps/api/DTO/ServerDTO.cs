namespace API.DTO;

public static class ServerDto
{
    public record Create(string Name);

    public record Update(string? Name);

    public record Response(string Name);
}