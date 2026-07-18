using Microsoft.AspNetCore.Http;

namespace BuyMoreApi.Application.Dtos.RequestDtos
{
    public class ItemRequest
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string Category { get; set; } = "General";
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public int Quantity { get; set; }
        public List<IFormFile> Images { get; set; } = new();
    }
}