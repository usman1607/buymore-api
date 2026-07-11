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

        public async Task<List<UserDto>> GetAllUsers(SearchUserRequest request)
        {
            var users = await _userRepository.SearchUsers(request);
            
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Role = u.Role.ToString(),
                PhoneNumber = u.PhoneNumber,
                Address = u.Address
            }).ToList();
        }

        public async Task<UserDto> GetProfile(Guid id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                throw new NotFoundException($"User with ID: {id} not found.");
            }

            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role.ToString(),
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            };
        }

        public async Task<UserDto> GetUserByEmail(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null)
            {
                throw new NotFoundException($"User with email: {email} not found.");
            }

            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role.ToString(),
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            };
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            _logger.LogInformation("Attempting to log in user with email: {Email}", request.Email);
            var user = await _userRepository.GetUserByEmail(request.Email);
            if (user == null || !Util.IsValidPassword(request.Password, user.EncryptedPassword))
            {
                _logger.LogWarning("Invalid login attempt for email: {Email}", request.Email);
                throw new UnauthorizedException("Invalid email or password.");
            }

            var token = _jwtService.GenerateToken(user);

            return new LoginResponse
            {
                Token = token,
                Id = user.Id,
                Email = user.Email,
                FullName = $"{user.FirstName} {user.LastName}",
                Role = user.Role.ToString(),
            };
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

        public async Task<bool> UpdateProfile(Guid id, UpdateUserRequest request)
        {
            _logger.LogInformation("Updating profile for user with ID: {UserId}", id);
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found.", id);
                throw new NotFoundException($"User with ID: {id} not found.");
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;
            user.Address = request.Address;
            
            return await _userRepository.UpdateUser(id, user);
        }
    }
}