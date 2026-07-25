using BuyMoreApi.Application.Authentication;
using BuyMoreApi.Application.Dtos.RequestDtos;
using BuyMoreApi.Application.Exceptions;
using BuyMoreApi.Application.Repositories;
using BuyMoreApi.Application.Services.Implementations;
using BuyMoreApi.Application.Services.Interfaces;
using BuyMoreApi.Application.Storage;
using BuyMoreApi.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.ServiceTests
{
    public class ItemServiceTests
    {
        private readonly ItemService _itemService;
        private readonly Mock<IUserRepository> _userRepo;
        private readonly Mock<ICartRepository> _cartRepo;
        private readonly Mock<IItemRepository> _itemRepo;
        private readonly Mock<ICurrentUser> _currentUser;
        private readonly Mock<ILogger<ItemService>> _logger;
        private readonly Mock<IFileStorage> _fileStorage;

        public ItemServiceTests()
        {
            
            _userRepo = new Mock<IUserRepository>();
            _cartRepo = new Mock<ICartRepository>();
            _itemRepo = new Mock<IItemRepository>();
            _currentUser = new Mock<ICurrentUser>();
            _logger = new Mock<ILogger<ItemService>>();
            _fileStorage = new Mock<IFileStorage>();
            _itemService = new ItemService(_currentUser.Object, _userRepo.Object, _cartRepo.Object, _itemRepo.Object, _fileStorage.Object, _logger.Object);
        }   

        [Fact]
        public async Task AddItem_Should_Throw_Bad_Request_When_Price_IsLessThanZero()
        {
            //Arrange
            var request = new ItemRequest
            {
                Name = "test item",
                Category = "Test",
                CostPrice = -1,
                SellingPrice = 50,
                Description = "Test",
                Quantity = 10,
            };

            Item? item = null;

            _currentUser.Setup(c => c.LoggedInUserEmail()).Returns("sample@email.com");
            _itemRepo.Setup(i => i.AddAsync(item));

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() =>
                _itemService.AddAsync(request, It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task AddNewItem_Should_Return_Success_When_Request_IsValid()
        {
            //Arrange
            var request = new ItemRequest
            {
                Name = "test item",
                Category = "Test",
                CostPrice = 45,
                SellingPrice = 50,
                Description = "Test",
                Quantity = 10
            };

            var expected = new Item
            {
                Name = "test item",
                Category = "Test",
                CostPrice = 45,
                SellingPrice = 50,
                Description = "Test",
                Quantity = 10
            };

            _currentUser.Setup(c => c.LoggedInUserEmail()).Returns("sample@email.com");
            _itemRepo.Setup(i => i.AddAsync(expected));

            //Act
            var result = await _itemService.AddAsync(request, It.IsAny<CancellationToken>());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.Name, result.Name);
            Assert.Equal(expected.Category, result.Category);
            _itemRepo.Verify(r => r.AddAsync(It.IsAny<Item>()), Times.Once);

        }

        [Fact]
        public async Task AddItemToCart_Should_Be_Successful_When_Request_IsValid()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var user = new User() { Id = userId};

            var item = new Item
            {
                Name = "test item",
                Category = "Test",
                CostPrice = 45,
                SellingPrice = 50,
                Description = "Test",
                Quantity = 10
            };

            var cartItem = new Dictionary<Guid, int>();
            cartItem.Add(item.Id, 2);
            
            var cart = new Cart
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Items = cartItem
            };

            _currentUser.Setup(u => u.LoggedInUserEmail()).Returns("example@mail.com");
            _userRepo.Setup(u => u.GetUserByEmail("example@mail.com")).ReturnsAsync(user);
            _cartRepo.Setup(c => c.GetByUserIdAsync(userId)).ReturnsAsync(cart);
            _itemRepo.Setup(i => i.GetByIdAsync(item.Id)).ReturnsAsync(item);
            _cartRepo.Setup(c => c.Update(cart)).ReturnsAsync(cart);

            //Act

            var result = await _itemService.AddItemToCart(item.Id, 2);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.itesm[item.Id]);
            Assert.Equal(cart.Id, result.id);
            _cartRepo.Verify(i => i.Update(cart), Times.Once);
        }

        [Fact]
        public async Task AddItemToCart_Should_Return_BadRequest_When_There_IsNoEnough_Item()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var user = new User() { Id = userId };

            var item = new Item
            {
                Name = "test item",
                Category = "Test",
                CostPrice = 45,
                SellingPrice = 50,
                Description = "Test",
                Quantity = 2
            };
            var cartItem = new Dictionary<Guid, int>();
            cartItem.Add(item.Id, 2);

            var cart = new Cart
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Items = cartItem
            };

            _currentUser.Setup(u => u.LoggedInUserEmail()).Returns("example@mail.com");
            _userRepo.Setup(u => u.GetUserByEmail("example@mail.com")).ReturnsAsync(user);
            _cartRepo.Setup(c => c.GetByUserIdAsync(userId)).ReturnsAsync(cart);
            _itemRepo.Setup(i => i.GetByIdAsync(item.Id)).ReturnsAsync(item);
            _cartRepo.Setup(c => c.Update(cart)).ReturnsAsync(cart);

            //Act and Assert
            await Assert.ThrowsAsync<BadRequestException>(() =>
                _itemService.AddItemToCart(item.Id, 5));
        }

        [Fact]
        public async Task AddItemToCart_Should_Return_NotFound_When_Item_NotExists()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            var user = new User() { Id = userId };

            var cart = new Cart
            {
                Id = Guid.NewGuid(),
                UserId = userId,
            };

            _currentUser.Setup(u => u.LoggedInUserEmail()).Returns("example@mail.com");
            _userRepo.Setup(u => u.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(user);
            _cartRepo.Setup(c => c.GetByUserIdAsync(userId)).ReturnsAsync(cart);
            _itemRepo.Setup(i => i.GetByIdAsync(itemId)).ReturnsAsync((Item?)null);
            _cartRepo.Setup(c => c.Update(cart)).ReturnsAsync(cart);

            //Act and Assert
            await Assert.ThrowsAsync<NotFoundException>(() =>
                _itemService.AddItemToCart(itemId, 5));
        }


    }
}
