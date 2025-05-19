using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.ViewModels.Admin
{
    public class LowStockItemViewModel
    {
        public string ProductName { get; set; }
        public int CurrentStock { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
    }
}
