using BuyMoreApi.Application.Dtos.RequestDtos;
using BuyMoreApi.Application.Exceptions;
using BuyMoreApi.Application.Helpers;
using BuyMoreApi.Application.Payments;
using BuyMoreApi.Application.Payments.Paystack;
using BuyMoreApi.Application.Repositories;
using BuyMoreApi.Application.Services.Interfaces;
using BuyMoreApi.Domain.Entities;
using BuyMoreApi.Domain.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuyMoreApi.Application.Services.Implementations
{
    public class CustomerPaymentService : ICustomerPaymentService
    {
        private readonly ILogger<CustomerPaymentService> _logger;
        private readonly IUserRepository _userRepository;
        private readonly ICartRepository _cartRepositoty;
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPaymentService _paymentService;

        public CustomerPaymentService(IUserRepository userRepository, ICartRepository cartRepository, IOrderRepository orderRepository, IPaymentRepository paymentRepository, IPaymentService paymentService, ILogger<CustomerPaymentService> logger)
        {
            _logger = logger;
            _cartRepositoty = cartRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _paymentService = paymentService;
            _paymentRepository = paymentRepository;
        }

        public async Task<PaystackInitializeResponse> Checkout(CheckoutRequest request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserById(request.UserId);
            if(user == null)
            {
                _logger.LogWarning($"Customer with id: {request.UserId} does not exist.");
                throw new BadRequestException($"Customer with id: {request.UserId} does not exist.");
            }

            var orderReference = Util.GenerateReference("ORD");
            var paymentReference = Util.GenerateReference("PAY");
            var cart = await _cartRepositoty.GetAsync(request.UserId);

            if(cart.Items.Count <= 0)
            {
                _logger.LogWarning("There is not item in the cart.");
                throw new BadRequestException("There is not item in the cart.");
            }

            var amount = cart.Items.Sum(i => i.SellingPrice * i.Quantity);
            var metadata = new Dictionary<string, object>();
            foreach( var item in cart.Items)
            {
                if (!metadata.ContainsKey(item.Name))
                {
                    var value = $"{item.Quantity} x {item.SellingPrice} = {item.Quantity * item.SellingPrice}";
                    metadata.Add(item.Name, value);
                }
            }

            var order = new Order
            {
                UserId = request.UserId,
                User = user,
                Items = cart.Items,
                Reference = orderReference,
                Status = OrderStatus.Pending,
                TotalAmount = amount,
                CreatedBy = user.Email
            };


            var payment = new Payment
            {
                Reference = paymentReference,
                UserId = request.UserId,
                User = user,
                Amount = amount,
                OrderId = order.Id,
                Order = order,
                Status = PaymentStatus.Pending,
                Method = request.PaymentMethod,
                CreatedBy = user.Email
            };

            order.Payment = payment;
            order.PaymentId = payment.Id;

            await _orderRepository.AddOrder(order);
            await _paymentRepository.AddPayment(payment);

            var paymentRequest = new PaystackInitializeRequest
            {
                Amount = amount * 100,
                Email = user.Email,
                Reference = paymentReference,
                CallbackUrl = request.CallbackUrl,
                Metadata = metadata
            };

            return await _paymentService.InitializeTransactionAsync(paymentRequest, cancellationToken);
        }

        public Task<Payment?> GetPaymentByReference(string reference)
        {
            throw new NotImplementedException();
        }

        public Task<List<Payment>> GetUserPayment(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
