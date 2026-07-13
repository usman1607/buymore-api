using BuyMoreApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuyMoreApi.Application.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> AddOrder(Order order);
    }
}
