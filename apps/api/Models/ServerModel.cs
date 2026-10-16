using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

/// <summary>
/// The structure of how a server is stored in our persistent database.
/// </summary>
[Index(nameof(Name), IsUnique = true)]
public class ServerModel : IdentityModel
{
    /// <summary>
    /// The owner of the server.
    /// </summary>
    public UserModel Owner { get; set; } = null!;

    /// <summary>
    /// The ID of the owner of the server.
    /// </summary>
    public required Guid OwnerId { get; set; }

    /// <summary>
    /// The environment in-use by the server.
    /// </summary>
    public EnvironmentModel Environment { get; set; } = null!;

    /// <summary>
    /// The ID of the environment in-use by the server.
    /// </summary>
    public required Guid EnvironmentId { get; set; }

    /// <summary>
    /// The name of the server.
    /// </summary>
    [StringLength(30, MinimumLength = 3)]
    public required string Name { get; set; }

    /// <summary>
    /// The Docker container ID bound to the server.
    /// </summary>
    [MaxLength(64)]
    public required string ContainerId { get; set; }

    /// <summary>
    /// The configured variables for the server.
    /// </summary>
    [Column(TypeName = "jsonb")]
    public required Dictionary<string, ServerVariable> Variables { get; set; }
}

/// <summary>
/// A configured variable for a server.
/// </summary>
/// <param name="Name">The name of the variable.</param>
/// <param name="Value">The value of the variable.</param>
public record ServerVariable([StringLength(30, MinimumLength = 1)] string Name, [MinLength(1)] string Value);