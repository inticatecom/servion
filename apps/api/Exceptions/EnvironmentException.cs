namespace API.Exceptions;

/// <summary>
///     An exception relating to an issue with an environment process.
/// </summary>
public class EnvironmentException(int code, string message) : BaseException(code, message);