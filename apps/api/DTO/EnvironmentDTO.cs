using System.ComponentModel.DataAnnotations;
using API.Models;

namespace API.DTO;

/// <summary>
///     DTO's for environments.
/// </summary>
public static class EnvironmentDto
{
    /// <summary>
    ///     The data required to create a new environment.
    /// </summary>
    /// <param name="Data">The data for the new environment.</param>
    public record Create([Required] EnvironmentModel Data);

    /// <summary>
    ///     The data required to delete an existing environment.
    /// </summary>
    /// <param name="Id">The ID of the environment.</param>
    public record Delete([Required] Guid Id);
}