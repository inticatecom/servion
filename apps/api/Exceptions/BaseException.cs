namespace API.Exceptions;

public class BaseException(int code, string message) : Exception(message)
{
    public int Code { get; } = code;
};