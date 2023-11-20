using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GenericWebAPI.Utilities;


public class SummaryOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Get the method description from the method's XML documentation
        var controllerActionDescriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;
        if (controllerActionDescriptor == null) return;

        var methodInfo = controllerActionDescriptor.MethodInfo;
        var summary = methodInfo.GetCustomAttribute<System.ComponentModel.DescriptionAttribute>()?.Description;

        // Set the method summary in the Swagger documentation
        if (!string.IsNullOrEmpty(summary))
        {
            operation.Description = summary;
        }

    }
}
