using API.Contexts;
using API.Exceptions;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class EnvironmentService(DatabaseContext database)
{
    public async Task CreateEnvironment(EnvironmentModel environment)
    {
        var existingEnvironment = await database.Environments.AnyAsync(env => env.Name == environment.Name);
        if (existingEnvironment)
        {
            throw new EnvironmentException(409, "There is already an environment that exists with the provided name.");
        }
    }

    public async Task<List<EnvironmentModel>> ListEnvironments()
    {
        return await database.Environments.ToListAsync();
    }
}