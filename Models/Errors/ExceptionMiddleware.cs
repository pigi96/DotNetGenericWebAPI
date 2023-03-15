using System.Net;
using GenericWebAPI.Models.Errors;
using Microsoft.AspNetCore.Http;

namespace GenericWebAPI.Models.Errors;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        if (exception is ApiException apiException)
        {
            return context.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = apiException.StatusCode,
                Message = apiException.Message,
                ExceptionDetails = $"Error type: {exception.GetType()}"
            }.ToString());
        }

        return context.Response.WriteAsync(new ErrorDetails
        {
            StatusCode = context.Response.StatusCode,
            Message = $"Runtime Error: {exception.Message}"
        }.ToString());
    }
}