using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CoreStarter.Api.Middleware
{
    public class RequestValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestValidationMiddleware(RequestDelegate next, ILogger<RequestValidationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Restart();

            await _next(context);

            stopwatch.Stop();

            _logger.LogTrace($"Method: {context.Request.Method}; Path: {context.Request.Path} Execution time: {stopwatch.ElapsedMilliseconds}[ms]");
        }
    }
}