using System.Globalization;
using API.Contexts;
using API.Exceptions;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

/// <summary>
///     A service for interacting with the environments loaded into the system.
/// </summary>
public sealed class EnvironmentService(DatabaseContext database)
{
    /// <summary>
    ///     Safely creates a new environment in the database.
    /// </summary>
    /// <param name="environment">The environment to save.</param>
    /// <exception cref="EnvironmentException">Errors if any of the safety rules aren't met.</exception>
    public async Task CreateEnvironment(EnvironmentModel environment)
    {
        var existingEnvironment = await database.Environments.AnyAsync(env => env.Name == environment.Name);
        if (existingEnvironment) throw new EnvironmentException(409, "There is already an environment that exists with the provided name.");

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
    ///     Fetches a specific environment.
    /// </summary>
    /// <param name="id">The ID of the environment.</param>
    /// <returns>The environment.</returns>
    /// <exception cref="EnvironmentException"></exception>
    public async Task<EnvironmentModel> GetEnvironmentById(Guid id)
    {
        return await database.Environments.FindAsync(id) ?? throw new EnvironmentException(404, "Could not find the environment with the provided ID.");
    }

    /// <summary>
    ///     Deletes an environment from the database.
    /// </summary>
    /// <param name="id">The ID of the environment to delete.</param>
    /// <exception cref="EnvironmentException">Errors if any of the deletion rules aren't met.</exception>
    public async Task DeleteEnvironment(Guid id)
    {
        var environment = await database.Environments.Include(env => env.Servers).FirstAsync(env => env.Id == id);
        if (environment is null) throw new EnvironmentException(404, "Environment could not be found with the provided ID.");

        if (environment.Servers.Count > 0) throw new EnvironmentException(400, "The environment is unable to be deleted due to it being in-use by one or more servers.");

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
    ///     Ensures that a server's variables meets the requirements of an environment.
    /// </summary>
    /// <param name="variables">The server's variables.</param>
    /// <param name="environmentId">The environment ID.</param>
    /// <returns>The result of the validation.</returns>
    public async Task<EnvironmentParametersValidationResult> EnsureServerMeetsEnvironmentParameters(Dictionary<string, ServerVariable> variables, Guid environmentId)
    {
        var errors = new List<string>();
        var parameters = (await GetEnvironmentById(environmentId)).Parameters;

        if (variables.Select(param => param.Value.Name).Distinct().Count() != variables.Count) errors.Add("Includes duplicate variables.");

        foreach (var (_, parameter) in parameters)
            if (parameter.Required == true && variables.All(value => value.Value.Name != parameter.Name))
                errors.Add($"Required variable '{parameter.Name} is missing.");

        // Validate data-types.
        foreach (var (_, variable) in variables)
        {
            if (!parameters.TryGetValue(variable.Name, out var parameter))
            {
                errors.Add("");
                continue;
            }

            var validDataType = parameter.Type switch
            {
                EnvironmentParameterType.String => true,

                EnvironmentParameterType.Boolean =>
                    bool.TryParse(variable.Value, out _),

                EnvironmentParameterType.Number =>
                    double.TryParse(
                        variable.Value,
                        NumberStyles.Float,
                        CultureInfo.InvariantCulture,
                        out _
                    ),

                _ => false
            };

            if (!validDataType) errors.Add($"Variable '{variable.Name}' must be of type '{parameter.Type}'.");
        }

        return new EnvironmentParametersValidationResult(errors.Count < 1, errors);
    }

    /// <summary>
    ///     Lists all environments in the database regardless of the server.
    /// </summary>
    /// <returns>The list of environments.</returns>
    public async Task<List<EnvironmentModel>> ListEnvironments()
    {
        return await database.Environments.ToListAsync();
    }
}

/// <summary>
///     The result for a server-environment validation.
/// </summary>
/// <param name="Success">Whether the operation succeeded or not.</param>
/// <param name="Errors">A list of errors.</param>
public record EnvironmentParametersValidationResult(bool Success, List<string> Errors);