using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace API.Models;

public class EnvironmentModel : IdentityModel
{
    public ICollection<ServerModel> Servers { get; set; } = new List<ServerModel>();
    [StringLength(30, MinimumLength = 3)] public required string Name { get; init; }

    public required string Description { get; init; }

    public required string Version { get; init; }

    [Column(TypeName = "jsonb")] public required EnvironmentAuthor Author { get; init; }

    [Column(TypeName = "jsonb")] public required Dictionary<string, string> Images { get; init; }

    public required List<string> Files { get; init; }

    [Column(TypeName = "jsonb")] public required EnvironmentStartup Startup { get; init; }

    [Column(TypeName = "jsonb")] public required Dictionary<string, EnvironmentParameter> Parameters { get; init; }
    [Column(TypeName = "jsonb")] public required Dictionary<string, JsonElement> Variables { get; init; }
}

public record EnvironmentAuthor(
    [StringLength(30, MinimumLength = 3)]
    string Name,
    [StringLength(30, MinimumLength = 3)]
    string Url
);

public record EnvironmentStartup(string Install, string Initialize);

public enum EnvironmentParameterType
{
    String,
    Number,
    Boolean,
    Dropdown
}

public record EnvironmentParameter(
    string Name,
    EnvironmentParameterType Type,
    bool? Required,
    string? Placeholder,
    string? DefaultValue,
    List<EnvironmentParameterValue>? Values
);

public record EnvironmentParameterValue(string Name, string Value);