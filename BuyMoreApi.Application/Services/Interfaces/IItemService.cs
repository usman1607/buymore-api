using BuyMoreApi.Application.Dtos.RequestDtos;
using BuyMoreApi.Application.Dtos.ResponseDtos;
using BuyMoreApi.Domain.Entities;

namespace BuyMoreApi.Application.Services.Interfaces
{
    public interface IItemService
    {
        Task<Item> AddAsync(ItemRequest request, CancellationToken cancellationToken);
        Task<ItemResponse?> GetByIdAsync(Guid id);
        Task<List<ItemResponse>> SearchItems(SearchItemRequest request);
        Task<Item> AddMoreQuantity(Guid id, int quantity);
        Task<Item?> AdminGet(Guid id);
        Task<List<Item>> AdminGetAll(SearchItemRequest request);
        Task<Item> Update(Guid id, ItemRequest request);
        Task<CartResponse> AddItemToCart(Guid itemId, int quantity);     
        Task<CartResponse> RemoveItemFromCart(Guid cartId, Guid itemId, int quantity);
        Task<CartResponse> GetCart();
    }
}