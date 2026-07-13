using System.Threading;
using System.Threading.Tasks;
using BuyMoreApi.Application.Payments.Paystack;

namespace BuyMoreApi.Application.Payments
{
    /// <summary>
    /// High-level payment facade so the rest of the app does not depend on Paystack specifics.
    /// </summary>
    public interface IPaymentService
    {
        Task<PaystackInitializeResponse> InitializeTransactionAsync(PaystackInitializeRequest request, CancellationToken cancellationToken = default);

        Task<PaystackVerifyResponse> VerifyTransactionAsync(string reference, CancellationToken cancellationToken = default);
    }
}
