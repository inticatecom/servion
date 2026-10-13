namespace API.Exceptions;

public class EnvironmentException(int code, string message) : Exception(message)
{
    public int Code { get; } = code;
};