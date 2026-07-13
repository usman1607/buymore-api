using System.Threading;
using System.Threading.Tasks;

namespace BuyMoreApi.Application.Notifications
{
    /// <summary>
    /// Contract for sending transactional emails. Concrete transports (SMTP, SendGrid, etc.) sit in infrastructure.
    /// </summary>
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest request, CancellationToken cancellationToken = default);
    }
}
