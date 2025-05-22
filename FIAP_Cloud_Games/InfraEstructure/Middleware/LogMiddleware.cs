using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace InfraEstructure.Middleware
{
    public class LogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogMiddleware> _logger;

        public LogMiddleware(RequestDelegate next, ILogger<LogMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            // Log de início de requisição
            _logger.LogInformation("[INÍCIO] HTTP {method} - {url}", context.Request.Method, context.Request.Path);

            try
            {
                await _next(context);
                stopwatch.Stop();

                // Log de fim de requisição com status
                _logger.LogInformation(
                    "[FIM] HTTP {method} - {url} responded {statusCode} in {elapsed}ms",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    stopwatch.ElapsedMilliseconds
                );
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                // Log de erro
                _logger.LogError(
                    ex,
                    "[ERRO] HTTP {method} - {url} failed in {elapsed}ms",
                    context.Request.Method,
                    context.Request.Path,
                    stopwatch.ElapsedMilliseconds
                );

                // Repropaga exceção
                throw;
            }
        }
    }
}