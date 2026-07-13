using BuyMoreApi.Application.Repositories;
using BuyMoreApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuyMoreApi.Infrastructure.Persistence.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;

        public CartRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Cart?> GetAsync(Guid userId)
        {
            var cart = await _context.Carts.Where(c => c.UserId == userId).Include(c => c.Items).FirstOrDefaultAsync();

            return cart;
        }
    }
}
