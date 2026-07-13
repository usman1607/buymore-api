using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyMoreApi.Domain.Enums;

namespace BuyMoreApi.Domain.Entities
{
    public class Order: BaseEntity
    {
        public string Reference { get; set; } = default!;
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;
        public Guid PaymentId { get; set; }
        public Payment Payment { get; set; } = default!;
        public List<Item> Items { get; set; } = new List<Item>();
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
    }
}