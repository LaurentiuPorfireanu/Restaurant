using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.ViewModels.Admin.Extras
{
    public class PopularProductViewModel
    {
        public string ProductName { get; set; }
        public int OrderCount { get; set; }
        public int TotalQuantity { get; set; }
        public decimal Revenue { get; set; }
        public string RevenueFormatted { get; set; }
    }
}
