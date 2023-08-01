using System.Text.Json;

namespace DisciApp.Api.CustomExceptionMiddleware
{
    public class ApiResponse
    {
       
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
