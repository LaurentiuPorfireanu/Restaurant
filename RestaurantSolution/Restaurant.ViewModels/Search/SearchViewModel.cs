using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using Restaurant.Domain.Entities;
using Restaurant.Services.Interfaces;
using Restaurant.ViewModels.Base;
using Restaurant.ViewModels.Commands;
using System.Windows.Input;

namespace Restaurant.ViewModels.Search
{
    public class SearchViewModel : ViewModelBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IPreparatService _preparatService;
        private readonly IMenuService _menuService;

        private string _searchTerm;
        private bool _isContainsSearch;
        private bool _isNotContainsSearch;
        private bool _isLoading;
        private ObservableCollection<SearchResultViewModel> _searchResults;
        private int _resultsCount;
        private bool _hasNoResults;
        private bool _hasSearched;

        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                _searchTerm = value;
                OnPropertyChanged(nameof(SearchTerm));
            }
        }

        public bool IsContainsSearch
        {
            get => _isContainsSearch;
            set
            {
                _isContainsSearch = value;
                OnPropertyChanged(nameof(IsContainsSearch));

                if (value && IsNotContainsSearch)
                {
                    IsNotContainsSearch = false;
                }
            }
        }

        public bool IsNotContainsSearch
        {
            get => _isNotContainsSearch;
            set
            {
                _isNotContainsSearch = value;
                OnPropertyChanged(nameof(IsNotContainsSearch));

                if (value && IsContainsSearch)
                {
                    IsContainsSearch = false;
                }
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

        public ObservableCollection<SearchResultViewModel> SearchResults
        {
            get => _searchResults;
            set
            {
                _searchResults = value;
                OnPropertyChanged(nameof(SearchResults));
            }
        }

        public int ResultsCount
        {
            get => _resultsCount;
            set
            {
                _resultsCount = value;
                OnPropertyChanged(nameof(ResultsCount));
                HasNoResults = _hasSearched && value == 0;
            }
        }

        public bool HasNoResults
        {
            get => _hasNoResults;
            set
            {
                _hasNoResults = value;
                OnPropertyChanged(nameof(HasNoResults));
            }
        }

        public ICommand SearchCommand { get; }

        public SearchViewModel(ICategoryService categoryService,
                               IPreparatService preparatService,
                               IMenuService menuService)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _preparatService = preparatService ?? throw new ArgumentNullException(nameof(preparatService));
            _menuService = menuService ?? throw new ArgumentNullException(nameof(menuService));

            IsContainsSearch = true;
            SearchResults = new ObservableCollection<SearchResultViewModel>();

            // Grupare rezultate după categorie
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(SearchResults);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("CategoryName");
            view.GroupDescriptions.Add(groupDescription);

            SearchCommand = new RelayCommand(_ => PerformSearch());
        }

        private void PerformSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
                return;

            try
            {
                IsLoading = true;
                _hasSearched = true;
                SearchResults.Clear();

                var categories = _categoryService.GetAllCategories().ToList();

                foreach (var category in categories)
                {
                    // Caută în preparate
                    var preparate = _preparatService.GetPreparateByCategory(category.CategoryId);
                    foreach (var preparat in preparate)
                    {
                        bool matchesSearch = false;

                        // Verifică dacă numele preparatului conține termenul de căutare
                        bool nameContainsTerm = preparat.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase);

                        // Verifică dacă alergenii preparatului conțin termenul de căutare
                        bool alergensContainTerm = false;
                        if (preparat.PreparatAlergens != null)
                        {
                            alergensContainTerm = preparat.PreparatAlergens.Any(pa =>
                                pa.Alergen != null &&
                                pa.Alergen.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));
                        }

                        // Stabilește dacă preparatul se potrivește cu criteriile de căutare
                        if (IsContainsSearch)
                        {
                            matchesSearch = nameContainsTerm || alergensContainTerm;
                        }
                        else // IsNotContainsSearch
                        {
                            matchesSearch = !(nameContainsTerm || alergensContainTerm);
                        }

                        if (matchesSearch)
                        {
                            SearchResults.Add(new SearchResultViewModel
                            {
                                Id = preparat.PreparatID,
                                CategoryName = category.Name,
                                Name = preparat.Name,
                                TypeName = "Preparat",
                                TypeBackground = "#2196F3", // albastru
                                QuantityInfo = $"{preparat.QuantityPortie}g/porție",
                                PriceFormatted = $"{preparat.Price:N2} Lei",
                                IsAvailable = preparat.QuantityTotal > 0,
                                AlergenInfo = GetAlergenInfo(preparat)
                            });
                        }
                    }

                    // Caută în meniuri
                    var menus = _menuService.GetMenusByCategory(category.CategoryId);
                    foreach (var menu in menus)
                    {
                        bool matchesSearch = false;

                        // Verifică dacă numele meniului conține termenul de căutare
                        bool nameContainsTerm = menu.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase);

                        // Verifică dacă vreunul dintre preparatele din meniu conține termenul de căutare
                        bool menuItemsContainTerm = false;
                        if (menu.MenuPreparate != null)
                        {
                            menuItemsContainTerm = menu.MenuPreparate.Any(mp =>
                                mp.Preparat != null &&
                                mp.Preparat.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));
                        }

                        // Verifică dacă vreunul dintre alergenii preparatelor din meniu conține termenul de căutare
                        bool menuAlergensContainTerm = false;
                        if (menu.MenuPreparate != null)
                        {
                            menuAlergensContainTerm = menu.MenuPreparate.Any(mp =>
                                mp.Preparat?.PreparatAlergens != null &&
                                mp.Preparat.PreparatAlergens.Any(pa =>
                                    pa.Alergen != null &&
                                    pa.Alergen.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)));
                        }

                        // Stabilește dacă meniul se potrivește cu criteriile de căutare
                        if (IsContainsSearch)
                        {
                            matchesSearch = nameContainsTerm || menuItemsContainTerm || menuAlergensContainTerm;
                        }
                        else // IsNotContainsSearch
                        {
                            matchesSearch = !(nameContainsTerm || menuItemsContainTerm || menuAlergensContainTerm);
                        }

                        if (matchesSearch)
                        {
                            SearchResults.Add(new SearchResultViewModel
                            {
                                Id = menu.MenuID,
                                CategoryName = category.Name,
                                Name = menu.Name,
                                TypeName = "Meniu",
                                TypeBackground = "#4CAF50", // verde
                                QuantityInfo = GetMenuQuantityInfo(menu),
                                PriceFormatted = $"{CalculateMenuPrice(menu):N2} Lei",
                                IsAvailable = IsMenuAvailable(menu),
                                AlergenInfo = GetMenuAlergenInfo(menu)
                            });
                        }
                    }
                }

                ResultsCount = SearchResults.Count;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Eroare la căutare: {ex.Message}", "Eroare",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private string GetAlergenInfo(Preparat preparat)
        {
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

        private string GetMenuQuantityInfo(Restaurant.Domain.Entities.Menu menu)
        {
            if (menu.MenuPreparate != null && menu.MenuPreparate.Any())
            {
                return string.Join(", ", menu.MenuPreparate
                    .OrderBy(mp => mp.Preparat?.Name)
                    .Select(mp => $"{mp.QuantityMenuPortie}g"));
            }

            return "N/A";
        }

        private string GetMenuAlergenInfo(Restaurant.Domain.Entities.Menu menu)
        {
            if (menu.MenuPreparate != null && menu.MenuPreparate.Any())
            {
                var allAlergens = menu.MenuPreparate
                    .Where(mp => mp.Preparat?.PreparatAlergens != null)
                    .SelectMany(mp => mp.Preparat.PreparatAlergens)
                    .Where(pa => pa.Alergen != null)
                    .Select(pa => pa.Alergen.Name)
                    .Distinct()
                    .Where(name => !string.IsNullOrEmpty(name))
                    .OrderBy(name => name); // Sort allergens alphabetically for better readability

                if (allAlergens.Any())
                    return $"Alergeni: {string.Join(", ", allAlergens)}";
            }

            return "Fără alergeni";
        }

        private decimal CalculateMenuPrice(Restaurant.Domain.Entities.Menu menu)
        {
            if (menu.MenuPreparate == null || !menu.MenuPreparate.Any())
                return 0;

            // În mod normal ai extrage valoarea reducerii din configurare
            decimal discountPercentage = 10; // Presupunem 10% reducere pentru meniuri

            decimal totalPrice = menu.MenuPreparate.Sum(mp => mp.Preparat?.Price ?? 0);
            return totalPrice * (1 - discountPercentage / 100);
        }

        private bool IsMenuAvailable(Restaurant.Domain.Entities.Menu menu)
        {
            if (menu.MenuPreparate == null || !menu.MenuPreparate.Any())
                return false;

            return menu.MenuPreparate.All(mp => (mp.Preparat?.QuantityTotal ?? 0) > 0);
        }
    }

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