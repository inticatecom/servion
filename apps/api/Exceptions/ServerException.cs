namespace API.Exceptions;

public class ServerException(int code, string message) : BaseException(code, message);