namespace API.Exceptions;

public class EnvironmentException(int code, string message) : BaseException(code, message);