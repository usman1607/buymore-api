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

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> CreateItem(ItemRequest request, CancellationToken cancellationToken)
        {
            var response = await _itemService.AddAsync(request, cancellationToken);
            return Ok(response);
        }

        [Authorize(Policy = "AdminOnly")]
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

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("admin/{id:guid}")]
        public async Task<IActionResult> GetItem(Guid id)
        {
            var response = await _itemService.AdminGet(id);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetItemDetails(Guid id)
        {
            var response = await _itemService.GetByIdAsync(id);
            return Ok(response);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPatch("add-quantity/{id:guid}")]
        public async Task<IActionResult> AddQuantity(Guid id, int quantity)
        {
            var response = await _itemService.AddMoreQuantity(id, quantity);
            return Ok(response);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPatch("update/{id:guid}")]
        public async Task<IActionResult> UpdateItem(Guid id, ItemRequest request)
        {
            var response = await _itemService.Update(id, request);
            return Ok(response);
        }

        [Authorize(Policy = "CustomerOnly")]
        [HttpPatch("add-to-cart")]
        public async Task<IActionResult> AddItemToCart(Guid itemId, int quantity)
        {
            var response = await _itemService.AddItemToCart(itemId, quantity);
            return Ok(response);
        }

        [Authorize(Policy = "CustomerOnly")]
        [HttpPatch("remove-from-cart")]
        public async Task<IActionResult> RemoveItemFromCart(Guid id, Guid itemId, int quantity)
        {
            var response = await _itemService.RemoveItemFromCart(id, itemId, quantity);
            return Ok(response);
        }

        [Authorize(Policy = "CustomerOnly")]
        [HttpPatch("get-cart")]
        public async Task<IActionResult> GetCart()
        {
            var response = await _itemService.GetCart();
            return Ok(response);
        }
    }
}