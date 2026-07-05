using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyMoreApi.Application.Dtos.RequestDtos;
using BuyMoreApi.Application.Dtos.ResponseDtos;

namespace BuyMoreApi.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<LoginResponse> Login(LoginRequest request);
        Task<LoginResponse> Register(RegisterRequest request);
    }
}