using Restaurant.Domain.Enums;
using Restaurant.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.ViewModels.Order
{
    public class OrderViewModel : ViewModelBase
    {
        public int OrderID { get; set; }
        public string OrderCode { get; set; }
        public DateTime OrderDateTime { get; set; }
        public string OrderDateFormatted { get; set; }
        public OrderStatus Status { get; set; }
        public string StatusText { get; set; }
        public string StatusBackground { get; set; }
        public DateTime? EstimatedDelivery { get; set; }
        public string EstimatedDeliveryFormatted { get; set; }
        public string DeliveryAddress { get; set; }
        public decimal Discount { get; set; }
        public string DiscountFormatted { get; set; }
        public bool HasDiscount { get; set; }
        public decimal DeliveryCost { get; set; }
        public string DeliveryCostFormatted { get; set; }
        public decimal TotalCost { get; set; }
        public string TotalCostFormatted { get; set; }
        public decimal ItemsCost { get; set; }
        public string ItemsCostFormatted { get; set; }
        public bool CanCancel { get; set; }
        public ObservableCollection<OrderItemViewModel> OrderItems { get; set; }
    }

}
