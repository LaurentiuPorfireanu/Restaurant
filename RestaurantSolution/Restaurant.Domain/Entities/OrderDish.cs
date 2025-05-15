using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Domain.Entities
{
    public class OrderDish
    {

        public int OrderID { get; set; }
        public Order Order { get; set; }

        public int PreparatID { get; set; }
        public Preparat Preparat { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
