using Restaurant.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Domain.Entities
{
    public class Order
    {

        public int OrderID { get; set; }
        public string OrderCode { get; set; }
        public DateTime OrderDateTime { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime? EstimatedDelivery { get; set; }

        public decimal Discount { get; set; }
        public decimal DeliveryCost { get; set; }

        public decimal TotalCost { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }

        public ICollection<OrderDish> OrderDishes { get; set; }
        public ICollection<OrderMenu> OrderMenus { get; set; }
    }
}
