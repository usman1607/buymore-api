using BuyMoreApi.Application.Dtos.RequestDtos;
using BuyMoreApi.Application.Payments.Paystack;
using BuyMoreApi.Domain.Entities;
using BuyMoreApi.Domain.Enums;

namespace BuyMoreApi.Application.Services.Interfaces
{
    public interface ICustomerPaymentService
    {
        Task<PaystackInitializeResponse> Checkout(CheckoutRequest request, CancellationToken cancellationToken);
        Task<List<Payment>> GetUserPayment(Guid userId);
        Task<Payment?> GetPaymentByReference(string reference);

    }
}
