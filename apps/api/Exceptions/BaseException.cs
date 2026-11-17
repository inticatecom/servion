namespace API.Exceptions;

/// <summary>
/// A generic exception, primarily extends into sub exceptions.
/// </summary>
/// 
/// <param name="code">The code for the exception.</param>
/// <param name="message">The public message for the exception.</param>
public class BaseException(int code, string message) : Exception(message)
{
    /// <summary>
    ///     The exception code.
    /// </summary>
    public int Code { get; } = code;
}