using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BuyMoreApi.Application.Payments.Paystack
{
    /// <summary>
    /// Handles incoming Paystack webhook notifications.
    /// </summary>
    public interface IPaystackWebhookService
    {
        Task HandleAsync(JsonDocument payload, string signature, string rawBody, CancellationToken cancellationToken = default);
    }
}
