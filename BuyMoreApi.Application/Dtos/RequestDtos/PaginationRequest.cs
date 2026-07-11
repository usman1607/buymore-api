using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyMoreApi.Application.Dtos.RequestDtos
{
    public class PaginationRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public SortBy? SortBy { get; set; }
        public bool SortDescending { get; set; } = false;
        public string? SearchTerm { get; set; }
    }

    public enum SortBy
    {
        FirstName,
        LastName,
        Email,
        Role
    }
}