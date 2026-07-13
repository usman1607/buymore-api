using System.Threading;
using System.Threading.Tasks;

namespace BuyMoreApi.Application.Payments.Paystack
{
    /// <summary>
    /// Configuration required to talk to Paystack's REST API.
    /// </summary>
    public sealed class PaystackOptions
    {
        public string SecretKey { get; set; } = string.Empty;
        public string PublicKey { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = "https://api.paystack.co";
    }
}
