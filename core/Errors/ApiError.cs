namespace Core.Errors;

public class ApiError
{
    public string Code { get; set; }

    public int Status { get; set; }
    public string Message { get; set; }

    public ApiError(string code, string message, int status)
    {
        Code = code;
        Message = message;
        Status = status;
    }
}
