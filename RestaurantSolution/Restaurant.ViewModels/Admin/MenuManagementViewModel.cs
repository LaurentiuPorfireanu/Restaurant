using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Restaurant.Domain.Entities;
using Restaurant.Services.Interfaces;
using Restaurant.ViewModels.Base;
using Restaurant.ViewModels.Commands;
using System.Configuration;
using Restaurant.ViewModels.Admin.Extras;

namespace Restaurant.ViewModels.Admin
{
    public class MenuManagementViewModel : ViewModelBase
    {
        private readonly IMenuService _menuService;
        private readonly ICategoryService _categoryService;
        private readonly IPreparatService _preparatService;

        private string _menuName;
        private Category _selectedCategory;
        private ObservableCollection<PreparatForMenuViewModel> _availablePreparate;
        private ObservableCollection<PreparatForMenuViewModel> _selectedPreparate;
        private Restaurant.Domain.Entities.Menu _currentMenu;
        private decimal _totalPrice;
        private decimal _discountedPrice;
        private bool _isAddMode;
        private bool _isEditMode;
        private bool _isLoading;

        private decimal _discountPercentage;
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

        public string MenuName
        {
            get => _menuName;
            set
            {
                if (_menuName != value)
                {
                    _menuName = value;
                    OnPropertyChanged(nameof(MenuName));
                    OnPropertyChanged(nameof(CanSaveMenu));
                    System.Diagnostics.Debug.WriteLine($"MenuName schimbat la: '{_menuName}'");
                }
            }
        }

        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory != value)
                {
                    _selectedCategory = value;
                    OnPropertyChanged(nameof(SelectedCategory));
                    OnPropertyChanged(nameof(CanSaveMenu));
                    System.Diagnostics.Debug.WriteLine($"SelectedCategory schimbat la: {_selectedCategory?.Name}");
                }
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

        public bool CanSaveMenu
        {
            get
            {
                bool nameValid = !string.IsNullOrWhiteSpace(MenuName);
                bool categoryValid = SelectedCategory != null;
                bool preparateValid = SelectedPreparate?.Count >= 1;
                bool loadingValid = !IsLoading;

                System.Diagnostics.Debug.WriteLine($"CanSaveMenu evaluat: nameValid={nameValid}, categoryValid={categoryValid}, preparateValid={preparateValid}, loadingValid={loadingValid}");

                return nameValid && categoryValid && preparateValid && loadingValid;
            }
        }

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
            try
            {
                TotalPrice = SelectedPreparate.Sum(p => p.Preparat?.Price ?? 0);
                DiscountedPrice = Math.Round(TotalPrice * (1 - DiscountPercentage / 100), 2);

                System.Diagnostics.Debug.WriteLine($"Preț recalculat: Total = {TotalPrice:F2} Lei, Cu reducere = {DiscountedPrice:F2} Lei");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Eroare la recalcularea prețului: {ex.Message}");
            }
        }

        private void SaveMenu()
        {
            
            if (string.IsNullOrWhiteSpace(MenuName) || SelectedCategory == null || SelectedPreparate.Count < 1)
            {
                string errorMessage = "Nu se poate salva meniul. Verificați următoarele:";
                if (string.IsNullOrWhiteSpace(MenuName))
                    errorMessage += "\n- Numele meniului este obligatoriu";
                if (SelectedCategory == null)
                    errorMessage += "\n- Trebuie selectată o categorie";
                if (SelectedPreparate.Count < 1)
                    errorMessage += "\n- Trebuie adăugat cel puțin un preparat";

                System.Diagnostics.Debug.WriteLine("Nu se poate salva meniul - validare eșuată");
                System.Diagnostics.Debug.WriteLine(errorMessage);

                MessageBox.Show(errorMessage, "Validare eșuată", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

           
            decimal orderThreshold = 100; 

            string configThreshold = ConfigurationManager.AppSettings["OrderDiscountThreshold"];
            if (!string.IsNullOrEmpty(configThreshold) && decimal.TryParse(configThreshold, out decimal thresholdValue))
            {
                orderThreshold = thresholdValue;
                System.Diagnostics.Debug.WriteLine($"Threshold citit din configurare: {thresholdValue} Lei");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Se folosește threshold-ul implicit: {orderThreshold} Lei");
            }

            
            decimal totalPrice = TotalPrice;

            
            if (totalPrice > orderThreshold)
            {
                string warningMessage = $"Atenție: Prețul total al meniului ({totalPrice:N2} Lei) depășește pragul recomandat de {orderThreshold:N2} Lei.\n\nDoriți să continuați oricum?";
                System.Diagnostics.Debug.WriteLine(warningMessage);

                var result = MessageBox.Show(warningMessage, "Avertisment", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.No)
                {
                    System.Diagnostics.Debug.WriteLine("Utilizatorul a ales să nu continue");
                    return;
                }

                System.Diagnostics.Debug.WriteLine("Utilizatorul a ales să continue");
            }

           
            IsLoading = true;

            try
            {
                System.Diagnostics.Debug.WriteLine("=== DEBUGGING SALVARE MENIU ===");
                System.Diagnostics.Debug.WriteLine($"MenuName: '{MenuName}', IsEmpty: {string.IsNullOrWhiteSpace(MenuName)}");
                System.Diagnostics.Debug.WriteLine($"SelectedCategory: {(SelectedCategory != null ? $"{SelectedCategory.Name} (ID: {SelectedCategory.CategoryId})" : "null")}");
                System.Diagnostics.Debug.WriteLine($"SelectedPreparate.Count: {SelectedPreparate.Count}");
                System.Diagnostics.Debug.WriteLine($"IsLoading: {IsLoading}");

                
                System.Diagnostics.Debug.WriteLine("Validare reușită, continuă salvarea...");

                int menuId;

                if (IsAddMode)
                {
                    
                    System.Diagnostics.Debug.WriteLine($"Începe crearea unui nou meniu: '{MenuName}', CategoryId: {SelectedCategory.CategoryId}");

                    try
                    {
                        menuId = _menuService.CreateMenu(MenuName, SelectedCategory.CategoryId);
                        System.Diagnostics.Debug.WriteLine($"Meniu creat cu ID: {menuId}");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Eroare la crearea meniului: {ex.Message}");
                        System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                        throw; 
                    }
                }
                else
                {
                  
                    menuId = _currentMenu.MenuID;
                    System.Diagnostics.Debug.WriteLine($"Actualizare meniu existent ID: {menuId}");

                    try
                    {
                        _menuService.UpdateMenu(menuId, MenuName, SelectedCategory.CategoryId);
                        System.Diagnostics.Debug.WriteLine("Meniu actualizat cu succes");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Eroare la actualizarea meniului: {ex.Message}");
                        System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                        throw; 
                    }
                }

               
                System.Diagnostics.Debug.WriteLine($"Ștergere preparate existente pentru meniul ID: {menuId}");
                try
                {
                    _menuService.RemoveAllPreparateFromMenu(menuId);
                    System.Diagnostics.Debug.WriteLine("Preparate șterse cu succes");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Eroare la ștergerea preparatelor: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                    throw;
                }

                
                System.Diagnostics.Debug.WriteLine($"Adăugare {SelectedPreparate.Count} preparate la meniul ID: {menuId}");
                foreach (var preparat in SelectedPreparate)
                {
                    try
                    {
                        System.Diagnostics.Debug.WriteLine($"Adăugare preparat: {preparat.Preparat.Name}, ID: {preparat.Preparat.PreparatID}, Cantitate: {preparat.QuantityMenuPortie}");
                        _menuService.AddPreparatToMenu(menuId, preparat.Preparat.PreparatID, preparat.QuantityMenuPortie);
                        System.Diagnostics.Debug.WriteLine("Preparat adăugat cu succes");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Eroare la adăugarea preparatului: {ex.Message}");
                        System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                        throw; 
                    }
                }

              
                string message = IsAddMode
                    ? "Meniul a fost creat cu succes!"
                    : "Meniul a fost actualizat cu succes!";

                System.Diagnostics.Debug.WriteLine(message);
                MessageBox.Show(message, "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

                
                OnMenuSaved?.Invoke(this, EventArgs.Empty);

               
                System.Diagnostics.Debug.WriteLine("Resetare formular și ieșire din modul editare/adăugare");
                Cancel();

                System.Diagnostics.Debug.WriteLine("=== SFÂRȘIT DEBUGGING SALVARE MENIU ===");
            }
            catch (Exception ex)
            {
            
                System.Diagnostics.Debug.WriteLine($"EROARE NEAȘTEPTATĂ: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Tip excepție: {ex.GetType().FullName}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    System.Diagnostics.Debug.WriteLine($"Inner Exception StackTrace: {ex.InnerException.StackTrace}");
                }

                MessageBox.Show($"Eroare la salvarea meniului: {ex.Message}\n\nDetalii: {(ex.InnerException != null ? ex.InnerException.Message : "Nicio detaliere disponibilă")}",
                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                
                IsLoading = false;
                System.Diagnostics.Debug.WriteLine("IsLoading setat la false");
            }
        }

   
        public event EventHandler OnMenuSaved;

        
        public event EventHandler CancelRequested;

      
        public void Initialize(Restaurant.Domain.Entities.Menu menu = null)
        {
            IsLoading = true;

            try
            {
                System.Diagnostics.Debug.WriteLine("=== INIȚIALIZARE MANAGEMENT MENIU ===");

                _currentMenu = menu;

         
                MenuName = menu?.Name ?? string.Empty;
                System.Diagnostics.Debug.WriteLine($"MenuName inițializat: '{MenuName}'");

            
                if (menu?.Category != null)
                {
                    SelectedCategory = menu.Category;
                    System.Diagnostics.Debug.WriteLine($"SelectedCategory inițializat cu categoria meniului: {SelectedCategory.Name} (ID: {SelectedCategory.CategoryId})");
                }
                else
                {
                    var categoriess = _categoryService.GetAllCategories();
                    SelectedCategory = categoriess.FirstOrDefault();
                    System.Diagnostics.Debug.WriteLine($"SelectedCategory inițializat cu prima categorie disponibilă: {(SelectedCategory != null ? $"{SelectedCategory.Name} (ID: {SelectedCategory.CategoryId})" : "null")}");

                    if (SelectedCategory == null)
                    {
                        System.Diagnostics.Debug.WriteLine("AVERTISMENT: Nu s-a găsit nicio categorie disponibilă!");
                        MessageBox.Show("Nu există categorii disponibile. Vă rugăm să creați mai întâi o categorie.",
                            "Lipsesc categoriile", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }

               
                SelectedPreparate.Clear();
                AvailablePreparate.Clear();
                System.Diagnostics.Debug.WriteLine("Liste de preparate resetate");

               
                decimal defaultDiscount = 10; 

                string configDiscount = ConfigurationManager.AppSettings["MenuDiscountPercentage"];
                if (!string.IsNullOrEmpty(configDiscount) && decimal.TryParse(configDiscount, out decimal discountValue))
                {
                    DiscountPercentage = discountValue;
                    System.Diagnostics.Debug.WriteLine($"Discount-ul încărcat din configurare: {discountValue}%");
                }
                else
                {
                    DiscountPercentage = defaultDiscount;
                    System.Diagnostics.Debug.WriteLine($"Se folosește discount-ul implicit: {defaultDiscount}%");
                }

           
                System.Diagnostics.Debug.WriteLine("Începe încărcarea preparatelor disponibile...");
                var categories = _categoryService.GetAllCategories();
                int totalPreparate = 0;

                foreach (var category in categories)
                {
                    try
                    {
                        var preparate = _preparatService.GetPreparateByCategory(category.CategoryId);
                        int categoryPreparateCount = 0;

                        foreach (var preparat in preparate)
                        {
                            
                            if (menu != null && menu.MenuPreparate != null &&
                                menu.MenuPreparate.Any(mp => mp.PreparatID == preparat.PreparatID))
                                continue;

                            AvailablePreparate.Add(new PreparatForMenuViewModel
                            {
                                Preparat = preparat,
                                QuantityMenuPortie = preparat.QuantityPortie / 2, 
                                Category = category
                            });
                            categoryPreparateCount++;
                            totalPreparate++;
                        }

                        System.Diagnostics.Debug.WriteLine($"S-au încărcat {categoryPreparateCount} preparate din categoria {category.Name}");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Eroare la încărcarea preparatelor din categoria {category.Name}: {ex.Message}");
                    }
                }

                System.Diagnostics.Debug.WriteLine($"Total preparate disponibile încărcate: {totalPreparate}");

              
                if (menu != null && menu.MenuPreparate != null)
                {
                    System.Diagnostics.Debug.WriteLine("Încărcare preparate existente pentru meniul de editat...");
                    int loadedCount = 0;

                    foreach (var mp in menu.MenuPreparate)
                    {
                        if (mp.Preparat != null)
                        {
                            try
                            {
                                SelectedPreparate.Add(new PreparatForMenuViewModel
                                {
                                    Preparat = mp.Preparat,
                                    QuantityMenuPortie = mp.QuantityMenuPortie,
                                    Category = mp.Preparat.Category
                                });
                                loadedCount++;
                                System.Diagnostics.Debug.WriteLine($"Preparat adăugat în lista selectate: {mp.Preparat.Name}");
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"Eroare la adăugarea preparatului {mp.Preparat.Name} în lista selectate: {ex.Message}");
                            }
                        }
                    }

                    System.Diagnostics.Debug.WriteLine($"Total preparate existente încărcate: {loadedCount}");
                }

            
                IsAddMode = menu == null;
                IsEditMode = menu != null;
                System.Diagnostics.Debug.WriteLine($"Mod: {(IsAddMode ? "Adăugare" : "Editare")}");

           
                RecalculatePrice();
                System.Diagnostics.Debug.WriteLine($"Preț calculat: {TotalPrice:F2} Lei, Cu reducere: {DiscountedPrice:F2} Lei");

                System.Diagnostics.Debug.WriteLine("=== SFÂRȘIT INIȚIALIZARE MANAGEMENT MENIU ===");
                System.Diagnostics.Debug.WriteLine($"Stare finală: MenuName='{MenuName}', Category={SelectedCategory?.Name}, SelectedPreparate.Count={SelectedPreparate.Count}, CanSaveMenu={CanSaveMenu}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EROARE LA INIȚIALIZARE: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                MessageBox.Show($"Eroare la inițializarea formularului: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
              
                IsLoading = false;
                System.Diagnostics.Debug.WriteLine("IsLoading setat la false");

               
                OnPropertyChanged(nameof(MenuName));
                OnPropertyChanged(nameof(SelectedCategory));
                OnPropertyChanged(nameof(SelectedPreparate));
                OnPropertyChanged(nameof(AvailablePreparate));
                OnPropertyChanged(nameof(IsAddMode));
                OnPropertyChanged(nameof(IsEditMode));
                OnPropertyChanged(nameof(TotalPrice));
                OnPropertyChanged(nameof(DiscountedPrice));
                OnPropertyChanged(nameof(CanSaveMenu));
            }


            Application.Current.Dispatcher.Invoke(() =>
            {
               
                OnPropertyChanged(nameof(MenuName));
                OnPropertyChanged(nameof(SelectedCategory));
                OnPropertyChanged(nameof(SelectedPreparate));
                OnPropertyChanged(nameof(AvailablePreparate));
                OnPropertyChanged(nameof(IsAddMode));
                OnPropertyChanged(nameof(IsEditMode));
                OnPropertyChanged(nameof(TotalPrice));
                OnPropertyChanged(nameof(DiscountedPrice));
                OnPropertyChanged(nameof(CanSaveMenu));

             
                (SaveMenuCommand as RelayCommand)?.RaiseCanExecuteChanged();

           
                System.Diagnostics.Debug.WriteLine($"CanSaveMenu după Initialize pe UI thread: {CanSaveMenu}");
                bool nameValid = !string.IsNullOrWhiteSpace(MenuName);
                bool categoryValid = SelectedCategory != null;
                bool preparateValid = SelectedPreparate?.Count >= 1;
                bool loadingValid = !IsLoading;
                System.Diagnostics.Debug.WriteLine($"Condiții finale: nameValid={nameValid}, categoryValid={categoryValid}, preparateValid={preparateValid}, loadingValid={loadingValid}");
            });
        }

      

        private void Cancel()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Începe procesul de anulare...");

             
                MenuName = string.Empty;
                System.Diagnostics.Debug.WriteLine("MenuName resetat");

                SelectedCategory = _categoryService.GetAllCategories().FirstOrDefault();
                System.Diagnostics.Debug.WriteLine($"SelectedCategory resetat la: {SelectedCategory?.Name}");

                SelectedPreparate.Clear();
                System.Diagnostics.Debug.WriteLine("Lista de preparate selectate golită");

                _currentMenu = null;
                IsAddMode = false;
                IsEditMode = false;
                System.Diagnostics.Debug.WriteLine("Mod de operare resetat");

                RecalculatePrice();
                System.Diagnostics.Debug.WriteLine("Preț recalculat după resetare");

  
                CancelRequested?.Invoke(this, EventArgs.Empty);
                System.Diagnostics.Debug.WriteLine("Eveniment CancelRequested declanșat");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Eroare în timpul anulării: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
            }
        }

        
        
    }

    
}
