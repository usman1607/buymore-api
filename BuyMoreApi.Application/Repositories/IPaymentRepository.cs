using BuyMoreApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuyMoreApi.Application.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment> AddPayment(Payment payment);
        Task<Payment?> GetPaymentByReference(string reference);
        Task<List<Payment>> GetUserPayments(Guid userId);
        Task<List<Payment>> GetAllPayments();
        Task UpdatePayment(Payment payment);        
    }
}
