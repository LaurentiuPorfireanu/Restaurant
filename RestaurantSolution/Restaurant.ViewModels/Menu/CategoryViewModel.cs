using Restaurant.ViewModels.Base;
using Restaurant.ViewModels.RestaurantMenu;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.ViewModels.Menu
{
    public class CategoryViewModel : ViewModelBase
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public ObservableCollection<PreparatViewModel> Preparate { get; set; }
        public ObservableCollection<MenuItemViewModel> Menus { get; set; }
        public bool HasPreparate { get; set; }
        public bool HasMenus { get; set; }
    }

}
