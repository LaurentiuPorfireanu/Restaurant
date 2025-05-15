using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Domain.Entities
{
    public class Preparat
    {

        public int PreparatID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int QuantityPortie { get; set; }
        public int QuantityTotal { get; set; }



        public int CategoryID { get; set; }
        public Category Category { get; set; }



        public ICollection<PreparatAlergen> PreparatAlergens { get; set; }


        public ICollection<PreparatFoto> Fotos { get; set; }


        public ICollection<OrderDish> OrderDishes { get; set; }
        public ICollection<MenuPreparat> MenuPreparate { get; set; }
    }
}
