using System.Text.Json.Serialization;

namespace BuyMoreApi.Application.Payments.Paystack
{
    /// <summary>
    /// Key bits the frontend needs after a Paystack initialization call.
    /// </summary>
    public sealed class PaystackInitializeResponse
    {
        [JsonPropertyName("authorization_url")]
        public required string AuthorizationUrl { get; init; }
        [JsonPropertyName("access_code")]
        public required string AccessCode { get; init; }
        [JsonPropertyName("reference")]
        public required string Reference { get; init; }
    }
}
