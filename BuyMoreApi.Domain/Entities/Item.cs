using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyMoreApi.Domain.Entities
{
    public class Item: BaseEntity
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; } = "General";
        public List<string> ImageUrls { get; set; } = new();

        public bool UpdateQuantity(int quantity, bool add)
        {
            if (add)
            {
                Quantity += quantity;
                return true;
            }

            if(Quantity >= quantity)
            {
                Quantity -= quantity;
                return true;
            }
            return false;
        }
    }
}