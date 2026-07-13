namespace BuyMoreApi.Application.Notifications
{
    /// <summary>
    /// Simple toggle per channel for the notification orchestrator.
    /// </summary>
    public sealed class NotificationOptions
    {
        public bool EmailEnabled { get; set; } = true;
        public bool LogEnabled { get; set; } = true;
    }
}
