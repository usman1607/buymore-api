using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyMoreApi.Application.Dtos.ResponseDtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Role { get; set; } = default!;
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }
}