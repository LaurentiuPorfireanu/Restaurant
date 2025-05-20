using Restaurant.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.ViewModels.Search.Extras
{
    public class SearchResultViewModel : ViewModelBase
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public string TypeName { get; set; }
        public string TypeBackground { get; set; }
        public string QuantityInfo { get; set; }
        public string PriceFormatted { get; set; }
        public bool IsAvailable { get; set; }
        public string AlergenInfo { get; set; }
    }
}
