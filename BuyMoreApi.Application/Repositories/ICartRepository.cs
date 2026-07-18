using BuyMoreApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuyMoreApi.Application.Repositories
{
    public interface ICartRepository
    {
        Task<Cart?> GetByIdAsync(Guid id);
        Task<Cart?> GetByUserIdAsync(Guid userId);
        Task<Cart> AddAsync(Cart cart);
        Task<Cart> Update(Cart cart);
    }
}
