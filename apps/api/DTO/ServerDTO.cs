namespace API.DTO;

/// <summary>
/// Server DTO for operations relating to server interaction.
/// </summary>
public static class ServerDto
{
    /// <summary>
    /// The data required from the client for creating a new server.
    /// </summary>
    /// <param name="Name">The name of the server.</param>
    /// <param name="ShouldStart">Whether the server should be automatically started once created or not.</param>
    public record Create(string Name, bool ShouldStart);

    /// <summary>
    /// Sets the state for a server.
    /// </summary>
    /// <param name="State">The enum index for a server state, 0 = off, 1 = on.</param>
    public record SetState(int State);
}