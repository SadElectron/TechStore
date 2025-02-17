namespace TechStore.Api.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorMiddleware>();
        }

    }
}
