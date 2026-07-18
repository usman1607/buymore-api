using BuyMoreApi.Application.Dtos.RequestDtos;
using BuyMoreApi.Application.Repositories;
using BuyMoreApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BuyMoreApi.Infrastructure.Persistence.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly AppDbContext _context;

        public ItemRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Item item)
        {
            await _context.Items.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Item>> GetAllAsync(SearchItemRequest request)
        {
            var query = _context.Items.Where(i => !i.IsDeleted).AsQueryable();

            if(request.PriceRange.MinPrice > 0 || request.PriceRange.MaxPrice > 0)
            {

                query = query.Where(i => i.SellingPrice >= request.PriceRange.MinPrice
                    && i.SellingPrice <= request.PriceRange.MaxPrice);

                query = request.SortDescending ? query.OrderByDescending(i => i.SellingPrice)
                    : query.OrderBy(i => i.SellingPrice);

            }

            if (!string.IsNullOrEmpty(request.Category))
            {
                query = query.Where(i => i.Category.Equals(request.Category, StringComparison.InvariantCultureIgnoreCase));
            }

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                var searchItem = request.SearchTerm;
                query = query.Where(i => i.Name.Contains(searchItem)
                 || i.Category.Contains(searchItem) 
                 || (!string.IsNullOrEmpty(i.Description) && i.Description.Contains(searchItem)));
            }

            return await query.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
        }

        public async Task<Item?> GetByIdAsync(Guid id)
        {
            return await _context.Items.Where(i => i.Id == id && !i.IsDeleted).FirstOrDefaultAsync();
        }

        public async Task<Item> Update(Item item)
        {
            _context.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }
    }
}