using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Domain.Entities
{
    public class OrderMenu
    {
        public int OrderID { get; set; }
        public Order Order { get; set; }

        public int MenuID { get; set; }
        public Menu Menu { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

    }
}
