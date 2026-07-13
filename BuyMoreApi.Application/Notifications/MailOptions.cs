namespace BuyMoreApi.Application.Notifications
{
    /// <summary>
    /// SMTP configuration needed by the mail service. Values are provided via appsettings.
    /// </summary>
    public sealed class MailOptions
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } = 587;
        public bool EnableSsl { get; set; } = true;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FromEmail { get; set; } = string.Empty;
        public string FromName { get; set; } = "BuyMore Team";
    }
}
