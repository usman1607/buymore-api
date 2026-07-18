using System.Text.Json.Serialization;

namespace BuyMoreApi.Application.Payments.Paystack
{
    /// <summary>
    /// Minimal verification info we care about after Paystack confirms a transaction.
    /// </summary>
    public sealed class PaystackVerifyResponse
    {
        public required string Status { get; init; }
        public required string Reference { get; init; }
        [JsonPropertyName("gateway_response")]
        public required string GatewayResponse { get; init; }
        [JsonPropertyName("authorization_code")]
        public string? AuthorizationCode { get; init; }
    }
}
