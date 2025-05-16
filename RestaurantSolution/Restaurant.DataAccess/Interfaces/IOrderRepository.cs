using System;
using System.Collections.Generic;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Enums;

namespace Restaurant.DataAccess.Interfaces
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetAll();
        IEnumerable<Order> GetByUser(int userId);
        IEnumerable<Order> GetActive();
        Order GetDetails(int orderId);

        void Insert(
            string orderCode,
            int userId,
            DateTime orderDateTime,
            OrderStatus status,
            DateTime? estimatedDelivery,
            decimal discount,
            decimal DeliveryCost,
            decimal totalCost);

        void UpdateStatus(int orderId, OrderStatus status);
        void Delete(int orderId);
    }
}
