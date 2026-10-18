using System.Text.RegularExpressions;

namespace API.Utilities;

public enum EnvironmentVariableDataType
{
    String,
    Bool,
    Int
}

public enum EnvironmentVariableKey
{
    DockerPath,
    DatabaseUrl,
    ServerPort,

    SmtpHost,
    SmtpPort,

    SmtpFromAddress,
    SmtpFromName,

    SmtpUser,
    SmtpPassword
}

public static class EnvironmentVariableUtility
{
    private static readonly Dictionary<EnvironmentVariableKey, EnvironmentVariableDataType> RequiredVariables = new()
    {
        // Base Settings
        [EnvironmentVariableKey.DockerPath] = EnvironmentVariableDataType.String,
        [EnvironmentVariableKey.DatabaseUrl] = EnvironmentVariableDataType.String,
        [EnvironmentVariableKey.ServerPort] = EnvironmentVariableDataType.Int,

        // SMTP Connection
        [EnvironmentVariableKey.SmtpHost] = EnvironmentVariableDataType.String,
        [EnvironmentVariableKey.SmtpPort] = EnvironmentVariableDataType.Int,

        // SMTP Sender Data
        [EnvironmentVariableKey.SmtpFromAddress] = EnvironmentVariableDataType.String,
        [EnvironmentVariableKey.SmtpFromName] = EnvironmentVariableDataType.String,

        // SMTP Authentication
        [EnvironmentVariableKey.SmtpUser] = EnvironmentVariableDataType.String,
        [EnvironmentVariableKey.SmtpPassword] = EnvironmentVariableDataType.String
    };

    private static readonly Dictionary<EnvironmentVariableKey, object> Variables = new();

    private static bool _initialized;

    public static void Initialize()
    {
        if (_initialized) return;

        foreach (var (name, type) in RequiredVariables)
        {
            var variableName = Regex.Replace(name.ToString(), "([a-z])([A-Z])", "$1_$2").ToUpperInvariant();
            var value = Environment.GetEnvironmentVariable(variableName);

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException(
                    $"Required environment variable '{variableName}' is missing."
                );
            }

            object parsed = type switch
            {
                EnvironmentVariableDataType.String => value,

                EnvironmentVariableDataType.Bool =>
                    bool.TryParse(value, out var boolean)
                        ? boolean
                        : throw new FormatException(
                            $"Environment variable '{variableName}' must be a boolean."
                        ),

                EnvironmentVariableDataType.Int =>
                    int.TryParse(value, out var number)
                        ? number
                        : throw new FormatException(
                            $"Environment variable '{variableName}' must be an integer."
                        ),

                _ => throw new ArgumentOutOfRangeException(nameof(type))
            };

            Variables[name] = parsed;
        }

        _initialized = true;
    }

    public static T GetEnvironmentVariable<T>(EnvironmentVariableKey key)
    {
        return (T)Variables[key];
    }
}