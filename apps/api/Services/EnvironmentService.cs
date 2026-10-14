using API.Contexts;
using API.Exceptions;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

/// <summary>
/// A service for interacting with the environments loaded into the system.
/// </summary>
/// <param name="database">The database SDK.</param>
public sealed class EnvironmentService(DatabaseContext database)
{
    /// <summary>
    /// Safely creates a new environment in the database.
    /// </summary>
    /// <param name="environment">The environment to save.</param>
    /// <exception cref="EnvironmentException">Errors if any of the safety rules aren't met.</exception>
    public async Task CreateEnvironment(EnvironmentModel environment)
    {
        var existingEnvironment = await database.Environments.AnyAsync(env => env.Name == environment.Name);
        if (existingEnvironment)
        {
            throw new EnvironmentException(409, "There is already an environment that exists with the provided name.");
        }

        try
        {
            database.Environments.Add(environment);
            await database.SaveChangesAsync();
        }
        catch
        {
            throw new EnvironmentException(500, "Failed to create environment for an unknown reason.");
        }
    }

    /// <summary>
    /// Deletes an environment from the database.
    /// </summary>
    /// <param name="id">The ID of the environment to delete.</param>
    /// <exception cref="EnvironmentException">Errors if any of the deletion rules aren't met.</exception>
    public async Task DeleteEnvironment(Guid id)
    {
        var environment = await database.Environments.Include(env => env.Servers).FirstAsync(env => env.Id == id);
        if (environment is null)
        {
            throw new EnvironmentException(404, "Environment could not be found with the provided ID.");
        }

        if (environment.Servers.Count > 0)
        {
            throw new EnvironmentException(400, "The environment is unable to be deleted due to it being in-use by one or more servers.");
        }

        try
        {
            database.Environments.Remove(environment);
            await database.SaveChangesAsync();
        }
        catch
        {
            throw new EnvironmentException(500, "Failed to delete environment from database for an unknown reason.");
        }
    }

    /// <summary>
    /// Lists all environments in the database regardless of the server.
    /// </summary>
    /// <returns>The list of environments.</returns>
    public async Task<List<EnvironmentModel>> ListEnvironments()
    {
        return await database.Environments.ToListAsync();
    }
}