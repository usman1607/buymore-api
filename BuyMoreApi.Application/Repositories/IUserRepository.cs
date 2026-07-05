using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyMoreApi.Domain.Entities;

namespace BuyMoreApi.Application.Repositories
{
    public interface IUserRepository
    {
        Task AddUser(User user);
        Task<User?> GetUserByEmail(string email);
        Task<User?> GetUserById(Guid id);
        Task<List<User>> GetAllUsers();
        Task<bool> UpdateUser(Guid id, User user);
        Task<bool> DeleteUser(Guid id);
        Task<bool> UpdateWalletBalance(Guid id, decimal newBalance);
        Task<bool> EmailExists(string email);
    }
}