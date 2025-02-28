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
                LogRequestDetails(context, $"Resource not found: {ex.Message}");
                await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound, "Resource not found.");
            }
            catch (UnauthorizedAccessException ex)
            {
                LogRequestDetails(context, $"Unauthorized access: {ex.Message}");
                await HandleExceptionAsync(context, ex, HttpStatusCode.Unauthorized, "Unauthorized access.");
            }
            catch (ArgumentException ex)
            {
                LogRequestDetails(context, $"Bad request: {ex.Message}");
                await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest, $"Bad request: {ex.Message}");
            }
            catch (Exception ex)
            {
                LogRequestDetails(context, ex.Message);
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }
        private void LogRequestDetails(HttpContext context, string message)
        {
            var requestId = context.TraceIdentifier;
            var clientIp = context.Connection.RemoteIpAddress?.ToString();
            var endpoint = context.Request;
            var error = $@"======== Error Log ========
Message: {message}
Request ID: {context.TraceIdentifier}
Client IP: {context.Connection.RemoteIpAddress}
Endpoint: {context.Request?.Method} {context.Request?.Path}
=============================";
            _logger.LogWarning(error);
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode, string message)
        {

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsJsonAsync(new { error = message });
        }

    }
}
