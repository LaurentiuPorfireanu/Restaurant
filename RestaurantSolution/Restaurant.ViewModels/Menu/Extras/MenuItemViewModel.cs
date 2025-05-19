using Restaurant.ViewModels.Base;
using Restaurant.ViewModels.RestaurantMenu;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.ViewModels.Menu.Extras
{
    public class MenuItemViewModel : ViewModelBase
    {
        public int MenuId { get; set; }
        public string Name { get; set; }
        public string PriceFormatted { get; set; }
        public bool IsAvailable { get; set; }
        public ObservableCollection<MenuPreparatViewModel> MenuPreparate { get; set; }

        public Domain.Entities.Menu Menu { get; set; }
        public decimal Price { get; set; }
    }
}
