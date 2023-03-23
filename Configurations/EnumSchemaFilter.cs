using System.Runtime.Serialization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GenericWebAPI.Configurations;

public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            schema.Enum.Clear();

            foreach (var enumName in Enum.GetNames(context.Type))
            {
                var memberInfo = context.Type.GetMember(enumName).FirstOrDefault(m => m.DeclaringType == context.Type);
                var enumMemberAttribute = memberInfo?.GetCustomAttributes(typeof(EnumMemberAttribute), false)
                    .OfType<EnumMemberAttribute>().FirstOrDefault();
                var label = enumMemberAttribute?.Value ?? enumName;

                schema.Enum.Add(new OpenApiString(label));
            }
        }
    }
}