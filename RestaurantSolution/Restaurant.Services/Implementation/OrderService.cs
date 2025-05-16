using System;
using System.Collections.Generic;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Enums;
using Restaurant.Services.Interfaces;
using Restaurant.DataAccess.Interfaces;

namespace Restaurant.Services.Implementation
{
    internal class OrderService:IOrderService
    {
        private readonly IOrderRepository _repo;

        public OrderService(IOrderRepository repo)
        => _repo = repo;
        

        public IEnumerable<Order> GetAllOrders()
        {
            return _repo.GetAll();
        }

        public IEnumerable<Order> GetOrdersByUser(int userId)
        {
                if (userId <= 0) throw new ArgumentException("User ID invalid", nameof(userId));
               return _repo.GetByUser(userId);
            
        }


        public IEnumerable<Order> GetActiveOrders()
            => _repo.GetActive();

        public Order GetOrderDetails(int orderId)
        {
            if (orderId <= 0) throw new ArgumentException("Order ID invalid", nameof(orderId));
            return _repo.GetDetails(orderId);
        }

        public void CreateOrder(
            string orderCode,
            int userId,
            DateTime orderDateTime,
            OrderStatus status,
            DateTime? estimatedDelivery,
            decimal discount,
            decimal DeliveryCost,
            decimal totalCost)
        {
            if (string.IsNullOrWhiteSpace(orderCode)) throw new ArgumentException("Cod comandă obligatoriu", nameof(orderCode));
            if (userId <= 0) throw new ArgumentException("User ID invalid", nameof(userId));
            if (orderDateTime == default) throw new ArgumentException("Data comandă invalidă", nameof(orderDateTime));
            _repo.Insert(orderCode, userId, orderDateTime, status, estimatedDelivery, discount, DeliveryCost, totalCost);
        }

        public void UpdateOrderStatus(int orderId, OrderStatus status)
        {
            if (orderId <= 0) throw new ArgumentException("Order ID invalid", nameof(orderId));
            _repo.UpdateStatus(orderId, status);
        }

        public void DeleteOrder(int orderId)
        {
            if (orderId <= 0) throw new ArgumentException("Order ID invalid", nameof(orderId));
            _repo.Delete(orderId);
        }
    }
}
