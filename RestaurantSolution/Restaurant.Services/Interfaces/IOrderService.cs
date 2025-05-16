using System;
using System.Collections.Generic;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Enums;

namespace Restaurant.Services.Interfaces
{
    public interface IOrderService
    {
        IEnumerable<Order> GetAllOrders();
        IEnumerable<Order> GetOrdersByUser(int userId);
        IEnumerable<Order> GetActiveOrders();
        Order GetOrderDetails(int orderId);
        void CreateOrder(
            string orderCode,
            int userId,
            DateTime orderDateTime,
            OrderStatus status,
            DateTime? estimatedDelivery,
            decimal discount,
            decimal DeliveryCost,
            decimal totalCost);
        void UpdateOrderStatus(int orderId, OrderStatus status);
        void DeleteOrder(int orderId);
    }
}
