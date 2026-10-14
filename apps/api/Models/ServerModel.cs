using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

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

    public EnvironmentModel Environment { get; set; } = null!;

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

    [Column(TypeName = "jsonb")] public required Dictionary<string, ServerVariable> Variables { get; set; }
}

public record ServerVariable(string Name, string Value);