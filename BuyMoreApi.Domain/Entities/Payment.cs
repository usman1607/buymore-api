using BuyMoreApi.Domain.Enums;

namespace BuyMoreApi.Domain.Entities
{
    public class Payment: BaseEntity
    {
        public string Reference { get; set; } = default!;
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = default!;
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    }
}