using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Domain.Entities
{
    public  class Alergen
    {

        public int AlergenID { get; set; }
        public string Name { get; set; }
      
        public ICollection<PreparatAlergen> PreparatAlergens { get; set; }
    }
}
