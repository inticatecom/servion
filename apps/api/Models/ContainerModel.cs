using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

/// <summary>
///     Individual entity so we can clean up unused containers.
/// </summary>
[Index(nameof(Id), IsUnique = true)]
public class ContainerModel
{
    /// <summary>
    ///     The ID of the container.
    /// </summary>
    [MaxLength(64)]
    public required string Id { get; init; }

    /// <summary>
    ///     The server (if it exists) that is linked to the container.
    /// </summary>
    public ServerModel? Server { get; init; }
}