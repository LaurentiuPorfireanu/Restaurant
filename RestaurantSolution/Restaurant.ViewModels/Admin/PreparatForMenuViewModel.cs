using Restaurant.Domain.Entities;
using Restaurant.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.ViewModels.Admin
{
    public class PreparatForMenuViewModel : ViewModelBase
    {
        private Preparat _preparat;
        private int _quantityMenuPortie;
        private Category _category;

        public Preparat Preparat
        {
            get => _preparat;
            set => SetProperty(ref _preparat, value);
        }

        public int QuantityMenuPortie
        {
            get => _quantityMenuPortie;
            set => SetProperty(ref _quantityMenuPortie, value);
        }

        public Category Category
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }

        public string DisplayName => Preparat?.Name;

        public string CategoryName => Category?.Name;

        public string StandardQuantity => $"{Preparat?.QuantityPortie ?? 0}g (porția standard)";

        public string MenuQuantity => $"{QuantityMenuPortie}g (în meniu)";

        public string PriceFormatted => $"{Preparat?.Price ?? 0:N2} Lei";
    }
}
