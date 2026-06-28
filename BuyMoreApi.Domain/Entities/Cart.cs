using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyMoreApi.Domain.Entities
{
    public class Cart: BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;
        public List<Item> Items { get; set; } = new();
    }
}