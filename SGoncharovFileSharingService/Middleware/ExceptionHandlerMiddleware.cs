using System.ComponentModel;
using System.Net;
using Newtonsoft.Json;

namespace SGoncharovFileSharingService;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionMessageAsync(context, ex).ConfigureAwait(false);
        }
    }

    private static async Task HandleExceptionMessageAsync(HttpContext context, Exception exception)
    {
        //int statusCode = Int32.Parse((exception.Message.Split("_"))[0]);

        int statusCode = exception switch
        {
            BadRequestException => 400,
            NotFoundException => 404,
            _ => (int)HttpStatusCode.InternalServerError
        };

        var result = JsonConvert.SerializeObject(new
        {
            StatusCode = statusCode,
            ErrorMessage = exception.Message
        });

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(result);
    }
}
