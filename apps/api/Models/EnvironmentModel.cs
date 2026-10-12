using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace API.Models;

public class EnvironmentModel : IdentityModel
{
    public ICollection<ServerModel> Servers { get; set; } = new List<ServerModel>();
    [StringLength(30, MinimumLength = 3)] public required string Name { get; init; }

    public required string Description { get; init; }

    public required string Version { get; init; }

    public required EnvironmentAuthor Author { get; init; }

    public required Dictionary<string, string> Images { get; init; }

    public required List<string> Files { get; init; }

    public required EnvironmentStartup Startup { get; init; }

    public required Dictionary<string, EnvironmentParameter> Parameters { get; init; }

    public required Dictionary<string, JsonElement> Variables { get; init; }
}

public record EnvironmentAuthor(
    [property: StringLength(30, MinimumLength = 3)]
    string Name,
    [property: StringLength(30, MinimumLength = 3)]
    string Url
);

public record EnvironmentStartup(string Install, string Command);

public enum EnvironmentParameterType
{
    String,
    Number,
    Boolean
}

public record EnvironmentParameter(
    string Name,
    EnvironmentStartup Type,
    bool Required,
    string Placeholder,
    string DefaultValue,
    List<EnvironmentParameterValue> Value
);

public record EnvironmentParameterValue(string Name, string Value);