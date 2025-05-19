using Restaurant.Domain.Enums;
using Restaurant.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.ViewModels.Admin
{
    public class OrderViewModel : ViewModelBase
    {
        public int OrderID { get; set; }
        public string OrderCode { get; set; }
        public DateTime OrderDateTime { get; set; }
        public string OrderDateFormatted { get; set; }
        public string ClientName { get; set; }
        public OrderStatus Status { get; set; }
        public string StatusText { get; set; }
        public string StatusBackground { get; set; }
        public decimal TotalCost { get; set; }
        public string TotalCostFormatted { get; set; }
        public bool CanUpdateStatus { get; set; }
    }

}
