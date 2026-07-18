using BuyMoreApi.Application.Dtos.RequestDtos;
using BuyMoreApi.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuyMoreApi.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] ItemRequest request, CancellationToken cancellationToken)
        {
            var response = await _itemService.AddAsync(request, cancellationToken);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromRoute] SearchItemRequest request)
        {
            var response = await _itemService.AdminGetAll(request);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("search-items")]
        public async Task<IActionResult> SearchItems([FromRoute] SearchItemRequest request)
        {
            var response = await _itemService.SearchItems(request);
            return Ok(response);
        }
    }
}