using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Restaurant.Data.Context;
using Restaurant.DataAccess.Interfaces;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Enums;

namespace Restaurant.DataAccess.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly RestaurantContext _context;
        public OrderRepository(RestaurantContext ctx) => _context = ctx;

        public IEnumerable<Order> GetAll()
            => _context.Orders
                   .FromSqlRaw("EXEC spGetAllOrders")
                   .AsNoTracking()
                   .ToList();

        public IEnumerable<Order> GetByUser(int userId)
            => _context.Orders
                   .FromSqlRaw("EXEC spGetOrdersByUser @p0", userId)
                   .AsNoTracking()
                   .ToList();

        public IEnumerable<Order> GetActive()
            => _context.Orders
                   .FromSqlRaw("EXEC spGetActiveOrders")
                   .AsNoTracking()
                   .ToList();

        public Order GetDetails(int orderId)
            => _context.Orders
                   .FromSqlRaw("EXEC spGetOrderDetails @p0", orderId)
                   .AsNoTracking()
                   .FirstOrDefault();

        public void Insert(
            string orderCode,
            int userId,
            DateTime orderDateTime,
            OrderStatus status,
            DateTime? estimatedDelivery,
            decimal discount,
            decimal DeliveryCost,
            decimal totalCost)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC spInsertOrder " +
                "@OrderCode = {0}, @UserID = {1}, @OrderDateTime = {2}, " +
                "@Status = {3}, @EstimatedDelivery = {4}, @Discount = {5}, " +
                "@DeliveryCost = {6}, @TotalCost = {7}",
                orderCode,
                userId,
                orderDateTime,
                (int)status,
                estimatedDelivery,
                discount,
                DeliveryCost,
                totalCost);
        }

        public void UpdateStatus(int orderId, OrderStatus status)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC spUpdateOrderStatus @OrderID = {0}, @Status = {1}",
                orderId, (int)status);
        }

        public void Delete(int orderId)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC spDeleteOrder @OrderID = {0}",
                orderId);
        }
    }
}
