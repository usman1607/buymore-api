using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyMoreApi.Application.Dtos.ResponseDtos
{
    public class LoginResponse
    {
        public string Token { get; set; } = default!;
        public Guid Id { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Role { get; set; } = default!;
        public string FullName { get; set; } = default!;
    }
}