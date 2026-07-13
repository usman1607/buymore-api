using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using BuyMoreApi.Application.Notifications;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BuyMoreApi.Infrastructure.Notifications
{
    /// <summary>
    /// Basic SMTP implementation that reads its settings from configuration.
    /// </summary>
    public sealed class SmtpMailService : IMailService
    {
        private readonly MailOptions _options;
        private readonly ILogger<SmtpMailService> _logger;

        public SmtpMailService(IOptions<MailOptions> options, ILogger<SmtpMailService> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(MailRequest request, CancellationToken cancellationToken = default)
        {
            using var message = BuildMailMessage(request);
            using var client = BuildClient();

            _logger.LogInformation("Sending email to {Recipient} with subject {Subject}", request.To, request.Subject);

            await Task.Run(() => client.Send(message), cancellationToken);
        }

        private MailMessage BuildMailMessage(MailRequest request)
        {
            var from = new MailAddress(_options.FromEmail, _options.FromName);
            var to = new MailAddress(request.To);

            var message = new MailMessage(from, to)
            {
                Subject = request.Subject,
                Body = request.Body,
                IsBodyHtml = request.IsBodyHtml
            };

            if (!string.IsNullOrWhiteSpace(request.ReplyTo))
            {
                message.ReplyToList.Add(new MailAddress(request.ReplyTo));
            }

            foreach (var attachment in request.Attachments)
            {
                var stream = new System.IO.MemoryStream(attachment.Value);
                message.Attachments.Add(new Attachment(stream, attachment.Key));
            }

            return message;
        }

        private SmtpClient BuildClient()
        {
            return new SmtpClient(_options.Host, _options.Port)
            {
                EnableSsl = _options.EnableSsl,
                Credentials = new NetworkCredential(_options.UserName, _options.Password)
            };
        }
    }
}
