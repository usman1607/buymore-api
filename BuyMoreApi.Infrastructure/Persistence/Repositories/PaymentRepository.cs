using BuyMoreApi.Application.Repositories;
using BuyMoreApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BuyMoreApi.Infrastructure.Persistence.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;

        public PaymentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Payment> AddPayment(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();

            return payment;
        }

        public async Task<List<Payment>> GetAllPayments()
        {
            var payments = await _context.Payments.Where(p => !p.IsDeleted).ToListAsync();

            return payments;
        }

        public async Task<Payment?> GetPaymentByReference(string reference)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.Reference == reference && !p.IsDeleted);
            return payment;
        }

        public async Task<List<Payment>> GetUserPayments(Guid userId)
        {
            return await _context.Payments.Where(p => p.UserId == userId && !p.IsDeleted).ToListAsync();
        }

        public async Task UpdatePayment(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
        }
    }
}
