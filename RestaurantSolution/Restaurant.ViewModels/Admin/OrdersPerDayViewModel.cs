using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.ViewModels.Admin
{
    public class OrdersPerDayViewModel
    {
        public DateTime Date { get; set; }
        public string DateFormatted { get; set; }
        public int OrderCount { get; set; }
        public decimal TotalValue { get; set; }
        public string TotalValueFormatted { get; set; }
    }
}
