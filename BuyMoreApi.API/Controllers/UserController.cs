using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyMoreApi.Application.Dtos.RequestDtos;
using BuyMoreApi.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuyMoreApi.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var response = await _userService.Register(request);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _userService.Login(request);
            return Ok(response);
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile([FromQuery] Guid id)
        {
            var response = await _userService.GetProfile(id);
            return Ok(response);
        }
    }
}