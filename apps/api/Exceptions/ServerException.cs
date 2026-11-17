namespace API.Exceptions;

/// <summary>
///     An exception relating to an error with the server process.
/// </summary>
public class ServerException(int code, string message) : BaseException(code, message);