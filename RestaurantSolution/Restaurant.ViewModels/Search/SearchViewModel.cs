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
using System.Collections.Generic;
using Restaurant.ViewModels.Search.Extras;

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
        public ICommand SearchCommand { get; }


        private readonly IAlergenService _alergenService;
        private List<Alergen> _allAlergens;
        #region Properties
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
        #endregion

        #region Constructors
        public SearchViewModel(ICategoryService categoryService,
                               IPreparatService preparatService,
                               IMenuService menuService,
                               IAlergenService alergenService)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _preparatService = preparatService ?? throw new ArgumentNullException(nameof(preparatService));
            _menuService = menuService ?? throw new ArgumentNullException(nameof(menuService));
            _alergenService = alergenService ?? throw new ArgumentNullException(nameof(alergenService));

            IsContainsSearch = true;
            SearchResults = new ObservableCollection<SearchResultViewModel>();

            // Group results by category
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(SearchResults);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("CategoryName");
            view.GroupDescriptions.Add(groupDescription);

            SearchCommand = new RelayCommand(_ => PerformSearch());

            // Initialize allergens list
            LoadAllergens();
        }

        #endregion


        #region Methods

        private void LoadAllergens()
        {
            try
            {
                _allAlergens = _alergenService.GetAllAlergens().ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading allergens: {ex.Message}");
                _allAlergens = new List<Alergen>();
            }
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
                string searchTerm = SearchTerm.Trim().ToLower();

               
                bool isAllergenSearch = IsAllergenSearch(searchTerm);

                foreach (var category in categories)
                {
                
                    var preparate = _preparatService.GetPreparateByCategory(category.CategoryId);
                    foreach (var preparat in preparate)
                    {
                        bool matchesSearch = false;

                      
                        bool nameContainsTerm = preparat.Name.ToLower().Contains(searchTerm);

                       
                        bool allergenMatch = false;
                        if (isAllergenSearch)
                        {
                   
                            var preparatAlergenIds = preparat.PreparatAlergens?.Select(pa => pa.AlergenID) ?? Enumerable.Empty<int>();

                           
                            var matchingAllergens = _allAlergens
                                .Where(a => a.Name.ToLower().Contains(searchTerm))
                                .Select(a => a.AlergenID);

                            bool hasMatchingAllergen = preparatAlergenIds.Any(id => matchingAllergens.Contains(id));

                            allergenMatch = IsContainsSearch ? hasMatchingAllergen : !hasMatchingAllergen;
                        }

                        if (isAllergenSearch)
                        {
                            matchesSearch = allergenMatch;
                        }
                        else
                        {
                            matchesSearch = IsContainsSearch ? nameContainsTerm : !nameContainsTerm;
                        }

                        if (matchesSearch)
                        {
                            SearchResults.Add(new SearchResultViewModel
                            {
                                Id = preparat.PreparatID,
                                CategoryName = category.Name,
                                Name = preparat.Name,
                                TypeName = "Preparat",
                                TypeBackground = "#2196F3", // blue
                                QuantityInfo = $"{preparat.QuantityPortie}g/porție",
                                PriceFormatted = $"{preparat.Price:N2} Lei",
                                IsAvailable = preparat.QuantityTotal > 0,
                                AlergenInfo = GetAlergenInfo(preparat)
                            });
                        }
                    }

                
                    var menus = _menuService.GetMenusByCategory(category.CategoryId);
                    foreach (var menu in menus)
                    {
                        bool matchesSearch = false;

                       
                        bool nameContainsTerm = menu.Name.ToLower().Contains(searchTerm);

                    
                        bool menuItemsContainTerm = menu.MenuPreparate?.Any(mp =>
                            mp.Preparat != null &&
                            mp.Preparat.Name.ToLower().Contains(searchTerm)) ?? false;

                      
                        bool allergenMatch = false;
                        if (isAllergenSearch)
                        {
                           
                            var menuAlergenIds = menu.MenuPreparate?
                                .Where(mp => mp.Preparat?.PreparatAlergens != null)
                                .SelectMany(mp => mp.Preparat.PreparatAlergens)
                                .Select(pa => pa.AlergenID)
                                .Distinct() ?? Enumerable.Empty<int>();

                        
                            var matchingAllergens = _allAlergens
                                .Where(a => a.Name.ToLower().Contains(searchTerm))
                                .Select(a => a.AlergenID);

                            bool hasMatchingAllergen = menuAlergenIds.Any(id => matchingAllergens.Contains(id));

                         
                            allergenMatch = IsContainsSearch ? hasMatchingAllergen : !hasMatchingAllergen;
                        }

                      
                        if (isAllergenSearch)
                        {
                            matchesSearch = allergenMatch;
                        }
                        else
                        {
                            if (IsContainsSearch)
                            {
                                matchesSearch = nameContainsTerm || menuItemsContainTerm;
                            }
                            else 
                            {
                                matchesSearch = !nameContainsTerm && !menuItemsContainTerm;
                            }
                        }

                        if (matchesSearch)
                        {
                            SearchResults.Add(new SearchResultViewModel
                            {
                                Id = menu.MenuID,
                                CategoryName = category.Name,
                                Name = menu.Name,
                                TypeName = "Meniu",
                                TypeBackground = "#4CAF50", // green
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

        private bool IsAllergenSearch(string searchTerm)
        {
            // Check if the search term matches any known allergen name
            return _allAlergens.Any(a => a.Name.ToLower().Contains(searchTerm));
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
        #endregion


    }

    
}