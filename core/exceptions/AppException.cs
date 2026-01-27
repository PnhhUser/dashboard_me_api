using Core.Errors;

namespace Core.Exceptions;

public class AppException : Exception
{
    public string Code { get; }

    public AppException(string code, string message)
        : base(message)
    {
        Code = code;
    }
}
