using GenericWebAPI.Models.Errors;
using Microsoft.AspNetCore.Builder;

namespace GenericWebAPI.Configuration;

public static class ExceptionMiddlewareExtension
{
    public static void ConfigureExceptionMiddlewareExtension(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }
}