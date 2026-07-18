using System.Collections.Generic;

namespace BuyMoreApi.Application.Payments.Paystack
{
    /// <summary>
    /// Payload sent to Paystack when creating a payment session.
    /// </summary>
    public sealed class PaystackInitializeRequest
    {
        public required decimal Amount { get; init; } // Paystack expects amount in kobo
        public required string Email { get; init; }
        public required string Reference { get; init; }
        public string Currency { get; init; } = "NGN";
        public string? CallbackUrl { get; init; }
        public IDictionary<string, string> Metadata { get; init; } = new Dictionary<string, string>();
    }
}
