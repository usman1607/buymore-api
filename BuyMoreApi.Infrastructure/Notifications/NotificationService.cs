using System.Threading;
using System.Threading.Tasks;
using BuyMoreApi.Application.Notifications;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BuyMoreApi.Infrastructure.Notifications
{
    /// <summary>
    /// Coordinates notification delivery and keeps cross-cutting concerns (logging, toggles) in one place.
    /// </summary>
    public sealed class NotificationService : INotificationService
    {
        private readonly NotificationOptions _options;
        private readonly IMailService _mailService;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(IOptions<NotificationOptions> options, IMailService mailService, ILogger<NotificationService> logger)
        {
            _options = options.Value;
            _mailService = mailService;
            _logger = logger;
        }

        public async Task NotifyUserAsync(string userId, string subject, string message, CancellationToken cancellationToken = default)
        {
            if (_options.EmailEnabled)
            {
                await _mailService.SendEmailAsync(new MailRequest
                {
                    Subject = subject,
                    Body = message,
                    To = userId
                }, cancellationToken);
            }

            if (_options.LogEnabled)
            {
                _logger.LogInformation("Notification sent to {UserId}: {Subject}", userId, subject);
            }
        }
    }
}
