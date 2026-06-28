using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyMoreApi.Domain.Enums;

namespace BuyMoreApi.Domain.Entities
{
    public class Order: BaseEntity
    {
        public string Reference { get; } = default!;
        public Guid UserId { get; }
        public User User { get; } = default!;
        public Guid PaymentId { get; }
        public Payment Payment { get; } = default!;
        public List<Item> Items { get; } = new List<Item>();
        public decimal TotalAmount { get; }
        public OrderStatus Status { get; set; }
    }
}