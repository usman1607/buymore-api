using System.Diagnostics;
using System.Text;
using BuyMoreApi.Application.Monitoring;
using Microsoft.Extensions.Options;

namespace BuyMoreApi.API.Middlewares
{
    /// <summary>
    /// Logs incoming requests and pushes timing information to the metrics service.
    /// Kept small on purpose so students can trace the full request lifecycle.
    /// </summary>
    public sealed class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;
        private readonly IMetricsService _metricsService;
        private readonly MonitoringOptions _options;

        public RequestLoggingMiddleware(
            RequestDelegate next,
            ILogger<RequestLoggingMiddleware> logger,
            IMetricsService metricsService,
            IOptions<MonitoringOptions> options)
        {
            _next = next;
            _logger = logger;
            _metricsService = metricsService;
            _options = options.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            string? requestBody = null;

            if (_options.CaptureRequestBody && context.Request.ContentLength > 0 && context.Request.Body.CanSeek)
            {
                context.Request.EnableBuffering();
                using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
                requestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            await _next(context);
            stopwatch.Stop();

            var route = $"{context.Request.Method} {context.Request.Path}";
            _metricsService.TrackRequest(route, stopwatch.Elapsed.TotalMilliseconds, context.Response.StatusCode);

            _logger.LogInformation(
                "Request {Route} completed with {StatusCode} in {ElapsedMilliseconds:0.000} ms {Body}",
                route,
                context.Response.StatusCode,
                stopwatch.Elapsed.TotalMilliseconds,
                string.IsNullOrWhiteSpace(requestBody) ? string.Empty : $"Payload: {requestBody}");
        }
    }
}
