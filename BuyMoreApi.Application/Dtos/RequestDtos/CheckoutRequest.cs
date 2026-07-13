using BuyMoreApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuyMoreApi.Application.Dtos.RequestDtos
{
    public class CheckoutRequest
    {
        public Guid UserId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string CallbackUrl { get; set; } = default!;
    }
}
