namespace BuyMoreApi.Application.Payments.Paystack
{
    /// <summary>
    /// Key bits the frontend needs after a Paystack initialization call.
    /// </summary>
    public sealed class PaystackInitializeResponse
    {
        public required string AuthorizationUrl { get; init; }
        public required string AccessCode { get; init; }
        public required string Reference { get; init; }
    }
}
