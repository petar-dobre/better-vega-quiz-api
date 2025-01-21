using System.ComponentModel.DataAnnotations;
using System.Net;
using Newtonsoft.Json;
using QuizWebApp.Exceptions;

namespace QuizWebApp.Middleware;

public class ErrorResult
{
    [Required]
    public readonly bool Success = false;

    [Required]
    public required string Message { get; set; }
}

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
        catch (UnauthorizedAccessException ex)
        {
            await HandleException(context, ex.Message, HttpStatusCode.Unauthorized);
        }
        catch (NotFoundException ex)
        {
            await HandleException(context, ex.Message, HttpStatusCode.NotFound);
        }
        catch (AlreadyExistsException ex)
        {
            await HandleException(context, ex.Message, HttpStatusCode.Conflict);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex.Message, HttpStatusCode.InternalServerError);
        }
    }

    private static async Task HandleException(HttpContext context, string message, HttpStatusCode statusCode)
    {
        var errorResult = new ErrorResult
        {
            Message = message
        };

        var response = context.Response;
        response.StatusCode = (int)statusCode;
        response.ContentType = "application/json";
        await response.WriteAsync(JsonConvert.SerializeObject(errorResult));
    }
}