namespace Core.Responses;

public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; set; }

    public ApiResponse(T? data, string? message = null)
        : base(true, message)
    {
        Data = data;
    }



    public static ApiResponse<T> Ok(T data, string? message = null)
        => new(data, message);

}
