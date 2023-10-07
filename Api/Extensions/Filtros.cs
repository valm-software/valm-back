using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace Api.Extensions
{
    public class AcceptHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var accepts = context.ApiDescription.SupportedResponseTypes
                .SelectMany(rt => rt.ApiResponseFormats)
                .Select(responseFormat => responseFormat.MediaType)
                .Distinct();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Accept",
                In = ParameterLocation.Header,
                Description = "Supported media types",
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Enum = accepts.Select(a => new OpenApiString(a)).ToList<IOpenApiAny>()
                }
            });
        }
    }
    //public class AcceptHeaderOperationFilter : IOperationFilter
    //{
    //    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    //    {
    //        if (operation.Parameters == null)
    //            operation.Parameters = new List<OpenApiParameter>();

    //        var acceptHeaderParameter = new OpenApiParameter
    //        {
    //            Name = "Accept",
    //            In = ParameterLocation.Header,
    //            Required = true,
    //            Schema = new OpenApiSchema { Type = "string", Default = new OpenApiString("application/xml") }
    //        };

    //        operation.Parameters.Add(acceptHeaderParameter);
    //    }
    //}





}
