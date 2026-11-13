using System.ComponentModel.DataAnnotations;
using API.Models;
using API.Services;

namespace API.DTO;

/// <summary>
/// Server DTO for operations relating to server interaction.
/// </summary>
public static class ServerDto
{
    /// <summary>
    /// The data required from the client for creating a new server.
    /// </summary>
    /// <param name="OwnerId">The ID of the owner to assign the server to.</param>
    /// <param name="EnvironmentId">The ID of the environment to use for the server.</param>
    /// <param name="Name">The name of the server.</param>
    /// <param name="Description">The description to set for the server.</param>
    /// <param name="Image">The key of the image to use for the server..</param>
    /// <param name="Variables">The configured variables for the server.</param>
    /// <param name="ShouldStart">Whether the server should be automatically started once created or not.</param>
    public record Create(
        [Required] Guid OwnerId,
        [Required] Guid EnvironmentId,
        [Required] string Name,
        [Required] string Description,
        [Required] string Image,
        [Required] Dictionary<string, ServerVariable> Variables,
        [Required] bool ShouldStart
    );

    /// <summary>
    /// Sets the state for a server.
    /// </summary>
    /// <param name="State">The state of the server.</param>
    public record SetState([Required] ServerState State);

    /// <summary>
    /// The data required to delete a server.
    /// </summary>
    /// <param name="Id">The ID of the server to delete.</param>
    public record Delete([Required] Guid Id);
}