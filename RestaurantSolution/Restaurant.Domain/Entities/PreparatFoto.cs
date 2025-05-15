using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Domain.Entities
{
    public class PreparatFoto
    {

        public int FotoID { get; set; }
        public string ImagePath { get; set; }

        public int PreparatID { get; set; }
        public Preparat Preparat { get; set; }
    }
}
