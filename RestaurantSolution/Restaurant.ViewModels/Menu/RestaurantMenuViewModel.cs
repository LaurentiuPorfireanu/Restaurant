using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Restaurant.Domain.Entities;
using Restaurant.Services.Interfaces;
using Restaurant.ViewModels.Base;

namespace Restaurant.ViewModels.RestaurantMenu
{
    public class RestaurantMenuViewModel : ViewModelBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IPreparatService _preparatService;
        private readonly IMenuService _menuService;

        private ObservableCollection<CategoryViewModel> _categories;
        private bool _isLoading;

        public ObservableCollection<CategoryViewModel> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public RestaurantMenuViewModel(ICategoryService categoryService,
                                       IPreparatService preparatService,
                                       IMenuService menuService)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _preparatService = preparatService ?? throw new ArgumentNullException(nameof(preparatService));
            _menuService = menuService ?? throw new ArgumentNullException(nameof(menuService));

            Categories = new ObservableCollection<CategoryViewModel>();
        }

        public async Task LoadDataAsync()
        {
            try
            {
                IsLoading = true;
                Categories.Clear();

                // Obține toate categoriile
                var categories = _categoryService.GetAllCategories().ToList();

                foreach (var category in categories)
                {
                    var categoryVm = new CategoryViewModel
                    {
                        CategoryId = category.CategoryId,
                        Name = category.Name,
                        Preparate = new ObservableCollection<PreparatViewModel>(),
                        Menus = new ObservableCollection<MenuItemViewModel>()
                    };

                    // Obține preparatele din categoria curentă
                    var preparate = _preparatService.GetPreparateByCategory(category.CategoryId);
                    foreach (var preparat in preparate)
                    {
                        categoryVm.Preparate.Add(new PreparatViewModel
                        {
                            PreparatId = preparat.PreparatID,
                            Name = preparat.Name,
                            Price = preparat.Price,
                            QuantityPortie = preparat.QuantityPortie,
                            QuantityTotal = preparat.QuantityTotal,
                            QuantityInfo = $"{preparat.QuantityPortie}g/porție",
                            PriceFormatted = $"{preparat.Price:N2} Lei",
                            IsAvailable = preparat.QuantityTotal > 0,
                            FirstImagePath = GetPreparatImagePath(preparat),
                            AlergenInfo = GetAlergenInfo(preparat)
                        });
                    }

                    // Obține meniurile din categoria curentă
                    var menus = _menuService.GetMenusByCategory(category.CategoryId);
                    foreach (var menu in menus)
                    {
                        var menuVm = new MenuItemViewModel
                        {
                            MenuId = menu.MenuID,
                            Name = menu.Name,
                            PriceFormatted = $"{CalculateMenuPrice(menu):N2} Lei",
                            IsAvailable = IsMenuAvailable(menu),
                            MenuPreparate = new ObservableCollection<MenuPreparatViewModel>()
                        };

                        // Adaugă elementele din meniu (preparatele)
                        if (menu.MenuPreparate != null)
                        {
                            foreach (var menuPreparat in menu.MenuPreparate)
                            {
                                menuVm.MenuPreparate.Add(new MenuPreparatViewModel
                                {
                                    Name = menuPreparat.Preparat?.Name ?? "Necunoscut",
                                    Quantity = $"{menuPreparat.QuantityMenuPortie}g"
                                });
                            }
                        }

                        categoryVm.Menus.Add(menuVm);
                    }

                    categoryVm.HasPreparate = categoryVm.Preparate.Count > 0;
                    categoryVm.HasMenus = categoryVm.Menus.Count > 0;

                    Categories.Add(categoryVm);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Eroare la încărcarea meniului: {ex.Message}", "Eroare",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private string GetPreparatImagePath(Preparat preparat)
        {
            // Verificăm dacă preparatul și colecția de fotografii există
            if (preparat?.Fotos != null && preparat.Fotos.Any())
            {
                try
                {
                    // Obținem prima fotografie
                    var foto = preparat.Fotos.First();
                    string relativePath = foto.ImagePath;

                    if (!string.IsNullOrEmpty(relativePath))
                    {
                        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                        string fullPath = Path.Combine(baseDirectory, relativePath);

                        System.Diagnostics.Debug.WriteLine($"Cale imagine: {fullPath}");
                        System.Diagnostics.Debug.WriteLine($"Fișierul există: {File.Exists(fullPath)}");

                        if (File.Exists(fullPath))
                            return fullPath;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Eroare la încărcarea imaginii: {ex.Message}");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Preparatul nu are imagini asociate.");
            }

            // Verificăm dacă imaginea implicită există
            string defaultPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "no_image.png");
            if (File.Exists(defaultPath))
                return defaultPath;

            // Dacă nu există, returnăm o cale relativă pentru imaginea implicită
            return "Resources/no_image.png";
        }

        private string GetAlergenInfo(Preparat preparat)
        {
            // Aici ar trebui să returnezi informațiile despre alergeni
            if (preparat.PreparatAlergens != null && preparat.PreparatAlergens.Any())
            {
                var alergens = preparat.PreparatAlergens
                    .Where(pa => pa.Alergen != null)
                    .Select(pa => pa.Alergen.Name)
                    .Where(name => !string.IsNullOrEmpty(name));

                return $"Alergeni: {string.Join(", ", alergens)}";
            }

            return "Fără alergeni";
        }

        private decimal CalculateMenuPrice(Domain.Entities.Menu menu)
        {
            // Calculează prețul meniului pe baza preparatelor incluse
            // În mod normal, ar trebui aplicată și reducerea specificată în configurare
            if (menu.MenuPreparate == null || !menu.MenuPreparate.Any())
                return 0;

            // În mod normal ai extrage valoarea reducerii din configurare
            decimal discountPercentage = 10; // Presupunem 10% reducere pentru meniuri

            decimal totalPrice = menu.MenuPreparate.Sum(mp => mp.Preparat?.Price ?? 0);
            return totalPrice * (1 - discountPercentage / 100);
        }

        private bool IsMenuAvailable(Domain.Entities.Menu menu)
        {
            // Un meniu este disponibil doar dacă toate preparatele incluse sunt disponibile
            if (menu.MenuPreparate == null || !menu.MenuPreparate.Any())
                return false;

            return menu.MenuPreparate.All(mp => (mp.Preparat?.QuantityTotal ?? 0) > 0);
        }
    }

    public class CategoryViewModel : ViewModelBase
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public ObservableCollection<PreparatViewModel> Preparate { get; set; }
        public ObservableCollection<MenuItemViewModel> Menus { get; set; }
        public bool HasPreparate { get; set; }
        public bool HasMenus { get; set; }
    }

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

    public class MenuItemViewModel : ViewModelBase
    {
        public int MenuId { get; set; }
        public string Name { get; set; }
        public string PriceFormatted { get; set; }
        public bool IsAvailable { get; set; }
        public ObservableCollection<MenuPreparatViewModel> MenuPreparate { get; set; }
    }

    public class MenuPreparatViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string Quantity { get; set; }
    }
}