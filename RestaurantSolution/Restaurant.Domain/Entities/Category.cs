using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Domain.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        public ICollection<Preparat> Preparate { get; set; }
        public ICollection<Menu> Menus { get; set; }
    }
}
