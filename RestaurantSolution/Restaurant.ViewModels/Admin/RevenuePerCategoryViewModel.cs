using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.ViewModels.Admin
{
    public class RevenuePerCategoryViewModel
    {
        public string CategoryName { get; set; }
        public int OrderCount { get; set; }
        public decimal Revenue { get; set; }
        public string RevenueFormatted { get; set; }
        public decimal PercentageOfTotal { get; set; }
    }
}
