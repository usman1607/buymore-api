using System;

namespace BuyMoreApi.Application.Monitoring
{
    /// <summary>
    /// Represents a single HTTP request measurement captured by the monitoring middleware.
    /// Keeping the model in the application layer means any host (web, worker, etc.) can reuse it.
    /// </summary>
    public sealed class RequestMetric
    {
        public required string Route { get; init; }
        public required TimeSpan Duration { get; init; }
        public required int StatusCode { get; init; }
        public DateTime ObservedAtUtc { get; init; } = DateTime.UtcNow;
    }
}
