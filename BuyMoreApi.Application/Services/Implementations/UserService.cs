using BuyMoreApi.Application.Authentication;
using BuyMoreApi.Application.Dtos.RequestDtos;
using BuyMoreApi.Application.Dtos.ResponseDtos;
using BuyMoreApi.Application.Exceptions;
using BuyMoreApi.Application.Repositories;
using BuyMoreApi.Application.Services.Interfaces;
using BuyMoreApi.Application.Utilities;
using BuyMoreApi.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BuyMoreApi.Application.Services.Implementations
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IJwtService _jwtService;
        public UserService(IUserRepository userRepository, IJwtService jwtService, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
            _jwtService = jwtService;
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<LoginResponse> Register(RegisterRequest request)
        {
            _logger.LogInformation("Registering user with email: {Email}", request.Email);
            var alreadyExists = await _userRepository.EmailExists(request.Email);
            if (alreadyExists)
            {
                _logger.LogWarning("User with email {Email} already exists.", request.Email);
                throw new BadRequestException($"User with email: {request.Email} already exists.");
            }

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                EncryptedPassword = Util.EncryptPassword(request.Password),
                Role = Domain.Enums.Role.Customer,
                CreatedBy = request.Email
            };

            await _userRepository.AddUser(newUser);
            _logger.LogInformation("User registered successfully with email: {Email}", request.Email);

            var token = _jwtService.GenerateToken(newUser);

            return new LoginResponse
            {
                Token = token,
                Id = newUser.Id,
                Email = newUser.Email,
                FullName = $"{newUser.FirstName} {newUser.LastName}",
                Role = newUser.Role.ToString(),
            };
        }
    }
}