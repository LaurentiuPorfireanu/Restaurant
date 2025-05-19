using Restaurant.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.ViewModels.Menu.Extras
{
    public class MenuPreparatViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string Quantity { get; set; }
    }
}
