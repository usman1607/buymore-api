using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyMoreApi.Application.Authentication;
using BuyMoreApi.Application.Dtos.RequestDtos;
using BuyMoreApi.Application.Dtos.ResponseDtos;
using BuyMoreApi.Application.Exceptions;
using BuyMoreApi.Application.Repositories;
using BuyMoreApi.Application.Services.Interfaces;
using BuyMoreApi.Application.Storage;
using BuyMoreApi.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BuyMoreApi.Application.Services.Implementations
{
    public class ItemService : IItemService
    {
        private readonly IUserRepository _userRepo;
        private readonly ICartRepository _cartRepo;
        private readonly IItemRepository _itemRepo;
        private readonly ICurrentUser _currentUser;
        private readonly ILogger<ItemService> _logger;
        private readonly IFileStorage _fileStorage;

        public ItemService(ICurrentUser currentUser,IUserRepository userRepo, ICartRepository cartRepo, IItemRepository itemRepo, IFileStorage fileStorage, ILogger<ItemService> logger)
        {
            _logger = logger;
            _userRepo = userRepo;
            _itemRepo = itemRepo;
            _cartRepo = cartRepo;
            _currentUser = currentUser;
            _fileStorage = fileStorage;
        }

        public async Task<Item> AddAsync(ItemRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Adding a new Item {request.Name}.");
            if(request.CostPrice < 0 || request.SellingPrice < 0)
            {
                _logger.LogWarning("Cost price or selling price cannot be less than zero.");
                throw new BadRequestException("Cost price or selling price cannot be less than zero.");
            }

            var loggedInUser = _currentUser.LoggedInUserEmail();
            var item = new Item
            {
                Name = request.Name,
                Description = request.Description,
                Category = request.Category,
                SellingPrice = request.SellingPrice,
                CostPrice = request.CostPrice,
                CreatedBy = loggedInUser,
                Quantity = request.Quantity
            };

            if (request.Images != null)
            {
                foreach(var file in request.Images)
                {
                    if(file.Length > 0)
                    {
                        await using var stream = file.OpenReadStream();
                        item.ImageUrls.Add(await _fileStorage.SaveAsync(new FileUploadRequest
                        {
                            Content = stream,
                            FileName = file.FileName,
                            Folder = "Itmes",
                            ContentType = file.ContentType
                        }, cancellationToken));

                        if (file.Length > 1000 * 1024 * 1024)
                        {
                            _logger.LogWarning($"File {file.FileName} exceeds the maximum allowed size of 1GB.");
                            throw new BadRequestException($"File {file.FileName} exceeds the maximum allowed size of 5MB.");
                        }
                    }
                    
                    }                    
                }
            }

            await _itemRepo.AddAsync(item);
            return item;
        }

        public async Task<CartResponse> AddItemToCart(Guid itemId, int quantity)
        {
            var loggedInUser = _currentUser.LoggedInUserEmail();
            var user = await _userRepo.GetUserByEmail(loggedInUser);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            var item = await _itemRepo.GetByIdAsync(itemId);

            if(item == null)
            {
                throw new NotFoundException("Item not found.");
            }

            if(quantity > item.Quantity)
            {
                throw new BadRequestException($"No enough item, available quantity: {item.Quantity}.");
            }
            
            var cart = await _cartRepo.GetByUserIdAsync(user.Id);
            if(cart == null)
            {
                cart = await _cartRepo.AddAsync(new Cart
                {
                    UserId = user.Id,
                    User = user,
                    CreatedBy = loggedInUser
                });
            }

            if (cart.Items.ContainsKey(item.Id))
            {
                cart.Items[item.Id] += quantity;
            }
            else
            {
                cart.Items.Add(item.Id, quantity);
            }

            await _cartRepo.Update(cart);
            return new CartResponse(cart.Id, cart.Items);
        }

        public async Task<Item> AddMoreQuantity(Guid id, int quantity)
        {
            var item = await _itemRepo.GetByIdAsync(id);
            if(quantity <= 0)
            {
                throw new BadRequestException("Invalid quantity.");
            }

            if(item == null)
            {
                _logger.LogWarning($"Item with id: {id} not found.");
                throw new NotFoundException($"Item with id: {id} not found.");
            }

            item.UpdateQuantity(quantity, true);
            await _itemRepo.Update(item);
            return item;
        }

        public async Task<Item?> AdminGet(Guid id)
        {
            return await _itemRepo.GetByIdAsync(id);
        }

        public async Task<List<Item>> AdminGetAll(SearchItemRequest request)
        {
            return await _itemRepo.GetAllAsync(request);
        }

        public async Task<ItemResponse?> GetByIdAsync(Guid id)
        {
            var item = await _itemRepo.GetByIdAsync(id);
            return item == null ? null : new ItemResponse
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Category = item.Category,
                Quantity = item.Quantity,
                SellingPrice = item.SellingPrice,
                ImageUrls = item.ImageUrls
            };
        }

        public async Task<CartResponse> GetCart()
        {
            var userId = _currentUser.LoggedInUserId();
            var cart = await _cartRepo.GetByUserIdAsync(userId);
            if(cart == null)
            {
                throw new NotFoundException("Cart not found.");
            }
                
            return new CartResponse(cart.Id, cart.Items);
        }

        public async Task<CartResponse> RemoveItemFromCart(Guid cartId, Guid itemId, int quantity)
        {
            var loggedInUser = _currentUser.LoggedInUserEmail();
            var cart = await _cartRepo.GetByIdAsync(cartId);
            if(cart == null)
            {
                throw new NotFoundException("Cart not found");
            }         
            
            if (cart.Items.TryGetValue(itemId, out int value))
            {

                if(quantity >= value)
                {
                    cart.Items.Remove(itemId);
                }
                else
                {
                    cart.Items[itemId] -= quantity;
                }

                await _cartRepo.Update(cart);
                return new CartResponse(cart.Id, cart.Items);
            }

            throw new NotFoundException($"There is no selected item in your cart.");
        }

        public async Task<List<ItemResponse>> SearchItems(SearchItemRequest request)
        {
            var items = await _itemRepo.GetAllAsync(request);
            return items.Select(item => new ItemResponse
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Category = item.Category,
                Quantity = item.Quantity,
                SellingPrice = item.SellingPrice,
                ImageUrls = item.ImageUrls
            }).ToList();
        }

        public async Task<Item> Update(Guid id, ItemRequest request)
        {
            var item = await _itemRepo.GetByIdAsync(id);
            if(item == null)
            {
                _logger.LogWarning($"Item with id: {id} not found.");
                throw new NotFoundException($"Item with id: {id} not found.");
            }
            item.Name = request.Name;
            item.Category = request.Category;
            item.Description = request.Description;
            item.SellingPrice = request.SellingPrice;
            item.CostPrice = request.CostPrice;
            item.Quantity = request.Quantity;

            await _itemRepo.Update(item);
            return item;
        }
    }
}