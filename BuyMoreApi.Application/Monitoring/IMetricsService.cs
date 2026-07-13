using System.Collections.Generic;

namespace BuyMoreApi.Application.Monitoring
{
    /// <summary>
    /// Abstraction for recording lightweight diagnostics that higher layers can expose as metrics or dashboards.
    /// </summary>
    public interface IMetricsService
    {
        void TrackRequest(string route, double durationMilliseconds, int statusCode);

        IReadOnlyCollection<RequestMetric> GetSnapshot();
    }
}
