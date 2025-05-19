using Restaurant.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.ViewModels.Order
{
    public class OrderItemViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string UnitPriceFormatted { get; set; }
        public decimal TotalPrice { get; set; }
        public string TotalPriceFormatted { get; set; }
    }
}
