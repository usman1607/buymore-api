using System;
using System.Collections.Concurrent;
using BuyMoreApi.Application.Monitoring;
using Microsoft.Extensions.Options;

namespace BuyMoreApi.Infrastructure.Monitoring
{
    /// <summary>
    /// Thread-safe in-memory metrics buffer. Lightweight but good enough for dashboards during development.
    /// </summary>
    public sealed class MetricsService : IMetricsService
    {
        private readonly MonitoringOptions _options;
        private readonly ConcurrentQueue<RequestMetric> _buffer = new();

        public MetricsService(IOptions<MonitoringOptions> options)
        {
            _options = options.Value;
        }

        public IReadOnlyCollection<RequestMetric> GetSnapshot()
        {
            if (!_options.Enabled)
            {
                return Array.Empty<RequestMetric>();
            }

            return _buffer.ToArray();
        }

        public void TrackRequest(string route, double durationMilliseconds, int statusCode)
        {
            if (!_options.Enabled)
            {
                return;
            }

            var metric = new RequestMetric
            {
                Route = route,
                Duration = TimeSpan.FromMilliseconds(durationMilliseconds),
                StatusCode = statusCode
            };

            _buffer.Enqueue(metric);

            while (_buffer.Count > _options.MaxStoredEntries && _buffer.TryDequeue(out _))
            {
                // we intentionally drain old entries to keep memory bounded
            }
        }
    }
}
