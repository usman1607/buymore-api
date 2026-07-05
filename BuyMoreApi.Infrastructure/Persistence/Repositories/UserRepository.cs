using BuyMoreApi.Application.Repositories;
using BuyMoreApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BuyMoreApi.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id && !u.IsDeleted);
            if (user == null)
            {
                return false;
            }

            user.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<User>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            var user = await _context.Users.Where(u => u.Email == email && !u.IsDeleted).Include(u => u.Orders).FirstOrDefaultAsync();
            return user;
        }

        public async Task<User?> GetUserById(Guid id)
        {
            var user = await _context.Users.Where(u => u.Id == id && !u.IsDeleted).Include(u => u.Orders).FirstOrDefaultAsync();
            return user;
        }

        public async Task<bool> UpdateUser(Guid id, User user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateWalletBalance(Guid id, decimal newBalance)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
    }
}