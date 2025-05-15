using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Domain.Entities
{
    public class PreparatAlergen
    {

        public int PreparatID { get; set; }
        public Preparat Preparat { get; set; }

        public int AlergenID { get; set; }
        public Alergen Alergen { get; set; }
    }
}
