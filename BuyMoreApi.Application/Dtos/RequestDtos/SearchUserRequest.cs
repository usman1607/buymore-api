using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyMoreApi.Domain.Enums;

namespace BuyMoreApi.Application.Dtos.RequestDtos
{
    public class SearchUserRequest: PaginationRequest
    {
        public Role? Role { get; set; }
    }
    
}