// Restaurant.ViewModels/Admin/MenuManagementViewModel.cs
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Restaurant.Domain.Entities;
using Restaurant.Services.Interfaces;
using Restaurant.ViewModels.Base;
using Restaurant.ViewModels.Commands;

namespace Restaurant.ViewModels.Admin
{
    public class MenuManagementViewModel : ViewModelBase
    {
        private readonly IMenuService _menuService;
        private readonly ICategoryService _categoryService;
        private readonly IPreparatService _preparatService;

        private string _menuName;
        private Category _selectedCategory;
        private decimal _discountPercentage = 10;  // Valoare implicită, ar trebui luată din configurare
        private ObservableCollection<PreparatForMenuViewModel> _availablePreparate;
        private ObservableCollection<PreparatForMenuViewModel> _selectedPreparate;
        private Menu _currentMenu;
        private decimal _totalPrice;
        private decimal _discountedPrice;
        private bool _isAddMode;
        private bool _isEditMode;
        private bool _isLoading;

        public string MenuName
        {
            get => _menuName;
            set
            {
                SetProperty(ref _menuName, value);
                OnPropertyChanged(nameof(CanSaveMenu));
            }
        }

        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                SetProperty(ref _selectedCategory, value);
                OnPropertyChanged(nameof(CanSaveMenu));
            }
        }

        public decimal DiscountPercentage
        {
            get => _discountPercentage;
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentException("Procentajul de reducere trebuie să fie între 0 și 100");

                SetProperty(ref _discountPercentage, value);
                RecalculatePrice();
            }
        }

        public ObservableCollection<PreparatForMenuViewModel> AvailablePreparate
        {
            get => _availablePreparate;
            set => SetProperty(ref _availablePreparate, value);
        }

        public ObservableCollection<PreparatForMenuViewModel> SelectedPreparate
        {
            get => _selectedPreparate;
            set
            {
                SetProperty(ref _selectedPreparate, value);
                RecalculatePrice();
            }
        }

        public decimal TotalPrice
        {
            get => _totalPrice;
            set => SetProperty(ref _totalPrice, value);
        }

        public decimal DiscountedPrice
        {
            get => _discountedPrice;
            set => SetProperty(ref _discountedPrice, value);
        }

        public bool IsAddMode
        {
            get => _isAddMode;
            set => SetProperty(ref _isAddMode, value);
        }

        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                SetProperty(ref _isLoading, value);
                OnPropertyChanged(nameof(CanSaveMenu));
            }
        }

        public bool CanSaveMenu =>
            !string.IsNullOrWhiteSpace(MenuName) &&
            SelectedCategory != null &&
            SelectedPreparate.Count >= 1 &&
            !IsLoading;

        public ICommand AddPreparatCommand { get; }
        public ICommand RemovePreparatCommand { get; }
        public ICommand SaveMenuCommand { get; }
        public ICommand CancelCommand { get; }

        public MenuManagementViewModel(
            IMenuService menuService,
            ICategoryService categoryService,
            IPreparatService preparatService)
        {
            _menuService = menuService ?? throw new ArgumentNullException(nameof(menuService));
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _preparatService = preparatService ?? throw new ArgumentNullException(nameof(preparatService));

            AvailablePreparate = new ObservableCollection<PreparatForMenuViewModel>();
            SelectedPreparate = new ObservableCollection<PreparatForMenuViewModel>();

            AddPreparatCommand = new RelayCommand(param => AddPreparat(param as PreparatForMenuViewModel));
            RemovePreparatCommand = new RelayCommand(param => RemovePreparat(param as PreparatForMenuViewModel));
            SaveMenuCommand = new RelayCommand(_ => SaveMenu(), _ => CanSaveMenu);
            CancelCommand = new RelayCommand(_ => Cancel());
        }

        public void Initialize(Menu menu = null)
        {
            IsLoading = true;

            try
            {
                _currentMenu = menu;

                // Resetăm formularul
                MenuName = menu?.Name ?? string.Empty;
                SelectedCategory = menu?.Category ?? _categoryService.GetAllCategories().FirstOrDefault();
                SelectedPreparate.Clear();
                AvailablePreparate.Clear();

                // Încărcăm toate preparatele disponibile
                var categories = _categoryService.GetAllCategories();
                foreach (var category in categories)
                {
                    var preparate = _preparatService.GetPreparateByCategory(category.CategoryId);
                    foreach (var preparat in preparate)
                    {
                        // Dacă suntem în modul de editare și preparatul este deja în meniu,
                        // nu-l adăugăm în lista de disponibile
                        if (menu != null && menu.MenuPreparate != null &&
                            menu.MenuPreparate.Any(mp => mp.PreparatID == preparat.PreparatID))
                            continue;

                        AvailablePreparate.Add(new PreparatForMenuViewModel
                        {
                            Preparat = preparat,
                            QuantityMenuPortie = preparat.QuantityPortie / 2, // Implicit, jumătate din porția normală
                            Category = category
                        });
                    }
                }

                // Dacă suntem în modul de editare, încărcăm preparatele selectate
                if (menu != null && menu.MenuPreparate != null)
                {
                    foreach (var mp in menu.MenuPreparate)
                    {
                        if (mp.Preparat != null)
                        {
                            SelectedPreparate.Add(new PreparatForMenuViewModel
                            {
                                Preparat = mp.Preparat,
                                QuantityMenuPortie = mp.QuantityMenuPortie,
                                Category = mp.Preparat.Category
                            });
                        }
                    }
                }

                // Setăm modul de operare
                IsAddMode = menu == null;
                IsEditMode = menu != null;

                // Recalculăm prețul
                RecalculatePrice();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void AddPreparat(PreparatForMenuViewModel preparat)
        {
            if (preparat == null)
                return;

            AvailablePreparate.Remove(preparat);
            SelectedPreparate.Add(preparat);

            RecalculatePrice();
        }

        private void RemovePreparat(PreparatForMenuViewModel preparat)
        {
            if (preparat == null)
                return;

            SelectedPreparate.Remove(preparat);
            AvailablePreparate.Add(preparat);

            RecalculatePrice();
        }

        private void RecalculatePrice()
        {
            TotalPrice = SelectedPreparate.Sum(p => p.Preparat?.Price ?? 0);
            DiscountedPrice = Math.Round(TotalPrice * (1 - DiscountPercentage / 100), 2);
        }

        private void SaveMenu()
        {
            IsLoading = true;

            try
            {
                if (!CanSaveMenu)
                    return;

                int menuId;

                if (IsAddMode)
                {
                    // Creăm un nou meniu
                    menuId = _menuService.CreateMenu(MenuName, SelectedCategory.CategoryId);
                }
                else
                {
                    // Actualizăm meniul existent
                    menuId = _currentMenu.MenuID;
                    _menuService.UpdateMenu(menuId, MenuName, SelectedCategory.CategoryId);

                    // Ștergem toate preparatele existente din meniu
                    _menuService.RemoveAllPreparateFromMenu(menuId);
                }

                // Adăugăm preparatele selectate la meniu
                foreach (var preparat in SelectedPreparate)
                {
                    _menuService.AddPreparatToMenu(
                        menuId,
                        preparat.Preparat.PreparatID,
                        preparat.QuantityMenuPortie);
                }

                // Afișăm un mesaj de succes
                string message = IsAddMode
                    ? "Meniul a fost creat cu succes!"
                    : "Meniul a fost actualizat cu succes!";

                MessageBox.Show(message, "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

                // Resetăm formularul și ieșim din modul de editare/adăugare
                Cancel();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la salvarea meniului: {ex.Message}", "Eroare",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void Cancel()
        {
            // Resetăm formular și ieșim din modul de editare/adăugare
            MenuName = string.Empty;
            SelectedCategory = _categoryService.GetAllCategories().FirstOrDefault();
            SelectedPreparate.Clear();
            _currentMenu = null;
            IsAddMode = false;
            IsEditMode = false;

            // Reîncărcăm preparatele disponibile
            Initialize();
        }
    }

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