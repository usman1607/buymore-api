using System.Threading;
using System.Threading.Tasks;

namespace BuyMoreApi.Application.Notifications
{
    /// <summary>
    /// High-level notification entry point that can fan-out to email, SMS, push, etc.
    /// </summary>
    public interface INotificationService
    {
        Task NotifyUserAsync(string userId, string subject, string message, CancellationToken cancellationToken = default);
    }
}
