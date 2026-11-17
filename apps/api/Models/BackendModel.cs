namespace API.Models;

/// <summary>
///     A model for storing backend API paths. These are basically groups for servers that interlink with the root panel.
///     This allows for the panel to interlink with multiple servers instead of just the host.
/// </summary>
public class BackendModel : IdentityModel
{
    /// <summary>
    ///     The location for the backend.
    /// </summary>
    public LocationModel Location = null!;

    /// <summary>
    ///     The root endpoint of the backend.
    /// </summary>
    public required Uri Endpoint { get; init; }

    /// <summary>
    ///     The ID of the location for the backend.
    /// </summary>
    public required Guid LocationId { get; set; }
}