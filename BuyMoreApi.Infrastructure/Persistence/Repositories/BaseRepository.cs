using BuyMoreApi.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuyMoreApi.Infrastructure.Persistence.Repositories
{
    public class BaseRepository : IBaseRepository
    {
        private readonly AppDbContext _context;
        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
