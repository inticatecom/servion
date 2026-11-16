using System.ComponentModel.DataAnnotations;

namespace API.Models;

/// <summary>
///     A location acts as a group. Primarily used for location grouping of nodes like Canada or America.
/// </summary>
public class LocationModel : IdentityModel
{
    /// <summary>
    ///     The name of the location.
    /// </summary>
    [StringLength(30, MinimumLength = 2)]
    public required string Name { get; set; }

    /// <summary>
    ///     The description for what the location is used for.
    /// </summary>
    [StringLength(200, MinimumLength = 10)]
    public required string Description { get; set; }

    public ICollection<BackendModel> Backends { get; set; } = [];
}