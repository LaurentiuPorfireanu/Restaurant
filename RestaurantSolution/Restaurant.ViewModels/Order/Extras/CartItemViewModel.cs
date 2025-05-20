using Restaurant.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.ViewModels.Order.Extras
{
    public class CartItemViewModel : ViewModelBase
    {
        private int _quantity;
        private decimal _totalPrice;
        private string _totalPriceFormatted;

        public int ItemId { get; set; }
        public CartItemType ItemType { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public string UnitPriceFormatted { get; set; }
        public Domain.Entities.Preparat Preparat { get; set; }
        public Domain.Entities.Menu Menu { get; set; }
        public int MaxAvailableQuantity { get; set; }

        public int Quantity
        {
            get => _quantity;
            set
            {
                SetProperty(ref _quantity, value);
                TotalPrice = UnitPrice * Quantity;
                TotalPriceFormatted = $"{TotalPrice:N2} Lei";
            }
        }

        public decimal TotalPrice
        {
            get => _totalPrice;
            set => SetProperty(ref _totalPrice, value);
        }

        public string TotalPriceFormatted
        {
            get => _totalPriceFormatted;
            set => SetProperty(ref _totalPriceFormatted, value);
        }


       
    }
}
