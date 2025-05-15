using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Domain.Entities
{
    public class Menu
    {

        public int MenuID { get; set; }
        public string Name { get; set; }


        public int CategoryID { get; set; }
        public Category Category { get; set; }


        public ICollection<MenuPreparat> MenuPreparate { get; set; }
        public ICollection<OrderMenu> OrderMenus { get; set; }
    }
}
