using API.Contexts;
using API.Exceptions;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class EnvironmentService(DatabaseContext database)
{
    public async Task Create(EnvironmentModel environment)
    {
        var existingEnvironment = await database.Environments.Where(env => env.Name == environment.Name).FirstAsync();
        if (existingEnvironment is not null)
        {
            throw new EnvironmentException("There is already an environment that exists with the provided name.");
        }
    }
}