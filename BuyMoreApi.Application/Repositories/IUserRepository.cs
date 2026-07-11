using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyMoreApi.Application.Dtos.RequestDtos;
using BuyMoreApi.Domain.Entities;

namespace BuyMoreApi.Application.Repositories
{
    public interface IUserRepository
    {
        Task AddUser(User user);
        Task<User?> GetUserByEmail(string email);
        Task<User?> GetUserById(Guid id);
        Task<List<User>> GetAllUsers();
        Task<List<User>> SearchUsers(SearchUserRequest request);
        Task<bool> UpdateUser(Guid id, User user);
        Task<bool> DeleteUser(Guid id);
        Task<bool> UpdateWalletBalance(Guid id, decimal amount);
        Task<bool> EmailExists(string email);
    }
}