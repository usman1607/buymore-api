using BuyMoreApi.Domain.Enums;

namespace BuyMoreApi.Domain.Entities
{
    public class User: BaseEntity
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string? PhoneNumber { get; set; }
        public string Email { get; set; } = default!;
        public string EncryptedPassword { get; set; } = default!;
        public string? Address { get; set; }
        public Role Role { get; set; } = default!;
        public decimal WalletBalance { get; private set; }
        public List<Order> Orders { get; set; } = new List<Order>();
        public Guid? CartId { get; set; }
        public Cart? Cart { get; set; }

        public void UpdateWalletBalance(decimal amount)
        {
            WalletBalance += amount;
        }
    }
}