using System.Collections.Generic;

namespace BuyMoreApi.Application.Notifications
{
    /// <summary>
    /// Immutable description of an email message so that different mail providers can process the same payload.
    /// </summary>
    public sealed class MailRequest
    {
        public required string Subject { get; init; }
        public required string Body { get; init; }
        public required string To { get; init; }
        public string? ReplyTo { get; init; }
        public bool IsBodyHtml { get; init; } = true;
        public IDictionary<string, byte[]> Attachments { get; init; } = new Dictionary<string, byte[]>();
    }
}
