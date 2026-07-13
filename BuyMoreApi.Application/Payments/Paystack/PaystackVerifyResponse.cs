namespace BuyMoreApi.Application.Payments.Paystack
{
    /// <summary>
    /// Minimal verification info we care about after Paystack confirms a transaction.
    /// </summary>
    public sealed class PaystackVerifyResponse
    {
        public required string Status { get; init; }
        public required string Reference { get; init; }
        public required string GatewayResponse { get; init; }
        public string? AuthorizationCode { get; init; }
    }
}
