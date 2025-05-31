using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FIAP_Cloud_Games.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class LogMiddleware
    {
        private readonly ILogger<LogMiddleware> _logger;
        private readonly RequestDelegate _next;

        public LogMiddleware(RequestDelegate next, ILogger<LogMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {

            var request = httpContext.Request;
            _logger.LogInformation($"[{DateTime.UtcNow}] {request.Method} {request.Path}");
            await _next(httpContext);

            var response = httpContext.Response;
            var statusCode = response.StatusCode;

            var status = statusCode >= 200 && statusCode < 300 ? "Sucesso" :
                statusCode >= 400 && statusCode < 500 ? "Erro do cliente" :
                statusCode >= 500 ? "Erro do servidor" :
                "Desconhecido";

            _logger.LogInformation($"[{DateTime.UtcNow}] {statusCode} {status} {request.Method} {request.Path}");
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class LogMiddlewareExtensions
    {
        public static IApplicationBuilder UseLogMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogMiddleware>();
        }
    }
}
