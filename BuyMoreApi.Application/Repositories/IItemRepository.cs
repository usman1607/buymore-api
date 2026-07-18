using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyMoreApi.Application.Dtos.RequestDtos;
using BuyMoreApi.Domain.Entities;

namespace BuyMoreApi.Application.Repositories
{
    public interface IItemRepository
    {
        Task AddAsync(Item item);
        Task<Item?> GetByIdAsync(Guid id);
        Task<List<Item>> GetAllAsync(SearchItemRequest request);
        Task<Item> Update(Item item);
    }
}