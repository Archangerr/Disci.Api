using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DisciApp.Api.CustomHealthCheck
{
    public class WriteResponse
    {
        public static Task WriteResponseAsync(HttpContext httpContext,
                       HealthReport result)
        {
            httpContext.Response.ContentType = MediaTypeNames.Application.Json;
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter() }
            };
            var json = JsonSerializer.Serialize(result, options);   
            return httpContext.Response.WriteAsync(json);
        }
    }
}
