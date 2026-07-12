using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyMoreApi.Application.Dtos.RequestDtos;
using BuyMoreApi.Application.Dtos.ResponseDtos;
using BuyMoreApi.Domain.Entities;

namespace BuyMoreApi.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<LoginResponse> Login(LoginRequest request);
        Task<LoginResponse> Register(RegisterRequest request);
        Task<UserDto> AddUser(NewUserRequest request);
        Task<UserDto> GetProfile(Guid id);
        Task<List<UserDto>> GetAllUsers(SearchUserRequest request);
        Task<UserDto> GetUserByEmail(string email);
        Task<bool> UpdateProfile(Guid id, UpdateUserRequest request);
    }
}