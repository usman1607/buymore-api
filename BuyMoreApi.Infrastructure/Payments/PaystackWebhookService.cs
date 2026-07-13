using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BuyMoreApi.Application.Notifications;
using BuyMoreApi.Application.Payments.Paystack;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BuyMoreApi.Infrastructure.Payments
{
    /// <summary>
    /// Verifies Paystack webhook signatures and raises lightweight notifications for downstream processing.
    /// </summary>
    public sealed class PaystackWebhookService : IPaystackWebhookService
    {
        private readonly PaystackOptions _options;
        private readonly ILogger<PaystackWebhookService> _logger;
        private readonly INotificationService _notificationService;

        public PaystackWebhookService(
            IOptions<PaystackOptions> options,
            ILogger<PaystackWebhookService> logger,
            INotificationService notificationService)
        {
            _options = options.Value;
            _logger = logger;
            _notificationService = notificationService;
        }

        public async Task HandleAsync(JsonDocument payload, string signature, string rawBody, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(signature))
            {
                throw new InvalidOperationException("Missing Paystack signature header.");
            }

            if (!IsSignatureValid(signature, rawBody))
            {
                throw new InvalidOperationException("Invalid Paystack signature.");
            }

            var eventName = payload.RootElement.GetProperty("event").GetString() ?? "unknown";
            _logger.LogInformation("Received Paystack webhook for event {Event}", eventName);

            // For now we notify via email/logging so students can plug in domain-specific behavior later.
            await _notificationService.NotifyUserAsync(
                userId: _options.PublicKey,
                subject: $"Paystack webhook: {eventName}",
                message: payload.RootElement.ToString(),
                cancellationToken);
        }

        private bool IsSignatureValid(string signature, string rawBody)
        {
            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(_options.SecretKey));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawBody));
            var computed = BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();
            return string.Equals(computed, signature, StringComparison.OrdinalIgnoreCase);
        }
    }
}
