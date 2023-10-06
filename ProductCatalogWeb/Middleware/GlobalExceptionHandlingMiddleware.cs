using System.Net;

namespace ProductCatalogWeb.Middleware
{
	public class GlobalExceptionHandlingMiddleware : IMiddleware
	{
		private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
        public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger) => _logger = logger;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                string message = $"Response: {context.Response.StatusCode} - Error: {ex.Message} - StackTrace:{ex.StackTrace}";
                _logger.LogError(ex, message);
            }
        }
    }
}
