using API.Contexts;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

/// <summary>
///     Services for managing locations in the system.
/// </summary>
public class LocationService(DatabaseContext database)
{
    /// <summary>
    ///     Creates a new location that can be used with servers.
    /// </summary>
    /// 
    /// <param name="name">The name of the location.</param>
    /// <param name="description">The description for what the location will be used for.</param>
    /// 
    /// <returns>The newly created location.</returns>
    public async Task<LocationModel> CreateLocationAsync(string name, string description)
    {
        var location = new LocationModel
        {
            Name = name,
            Description = description
        };

        database.Add(location);
        await database.SaveChangesAsync();

        return location;
    }

    /// <summary>
    ///     Removes a location from the database if possible.
    /// </summary>
    /// 
    /// <param name="id">The ID of the location to delete.</param>
    /// 
    /// <exception cref="Exception">Throws if something fails while trying to delete the location.</exception>
    public async Task RemoveLocationAsync(Guid id)
    {
        var location = await database.Locations.Include(location => location.Backends).FirstAsync(location => location.Id == id) ?? throw new Exception("Location does not exist.");
        if (location.Backends.Count > 0) throw new Exception("Location is unable to be deleted since one or more backends are already using it.");

        database.Locations.Remove(location);
        await database.SaveChangesAsync();
    }
}