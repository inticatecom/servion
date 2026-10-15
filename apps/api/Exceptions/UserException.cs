namespace API.Exceptions;

public class UserException(int code, string message) : BaseException(code, message);