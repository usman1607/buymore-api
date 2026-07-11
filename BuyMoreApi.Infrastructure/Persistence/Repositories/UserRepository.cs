using BuyMoreApi.Application.Dtos.RequestDtos;
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
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
            if (existingUser == null)
            {
                return false;
            }

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.Address = user.Address;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateWalletBalance(Guid id, decimal amount)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
            if (existingUser == null)
            {
                return false;
            }

            existingUser.UpdateWalletBalance(amount);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<List<User>> SearchUsers(SearchUserRequest request)
        {
            var query = _context.Users.Where(u => !u.IsDeleted).AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = query.Where(u => u.FirstName.Contains(request.SearchTerm) 
                || u.LastName.Contains(request.SearchTerm) 
                || u.Email.Contains(request.SearchTerm));
            }

            if (request.Role.HasValue)
            {
                query = query.Where(u => u.Role == request.Role.Value);
            }

            if (request.SortBy.HasValue)
            {
                if (request.SortDescending)
                {
                    switch (request.SortBy.Value)
                    {
                        case SortBy.FirstName:
                            query = query.OrderByDescending(u => u.FirstName);
                            break;
                        case SortBy.LastName:
                            query = query.OrderByDescending(u => u.LastName);
                            break;
                        case SortBy.Email:
                            query = query.OrderByDescending(u => u.Email);
                            break;
                        case SortBy.Role:
                            query = query.OrderByDescending(u => u.Role);
                            break;
                    }
                }
                else
                {
                    switch (request.SortBy.Value)
                    {
                        case SortBy.FirstName:
                            query = query.OrderBy(u => u.FirstName);
                            break;
                        case SortBy.LastName:
                            query = query.OrderBy(u => u.LastName);
                            break;
                        case SortBy.Email:
                            query = query.OrderBy(u => u.Email);
                            break;
                        case SortBy.Role:
                            query = query.OrderBy(u => u.Role);
                            break;
                    }
                }
            }

            var users = await query.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
            return users;
        }
    }
}