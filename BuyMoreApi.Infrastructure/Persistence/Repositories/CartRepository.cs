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

        public async Task<Cart> AddAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<Cart?> GetAsync(Guid userId)
        {
            var cart = await _context.Carts.Where(c => c.UserId == userId).FirstOrDefaultAsync();

            return cart;
        }

        public async Task<Cart?> GetByIdAsync(Guid id)
        {
            return await _context.Carts.Where(c => c.Id == id && !c.IsDeleted).FirstOrDefaultAsync();
        }

        public async Task<Cart> Update(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
            return cart;
        }
    }
}
