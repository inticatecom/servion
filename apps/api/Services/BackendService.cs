using API.Contexts;
using API.Models;

namespace API.Services;

/// <summary>
///     A service for interacting with backends loaded into the system.
/// </summary>
public class BackendService(DatabaseContext database)
{
    /// <summary>
    ///     Retrieves a backend by its identifier.
    /// </summary>
    /// <param name="id">The ID of the backend.</param>
    /// <returns>The backend instance if found.</returns>
    public async Task<BackendModel?> GetBackendAsync(Guid id)
    {
        return await database.Backends.FindAsync(id);
    }

    /// <summary>
    ///     Creates & links a new backend instance into the system.
    /// </summary>
    /// <param name="endpoint">The endpoint at which the backend is hosted at.</param>
    /// <param name="options">The options needed for creating the backend.</param>
    /// <exception cref="Exception">If any of the operations fail.</exception>
    public async Task CreateBackendAsync(Uri endpoint, CreateBackendOptions options)
    {
        // TODO: Validate endpoint accessibility here.
        if (options.OneTimeCode != "TEST_CODE") throw new Exception("Invalid one-time code. Failed to link backend instance to the panel.");

        await database.Backends.AddAsync(new BackendModel
        {
            Endpoint = endpoint,
            LocationId = options.LocationId
        });
    }
}

/// <summary>
///     The options required to create & link a new backend instance.
/// </summary>
public record CreateBackendOptions
{
    /// <summary>
    ///     The one-time linking code for the backend instance.
    /// </summary>
    public required string OneTimeCode { get; init; }

    /// <summary>
    ///     The location ID for the location to categorize the backend under.
    /// </summary>
    public required Guid LocationId { get; init; }
}