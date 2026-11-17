using System.Text.RegularExpressions;

namespace API.Utilities;

/// <summary>
///     Data-types that are accepted as environment variables. Obviously environment variables are by
///     default a string, but they will be parsed into their respective data-types if possible or a
///     validation error will be thrown.
/// </summary>
public enum EnvironmentVariableDataType
{
    String,
    Bool,
    Int
}

/// <summary>
///     An enumerator pointing towards a specific key in the environment.
/// </summary>
public enum EnvironmentVariable
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

/// <summary>
///     Utility functions for reading environment variables loaded into the system.
/// </summary>
public static class EnvironmentVariableUtility
{
    /// <summary>
    ///     The environment variables that are required by the system.
    /// </summary>
    private static readonly Dictionary<EnvironmentVariable, EnvironmentVariableDataType> RequiredVariables = new()
    {
        // Base Settings
        [EnvironmentVariable.DockerPath] = EnvironmentVariableDataType.String,
        [EnvironmentVariable.DatabaseUrl] = EnvironmentVariableDataType.String,
        [EnvironmentVariable.ServerPort] = EnvironmentVariableDataType.Int,

        // SMTP Connection
        [EnvironmentVariable.SmtpHost] = EnvironmentVariableDataType.String,
        [EnvironmentVariable.SmtpPort] = EnvironmentVariableDataType.Int,

        // SMTP Sender Data
        [EnvironmentVariable.SmtpFromAddress] = EnvironmentVariableDataType.String,
        [EnvironmentVariable.SmtpFromName] = EnvironmentVariableDataType.String,

        // SMTP Authentication
        [EnvironmentVariable.SmtpUser] = EnvironmentVariableDataType.String,
        [EnvironmentVariable.SmtpPassword] = EnvironmentVariableDataType.String
    };

    private static readonly Dictionary<EnvironmentVariable, object> Variables = new();

    private static bool _initialized;

    /// <summary>
    ///     Initializes & validates all environment variables.
    /// </summary>
    /// 
    /// <exception cref="InvalidOperationException">If the operation fails.</exception>
    /// <exception cref="FormatException">If the format is not met.</exception>
    /// <exception cref="ArgumentOutOfRangeException">If the value does not meet the requirements.</exception>
    public static void Initialize()
    {
        if (_initialized) return;

        foreach (var (name, type) in RequiredVariables)
        {
#pragma warning disable SYSLIB1045
            var variableName = Regex.Replace(name.ToString(), "([a-z])([A-Z])", "$1_$2").ToUpperInvariant();
#pragma warning restore SYSLIB1045

            var value = Environment.GetEnvironmentVariable(variableName);

            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidOperationException(
                    $"Required environment variable '{variableName}' is missing."
                );

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

    /// <summary>
    ///     Gets the value of the specified environment variable.
    /// </summary>
    /// 
    /// <param name="key">The environment variable key.</param>
    /// 
    /// <typeparam name="T">The expected data-type of the returned value (e.g. string).</typeparam>
    /// 
    /// <returns>The value of the environment variable.</returns>
    public static T GetEnvironmentVariable<T>(EnvironmentVariable key)
    {
        return (T)Variables[key];
    }
}