namespace Core.Responses;

public class ApiResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }

    public ApiResponse(bool success, string? message = null)
    {
        Success = success;
        Message = message;
    }

    public static ApiResponse Ok(string? message = null)
        => new(true, message);

    public static ApiResponse Fail(string message)
        => new(false, message);
}
