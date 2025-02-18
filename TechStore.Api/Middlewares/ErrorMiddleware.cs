using System.Globalization;
using System.Net;
using System.Text.Json;

namespace TechStore.Api.Middlewares
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorMiddleware> _logger;

        public ErrorMiddleware(RequestDelegate next, ILogger<ErrorMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (KeyNotFoundException ex)  
            {
                _logger.LogWarning(ex, "Resource not found.");
                await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound, "Resource not found.");
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access.");
                await HandleExceptionAsync(context, ex, HttpStatusCode.Unauthorized, "Unauthorized access.");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Bad request: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest, $"Bad request: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }

        private  async Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode, string message)
        {

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsJsonAsync(new { error = message });
        }

    }
}
