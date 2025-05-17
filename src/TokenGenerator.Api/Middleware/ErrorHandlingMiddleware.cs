using System.Net;
using TokenGenerator.Domain.Errors;

namespace TokenGenerator.Api.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception)
            {
                await HandleExceptionAsync(context);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            if (context != null)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)statusCode;
                await context.Response.WriteAsJsonAsync(new Error
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Internal Server Error"
                });
            }
        }
    }
}
