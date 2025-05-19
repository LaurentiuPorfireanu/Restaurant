// Crează un nou fișier MenuViewModel.cs în directorul ViewModels 

using System;
using System.Collections.Generic;
using System.Linq;
using Restaurant.Domain.Entities;
using Restaurant.ViewModels.Base;

namespace Restaurant.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private Menu _menu;
        private decimal _calculatedPrice;

        public int MenuID => _menu?.MenuID ?? 0;
        public string Name => _menu?.Name ?? string.Empty;
        public int CategoryID => _menu?.CategoryID ?? 0;
        public Category Category => _menu?.Category;
        public ICollection<MenuPreparat> MenuPreparate => _menu?.MenuPreparate;
        public int ItemCount => _menu?.MenuPreparate?.Count ?? 0;

        public Menu Menu
        {
            get => _menu;
            set
            {
                SetProperty(ref _menu, value);
                CalculatePrice();
                // Notificăm și proprietățile derivate
                OnPropertyChanged(nameof(MenuID));
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(CategoryID));
                OnPropertyChanged(nameof(Category));
                OnPropertyChanged(nameof(MenuPreparate));
                OnPropertyChanged(nameof(ItemCount));
            }
        }

        public decimal CalculatedPrice
        {
            get => _calculatedPrice;
            set => SetProperty(ref _calculatedPrice, value);
        }

        public string PriceFormatted => $"{CalculatedPrice:N2} Lei";

        private void CalculatePrice()
        {
            if (_menu?.MenuPreparate == null || !_menu.MenuPreparate.Any())
            {
                CalculatedPrice = 0;
                return;
            }

            // În mod normal, procentul de reducere ar trebui luat din configurare
            decimal discountPercentage = 10;

            decimal totalPrice = 0;
            foreach (var mp in _menu.MenuPreparate)
            {
                if (mp.Preparat != null)
                {
                    totalPrice += mp.Preparat.Price;
                }
            }

            CalculatedPrice = Math.Round(totalPrice * (1 - discountPercentage / 100), 2);
            OnPropertyChanged(nameof(PriceFormatted));
        }
    }
}