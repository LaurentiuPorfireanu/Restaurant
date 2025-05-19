using Restaurant.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.ViewModels.Menu
{
    public class PreparatViewModel : ViewModelBase
    {
        public int PreparatId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string PriceFormatted { get; set; }
        public int QuantityPortie { get; set; }
        public int QuantityTotal { get; set; }
        public string QuantityInfo { get; set; }
        public bool IsAvailable { get; set; }
        public string FirstImagePath { get; set; }
        public string AlergenInfo { get; set; }
    }
}
