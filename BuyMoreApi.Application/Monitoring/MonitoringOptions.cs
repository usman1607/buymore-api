namespace BuyMoreApi.Application.Monitoring
{
    /// <summary>
    /// Configuration slice for request logging/monitoring. Bound directly from appsettings.
    /// </summary>
    public sealed class MonitoringOptions
    {
        public bool Enabled { get; set; } = true;
        public bool CaptureRequestBody { get; set; }
        public int MaxStoredEntries { get; set; } = 500;
    }
}
