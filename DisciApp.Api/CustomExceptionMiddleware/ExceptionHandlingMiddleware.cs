using System.Net;
using System.Text.Json;

namespace DisciApp.Api.CustomExceptionMiddleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
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

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;

            var errorResponse = new ApiResponse
            {
                Success = false
            };

            string[] stringsToCheck = { "Tarih formati hatali", "Tarih sadece gun bilgisi iceriyor olmali", "Tarih sadece gun bilgisi iceriyor olmali"
            ,"Mesai saati disinda"};
            switch (exception)
            {
                
                case ApplicationException ex:
                   foreach(string s in stringsToCheck)
                    {
                        if (ex.Message.Contains(s))
                        {
                            response.StatusCode = (int)HttpStatusCode.Forbidden;
                            errorResponse.Message = ex.Message;
                            break;
                        }
                    }
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = ex.Message;
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Message = "Internal server error!";
                    break;
            }
            _logger.LogError(exception.Message);
            var result = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(result);
        }
    }
}
