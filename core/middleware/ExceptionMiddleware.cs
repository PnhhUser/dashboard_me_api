using System.Net;
using System.Text.Json;
using Core.Errors;
using Core.Exceptions;

namespace Core.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (AppException ex)
        {
            await HandleAppException(context, ex);
        }
        catch (Exception ex)
        {
            await HandleUnknownException(context, ex);
        }
    }

    private static async Task HandleAppException(HttpContext context, AppException ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = MapStatusCode(ex.Code);

        var response = new
        {
            success = false,
            error = new ApiError(ex.Code, ex.Message, context.Response.StatusCode)
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static async Task HandleUnknownException(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new
        {
            success = false,
            error = new ApiError(
                ErrorCode.InternalError,
                "An unexpected error occurred",
                context.Response.StatusCode
            )
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static int MapStatusCode(string errorCode)
    {
        return errorCode switch
        {
            ErrorCode.NotFound => (int)HttpStatusCode.NotFound,
            ErrorCode.Unauthorized => (int)HttpStatusCode.Unauthorized,
            ErrorCode.Forbidden => (int)HttpStatusCode.Forbidden,
            ErrorCode.ValidationError => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.BadRequest
        };
    }

}

