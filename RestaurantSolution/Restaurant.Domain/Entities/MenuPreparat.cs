using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Domain.Entities
{
    public class MenuPreparat
    {

        public int MenuID { get; set; }
        public Menu Menu { get; set; }

        public int PreparatID { get; set; }
        public Preparat Preparat { get; set; }

        public int QuantityMenuPortie { get; set; }
    }
}
