namespace API.Exceptions;

/// <summary>
///     An exception relating to an error with the user process.
/// </summary>
public class UserException(int code, string message) : BaseException(code, message);