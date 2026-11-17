using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[Index(nameof(Name), IsUnique = true)]
public class EnvironmentModel : IdentityModel
{
    /// <summary>
    ///     The servers that use the environment.
    /// </summary>
    public ICollection<ServerModel> Servers { get; set; } = new List<ServerModel>();

    /// <summary>
    ///     The name of the environment.
    /// </summary>
    [StringLength(30, MinimumLength = 3)]
    public required string Name { get; init; }

    /// <summary>
    ///     The description of the environment.
    /// </summary>
    [StringLength(200, MinimumLength = 3)]
    public required string Description { get; init; }

    /// <summary>
    ///     The current version of the environment.
    /// </summary>
    [StringLength(30, MinimumLength = 5)]
    public required string Version { get; init; }

    /// <summary>
    ///     The author information of the environment.
    /// </summary>
    [Column(TypeName = "jsonb")]
    public required EnvironmentAuthor Author { get; init; }

    /// <summary>
    ///     The available images that user's can select when creating a server that uses the environment.
    /// </summary>
    [Column(TypeName = "jsonb")]
    public required Dictionary<string, string> Images { get; init; }

    /// <summary>
    ///     The files to store persistently (maintain data across reboots) for the environment.
    /// </summary>
    public required List<string> Files { get; init; }

    /// <summary>
    ///     The startup/install commands for servers that use the environment.
    /// </summary>
    [Column(TypeName = "jsonb")]
    public required EnvironmentStartup Startup { get; init; }

    /// <summary>
    ///     The configurable parameters for servers that use the environment.
    /// </summary>
    [Column(TypeName = "jsonb")]
    public required Dictionary<string, EnvironmentParameter> Parameters { get; init; }

    /// <summary>
    ///     Non-configurable environment variables that get applied to the image for servers that use the environment.
    /// </summary>
    [Column(TypeName = "jsonb")]
    public required Dictionary<string, JsonElement> DefaultVariables { get; init; }
}

/// <summary>
///     An author of an environment.
/// </summary>
/// <param name="Name">The name of the author.</param>
/// <param name="Url">The URL to the source of the author.</param>
public record EnvironmentAuthor(
    [StringLength(30, MinimumLength = 3)] string Name,
    [StringLength(30, MinimumLength = 3)] string Url
);

/// <summary>
///     The startup/installation commands for the environment.
/// </summary>
/// <param name="Install">The installation command.</param>
/// <param name="Initialize">The initialization command.</param>
public record EnvironmentStartup(string Install, string Initialize);

/// <summary>
///     The data types an environment parameter can have.
/// </summary>
public enum EnvironmentParameterType
{
    String,
    Number,
    Boolean,
    Dropdown
}

/// <summary>
///     The structure of an environment parameter.
/// </summary>
/// <param name="Name">The name of the parameter.</param>
/// <param name="Type">The type of parameter.</param>
/// <param name="Required">Whether the parameter is required or not.</param>
/// <param name="Placeholder">The placeholder/hint for the parameter.</param>
/// <param name="DefaultValue">The default value the parameter has.</param>
/// <param name="Values">The accepted values that the parameter can have (only used if the parameter type is a dropdown).</param>
public record EnvironmentParameter(
    string Name,
    EnvironmentParameterType Type,
    bool? Required,
    string? Placeholder,
    string? DefaultValue,
    List<EnvironmentParameterValue>? Values
);

/// <summary>
///     A value of a parameter value list.
/// </summary>
/// <param name="Name">The name of the value.</param>
/// <param name="Value">The actual value.</param>
public record EnvironmentParameterValue([StringLength(30, MinimumLength = 1)] string Name, [MinLength(1)] string Value);