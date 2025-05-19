using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Enums;
using Restaurant.Services.Interfaces;
using Restaurant.ViewModels.Base;
using Restaurant.ViewModels.Commands;
using Restaurant.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Restaurant.ViewModels.Admin
{
    public class AdminPanelViewModel : ViewModelBase
    {
        #region Services

        private readonly ICategoryService _categoryService;
        private readonly IPreparatService _preparatService;
        private readonly IAlergenService _alergenService;
        private readonly IMenuService _menuService;
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly IServiceProvider _serviceProvider;

        #endregion

        private bool _showReportFilters;
        private bool _isLowStockReportSelected;
        private bool _isOrdersReportSelected;
        private bool _isPopularProductsReportSelected;
        private bool _isReportLoading;
        private DateTime _reportStartDate;
        private DateTime _reportEndDate;
        private int _lowStockThreshold = 5;
        private int _topProductsCount = 20;


        public bool ShowReportFilters
        {
            get => _showReportFilters;
            set => SetProperty(ref _showReportFilters, value);
        }

        public bool IsLowStockReportSelected
        {
            get => _isLowStockReportSelected;
            set => SetProperty(ref _isLowStockReportSelected, value);
        }

        public bool IsOrdersReportSelected
        {
            get => _isOrdersReportSelected;
            set => SetProperty(ref _isOrdersReportSelected, value);
        }

        public bool IsPopularProductsReportSelected
        {
            get => _isPopularProductsReportSelected;
            set => SetProperty(ref _isPopularProductsReportSelected, value);
        }

        public bool IsReportLoading
        {
            get => _isReportLoading;
            set => SetProperty(ref _isReportLoading, value);
        }

        public DateTime ReportStartDate
        {
            get => _reportStartDate;
            set => SetProperty(ref _reportStartDate, value);
        }

        public DateTime ReportEndDate
        {
            get => _reportEndDate;
            set => SetProperty(ref _reportEndDate, value);
        }

        public int LowStockThreshold
        {
            get => _lowStockThreshold;
            set => SetProperty(ref _lowStockThreshold, value);
        }

        public int TopProductsCount
        {
            get => _topProductsCount;
            set => SetProperty(ref _topProductsCount, value);
        }

        public ICommand GenerateReportCommand { get; }

        #region Collections

        private ObservableCollection<Category> _categories;
        private ObservableCollection<Preparat> _preparate;
        private ObservableCollection<Alergen> _alergens;
        private ObservableCollection<MenuViewModel> _menus;
        private ObservableCollection<OrderViewModel> _orders;
        private ObservableCollection<ReportType> _reportTypes;
        private ObservableCollection<object> _reportData;

        #endregion

        #region Selected Items

        private Category _selectedCategory;
        private Preparat _selectedPreparat;
        private Alergen _selectedAlergen;
        private MenuViewModel _selectedMenu;
        private OrderViewModel _selectedOrder;
        private ReportType _selectedReportType;

        #endregion

        private string _preparatName;
        private decimal _preparatPrice;
        private int _preparatQuantityPortie;
        private int _preparatQuantityTotal;
        private Category _preparatSelectedCategory;
        private ObservableCollection<Alergen> _preparatAvailableAlergens;
        private ObservableCollection<Alergen> _preparatSelectedAlergens;
        private ObservableCollection<PreparatImageItem> _preparatImages;
        private Preparat _preparatBeingEdited;
        private bool _isAddPreparatMode;
        private bool _isEditPreparatMode;


        public string PreparatName
        {
            get => _preparatName;
            set
            {
                SetProperty(ref _preparatName, value);
                OnPropertyChanged(nameof(CanSavePreparat));
            }
        }

        public decimal PreparatPrice
        {
            get => _preparatPrice;
            set
            {
                SetProperty(ref _preparatPrice, value);
                OnPropertyChanged(nameof(CanSavePreparat));
            }
        }

        public int PreparatQuantityPortie
        {
            get => _preparatQuantityPortie;
            set
            {
                SetProperty(ref _preparatQuantityPortie, value);
                OnPropertyChanged(nameof(CanSavePreparat));
            }
        }

        public int PreparatQuantityTotal
        {
            get => _preparatQuantityTotal;
            set
            {
                SetProperty(ref _preparatQuantityTotal, value);
                OnPropertyChanged(nameof(CanSavePreparat));
            }
        }

        public Category PreparatSelectedCategory
        {
            get => _preparatSelectedCategory;
            set
            {
                SetProperty(ref _preparatSelectedCategory, value);
                OnPropertyChanged(nameof(CanSavePreparat));
            }
        }

        public ObservableCollection<Alergen> PreparatAvailableAlergens
        {
            get => _preparatAvailableAlergens;
            set => SetProperty(ref _preparatAvailableAlergens, value);
        }

        public ObservableCollection<Alergen> PreparatSelectedAlergens
        {
            get => _preparatSelectedAlergens;
            set => SetProperty(ref _preparatSelectedAlergens, value);
        }

        public ObservableCollection<PreparatImageItem> PreparatImages
        {
            get => _preparatImages;
            set => SetProperty(ref _preparatImages, value);
        }

        public bool IsAddPreparatMode
        {
            get => _isAddPreparatMode;
            set => SetProperty(ref _isAddPreparatMode, value);
        }

        public bool IsEditPreparatMode
        {
            get => _isEditPreparatMode;
            set => SetProperty(ref _isEditPreparatMode, value);
        }

        public bool CanSavePreparat =>
            !string.IsNullOrWhiteSpace(PreparatName) &&
            PreparatPrice >= 0 &&
            PreparatQuantityPortie > 0 &&
            PreparatQuantityTotal >= 0 &&
            PreparatSelectedCategory != null &&
            !IsLoading;


        #region Form Properties

        private string _categoryName;
        private string _alergenName;
        private bool _hasLowStockItems;
        private int _lowStockCount;
        private bool _isLoading;
        private bool _isAllOrdersSelected = true;
        private bool _isActiveOrdersSelected;
        private bool _hasReportData;

        #endregion

        private MenuManagementViewModel _menuManagement;

        public MenuManagementViewModel MenuManagement
        {
            get => _menuManagement;
            set => SetProperty(ref _menuManagement, value);
        }

        public ICommand AddNewMenuCommand { get; }
        public ICommand EditMenuCommand { get; }
        public ICommand DeleteMenuCommand { get; }

        #region Properties

        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }

        public ObservableCollection<Preparat> Preparate
        {
            get => _preparate;
            set => SetProperty(ref _preparate, value);
        }

        public ObservableCollection<Alergen> Alergens
        {
            get => _alergens;
            set => SetProperty(ref _alergens, value);
        }

        public ObservableCollection<MenuViewModel> Menus
        {
            get => _menus;
            set => SetProperty(ref _menus, value);
        }

        public ObservableCollection<OrderViewModel> Orders
        {
            get => _orders;
            set => SetProperty(ref _orders, value);
        }

        public ObservableCollection<ReportType> ReportTypes
        {
            get => _reportTypes;
            set => SetProperty(ref _reportTypes, value);
        }

        public ObservableCollection<object> ReportData
        {
            get => _reportData;
            set
            {
                // Instead of just setting the property, create a new ObservableCollection
                // This will force the DataGrid to completely re-evaluate and refresh
                _reportData = new ObservableCollection<object>(value ?? new ObservableCollection<object>());
                OnPropertyChanged(nameof(ReportData));
                HasReportData = _reportData != null && _reportData.Count > 0;
            }
        }

        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                SetProperty(ref _selectedCategory, value);
                if (value != null)
                {
                    CategoryName = value.Name;
                }
                OnPropertyChanged(nameof(IsUpdateCategoryEnabled));
            }
        }

        public Preparat SelectedPreparat
        {
            get => _selectedPreparat;
            set => SetProperty(ref _selectedPreparat, value);
        }

        public Alergen SelectedAlergen
        {
            get => _selectedAlergen;
            set
            {
                SetProperty(ref _selectedAlergen, value);
                if (value != null)
                {
                    AlergenName = value.Name;
                }
                OnPropertyChanged(nameof(IsUpdateAlergenEnabled));
            }
        }

        public MenuViewModel SelectedMenu
        {
            get => _selectedMenu;
            set => SetProperty(ref _selectedMenu, value);
        }

        public OrderViewModel SelectedOrder
        {
            get => _selectedOrder;
            set => SetProperty(ref _selectedOrder, value);
        }

        

        public string CategoryName
        {
            get => _categoryName;
            set
            {
                SetProperty(ref _categoryName, value);
                OnPropertyChanged(nameof(IsAddCategoryEnabled));
                OnPropertyChanged(nameof(IsUpdateCategoryEnabled));
            }
        }

        public string AlergenName
        {
            get => _alergenName;
            set
            {
                SetProperty(ref _alergenName, value);
                OnPropertyChanged(nameof(IsAddAlergenEnabled));
                OnPropertyChanged(nameof(IsUpdateAlergenEnabled));
            }
        }

        public bool HasLowStockItems
        {
            get => _hasLowStockItems;
            set => SetProperty(ref _hasLowStockItems, value);
        }

        public int LowStockCount
        {
            get => _lowStockCount;
            set => SetProperty(ref _lowStockCount, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public bool IsAllOrdersSelected
        {
            get => _isAllOrdersSelected;
            set
            {
                SetProperty(ref _isAllOrdersSelected, value);
                if (value && IsActiveOrdersSelected)
                {
                    IsActiveOrdersSelected = false;
                }

                if (value)
                {
                    LoadAllOrders();
                }
            }
        }

        public bool IsActiveOrdersSelected
        {
            get => _isActiveOrdersSelected;
            set
            {
                SetProperty(ref _isActiveOrdersSelected, value);
                if (value && IsAllOrdersSelected)
                {
                    IsAllOrdersSelected = false;
                }

                if (value)
                {
                    LoadActiveOrders();
                }
            }
        }

        public bool HasReportData
        {
            get => _hasReportData;
            set => SetProperty(ref _hasReportData, value);
        }

        public bool IsAddCategoryEnabled => !string.IsNullOrWhiteSpace(CategoryName);
        public bool IsUpdateCategoryEnabled => SelectedCategory != null && !string.IsNullOrWhiteSpace(CategoryName);
        public bool IsAddAlergenEnabled => !string.IsNullOrWhiteSpace(AlergenName);
        public bool IsUpdateAlergenEnabled => SelectedAlergen != null && !string.IsNullOrWhiteSpace(AlergenName);

        #endregion

        #region Commands

        public ICommand AddCategoryCommand { get; }
        public ICommand UpdateCategoryCommand { get; }
        public ICommand DeleteCategoryCommand { get; }
        public ICommand EditCategoryCommand { get; }

        public ICommand AddAlergenCommand { get; }
        public ICommand UpdateAlergenCommand { get; }
        public ICommand DeleteAlergenCommand { get; }
        public ICommand EditAlergenCommand { get; }

        public ICommand AddNewPreparatCommand { get; }
        public ICommand EditPreparatCommand { get; }
        public ICommand DeletePreparatCommand { get; }

        public ICommand ViewOrderDetailsCommand { get; }
        public ICommand UpdateOrderStatusCommand { get; }

        public ICommand ShowLowStockCommand { get; }

        public ICommand AddPreparatModeCommand { get; }
        public ICommand SavePreparatCommand { get; }
        public ICommand CancelPreparatEditCommand { get; }
        public ICommand AddPreparatAlergenCommand { get; }
        public ICommand RemovePreparatAlergenCommand { get; }
        public ICommand AddPreparatImagesCommand { get; }
        public ICommand RemovePreparatImageCommand { get; }

        #endregion


        private void EnterAddPreparatMode()
        {
            try
            {
                // Resetează formularul
                PreparatName = string.Empty;
                PreparatPrice = 0;
                PreparatQuantityPortie = 100;
                PreparatQuantityTotal = 0;
                PreparatSelectedCategory = Categories.FirstOrDefault();

                // Încarcă toți alergenii disponibili
                PreparatAvailableAlergens.Clear();
                PreparatSelectedAlergens.Clear();
                PreparatImages.Clear();

                foreach (var alergen in _alergenService.GetAllAlergens())
                {
                    PreparatAvailableAlergens.Add(alergen);
                }

                // Activează modul de adăugare
                IsAddPreparatMode = true;
                IsEditPreparatMode = false;
                _preparatBeingEdited = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la inițializarea formularului pentru preparat nou: {ex.Message}",
                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void SavePreparat()
        {
            System.Diagnostics.Debug.WriteLine("Metoda SavePreparat() a fost apelată la: " + DateTime.Now);

            try
            {
                if (!ValidatePreparat())
                    return;

                int preparatId;

                if (IsAddPreparatMode)
                {
                    // Adaugă preparat nou și salvează ID-ul returnat
                    preparatId = _preparatService.CreatePreparat(
                        PreparatName,
                        PreparatPrice,
                        PreparatQuantityPortie,
                        PreparatQuantityTotal,
                        PreparatSelectedCategory.CategoryId);

                    // Salvează imaginile folosind ID-ul preparatului nou creat
                    SaveImages(preparatId);

                    // Salvează alergenii pentru noul preparat
                    SaveAlergens(preparatId);

                    MessageBox.Show("Preparatul a fost adăugat cu succes!", "Succes",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (IsEditPreparatMode && _preparatBeingEdited != null)
                {
                    preparatId = _preparatBeingEdited.PreparatID;

                    // Actualizează preparat existent
                    _preparatService.UpdatePreparat(
                        preparatId,
                        PreparatName,
                        PreparatPrice,
                        PreparatQuantityPortie,
                        PreparatQuantityTotal,
                        PreparatSelectedCategory.CategoryId);

                    // Salvează imaginile pentru preparatul editat
                    SaveImages(preparatId);

                    // Salvează alergenii pentru preparatul editat
                    SaveAlergens(preparatId);

                    MessageBox.Show("Preparatul a fost actualizat cu succes!", "Succes",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }

                // Reîncarcă lista de preparate
                LoadPreparate();

                // Dezactivează modul de adăugare/editare
                IsAddPreparatMode = false;
                IsEditPreparatMode = false;
                _preparatBeingEdited = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la salvarea preparatului: {ex.Message}",
                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Metodă nouă pentru salvarea alergenilor
        private void SaveAlergens(int preparatId)
        {
            if (preparatId <= 0)
            {
                System.Diagnostics.Debug.WriteLine("ID preparat invalid în SaveAlergens");
                return;
            }

            try
            {
                // Dacă editam un preparat, ar trebui să ștergem asocierile vechi
                if (IsEditPreparatMode)
                {
                    // Șterge vechile asocieri de alergeni pentru acest preparat
                    var existingAlergens = _preparatBeingEdited?.PreparatAlergens?.Select(pa => pa.AlergenID) ?? Enumerable.Empty<int>();

                    foreach (var alergenId in existingAlergens)
                    {
                        _preparatService.RemovePreparatAlergen(preparatId, alergenId);
                    }
                }

                // Adaugă noile asocieri de alergeni
                foreach (var alergen in PreparatSelectedAlergens)
                {
                    System.Diagnostics.Debug.WriteLine($"Adăugare alergen {alergen.Name} (ID: {alergen.AlergenID}) la preparatul cu ID: {preparatId}");
                    _preparatService.AddPreparatAlergen(preparatId, alergen.AlergenID);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Eroare la salvarea alergenilor: {ex.Message}");
            }
        }
        private void SaveImages(int preparatId)
        {
            // Nu mai creăm un preparat nou, ci folosim ID-ul primit ca parametru
            if (preparatId <= 0)
            {
                System.Diagnostics.Debug.WriteLine("ID preparat invalid în SaveImages");
                return;
            }

            try
            {
                // Adăugăm imaginile pentru preparatul cu ID-ul specificat
                foreach (var image in PreparatImages)
                {
                    _preparatService.AddPreparatImage(preparatId, image.ImagePath);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Eroare la salvarea imaginilor: {ex.Message}");
            }
        }



        private void CancelPreparatEdit()
        {
            IsAddPreparatMode = false;
            IsEditPreparatMode = false;
            _preparatBeingEdited = null;
        }

        private void AddPreparatAlergen(object parameter)
        {
            if (parameter is Alergen alergen)
            {
                if (!PreparatSelectedAlergens.Contains(alergen))
                {
                    PreparatSelectedAlergens.Add(alergen);
                    PreparatAvailableAlergens.Remove(alergen);
                }
            }
        }

        private void RemovePreparatAlergen(object parameter)
        {
            if (parameter is Alergen alergen)
            {
                if (PreparatSelectedAlergens.Contains(alergen))
                {
                    PreparatSelectedAlergens.Remove(alergen);
                    PreparatAvailableAlergens.Add(alergen);
                }
            }
        }
  


        private void AddPreparatImages()
        {
            try
            {
                var dialog = new OpenFileDialog
                {
                    Filter = "Fișiere imagine (*.jpg;*.jpeg;*.png;*.gif)|*.jpg;*.jpeg;*.png;*.gif",
                    Multiselect = true,
                    Title = "Selectează imaginile pentru preparat"
                };

                if (dialog.ShowDialog() == true)
                {
                    // Ensure Resources directory exists
                    string resourcesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
                    Directory.CreateDirectory(resourcesFolder);

                    foreach (var fileName in dialog.FileNames)
                    {
                        // Generate unique filename
                        string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
                        string destinationPath = Path.Combine(resourcesFolder, uniqueFileName);

                        // Copy file to Resources folder
                        File.Copy(fileName, destinationPath, true);

                        // Use consistent path format with forward slashes
                        string relativePath = "Resources/" + uniqueFileName;

                        // Add image to list
                        PreparatImages.Add(new PreparatImageItem
                        {
                            ImagePath = relativePath,
                            FullPath = Path.GetFullPath(destinationPath) // Use absolute path for display
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la adăugarea imaginilor: {ex.Message}",
                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




        private void RemovePreparatImage(object parameter)
        {
            if (parameter is PreparatImageItem image)
            {
                PreparatImages.Remove(image);
            }
        }

        private bool ValidatePreparat()
        {
            // Validare nume
            if (string.IsNullOrWhiteSpace(PreparatName))
            {
                MessageBox.Show("Numele preparatului este obligatoriu.", "Validare",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Validare preț
            if (PreparatPrice < 0)
            {
                MessageBox.Show("Prețul trebuie să fie >= 0.", "Validare",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Validare cantitate porție
            if (PreparatQuantityPortie <= 0)
            {
                MessageBox.Show("Cantitatea per porție trebuie să fie > 0.", "Validare",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Validare cantitate totală
            if (PreparatQuantityTotal < 0)
            {
                MessageBox.Show("Cantitatea totală trebuie să fie >= 0.", "Validare",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Validare categorie
            if (PreparatSelectedCategory == null)
            {
                MessageBox.Show("Trebuie selectată o categorie.", "Validare",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
        

// Modify the SelectedReportType property to include data clearing
public ReportType SelectedReportType
        {
            get => _selectedReportType;
            set
            {
                if (SetProperty(ref _selectedReportType, value))
                {
                    // Clear report data first
                    ReportData.Clear();
                    HasReportData = false;

                    OnReportTypeSelected(value);

                    // Only call GenerateReport if value is not null
                    if (value != null)
                    {
                        GenerateSelectedReport();
                    }
                }
            }
        }
        // Metoda care se apelează când se face click pe butonul "Adaugă Preparat Nou"
        private void AddNewPreparat()
        {
            EnterAddPreparatMode();
        }

        // Metoda care se apelează când se face click pe butonul "Editează" pentru un preparat
        private void EditPreparat(Preparat preparat)
        {
            if (preparat == null)
                return;

            try
            {
                _preparatBeingEdited = preparat;

                // Populează formularul cu datele preparatului
                PreparatName = preparat.Name;
                PreparatPrice = preparat.Price;
                PreparatQuantityPortie = preparat.QuantityPortie;
                PreparatQuantityTotal = preparat.QuantityTotal;

                // Setează categoria
                PreparatSelectedCategory = Categories.FirstOrDefault(c => c.CategoryId == preparat.CategoryID);

                // Încarcă alergenii
                PreparatAvailableAlergens.Clear();
                PreparatSelectedAlergens.Clear();

                // Populează lista de alergeni disponibili și selectați
                var allAlergens = _alergenService.GetAllAlergens();
                var selectedAlergenIds = preparat.PreparatAlergens?.Select(pa => pa.AlergenID) ?? Enumerable.Empty<int>();

                foreach (var alergen in allAlergens)
                {
                    if (selectedAlergenIds.Contains(alergen.AlergenID))
                    {
                        PreparatSelectedAlergens.Add(alergen);
                    }
                    else
                    {
                        PreparatAvailableAlergens.Add(alergen);
                    }
                }

                // Încarcă imaginile
                PreparatImages.Clear();

                // Dacă există imagini asociate preparatului, le adăugăm în listă
                if (preparat.Fotos != null)
                {
                    foreach (var foto in preparat.Fotos)
                    {
                        string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, foto.ImagePath);

                        PreparatImages.Add(new PreparatImageItem
                        {
                            ImagePath = foto.ImagePath,
                            FullPath = fullPath
                        });
                    }
                }

                // Activează modul de editare
                IsAddPreparatMode = false;
                IsEditPreparatMode = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la încărcarea datelor preparatului: {ex.Message}",
                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        #region Constructor

        public AdminPanelViewModel(ICategoryService categoryService,
                                  IPreparatService preparatService,
                                  IAlergenService alergenService,
                                  IMenuService menuService,
                                  IOrderService orderService,
                                  IUserService userService)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _preparatService = preparatService ?? throw new ArgumentNullException(nameof(preparatService));
            _alergenService = alergenService ?? throw new ArgumentNullException(nameof(alergenService));
            _menuService = menuService ?? throw new ArgumentNullException(nameof(menuService));
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));

            // Initialize Collections
            MenuManagement = new MenuManagementViewModel(menuService, categoryService, preparatService);
            Categories = new ObservableCollection<Category>();
            Preparate = new ObservableCollection<Preparat>();
            Alergens = new ObservableCollection<Alergen>();
            Menus = new ObservableCollection<MenuViewModel>();
            Orders = new ObservableCollection<OrderViewModel>();
            ReportTypes = new ObservableCollection<ReportType>
            {
                new ReportType { Id = 1, Name = "Produse cu stoc redus" },
                new ReportType { Id = 2, Name = "Comenzi per zi" },
                new ReportType { Id = 3, Name = "Produse populare" },
                new ReportType { Id = 4, Name = "Venituri per categorie" }
            };
            ReportData = new ObservableCollection<object>();

            // Initialize Commands
            AddCategoryCommand = new RelayCommand(_ => AddCategory());
            UpdateCategoryCommand = new RelayCommand(_ => UpdateCategory());
            DeleteCategoryCommand = new RelayCommand(parameter => DeleteCategory(parameter as Category));
            EditCategoryCommand = new RelayCommand(parameter => EditCategory(parameter as Category));

            AddAlergenCommand = new RelayCommand(_ => AddAlergen());
            UpdateAlergenCommand = new RelayCommand(_ => UpdateAlergen());
            DeleteAlergenCommand = new RelayCommand(parameter => DeleteAlergen(parameter as Alergen));
            EditAlergenCommand = new RelayCommand(parameter => EditAlergen(parameter as Alergen));


            DeletePreparatCommand = new RelayCommand(parameter => DeletePreparat(parameter as Preparat));

            ViewOrderDetailsCommand = new RelayCommand(parameter => ViewOrderDetails(parameter as OrderViewModel));
            UpdateOrderStatusCommand = new RelayCommand(parameter => UpdateOrderStatus(parameter as OrderViewModel));

            ShowLowStockCommand = new RelayCommand(_ => ShowLowStockItems());
            GenerateReportCommand = new RelayCommand(_ => GenerateSelectedReport());

            // Initialize report dates
            ReportStartDate = DateTime.Now.AddMonths(-1);
            ReportEndDate = DateTime.Now;

            // Initialize report types if not already done
            if (ReportTypes == null)
            {
                ReportTypes = new ObservableCollection<ReportType>
    {
        new ReportType { Id = 1, Name = "Produse cu stoc redus" },
        new ReportType { Id = 2, Name = "Comenzi per zi" },
        new ReportType { Id = 3, Name = "Produse populare" },
        new ReportType { Id = 4, Name = "Venituri per categorie" }
    };
            }



            AddPreparatModeCommand = new RelayCommand(_ => EnterAddPreparatMode());
            SavePreparatCommand = new RelayCommand(_ => SavePreparat());
            CancelPreparatEditCommand = new RelayCommand(_ => CancelPreparatEdit());
            AddPreparatAlergenCommand = new RelayCommand(AddPreparatAlergen);
            RemovePreparatAlergenCommand = new RelayCommand(RemovePreparatAlergen);
            AddPreparatImagesCommand = new RelayCommand(_ => AddPreparatImages());
            RemovePreparatImageCommand = new RelayCommand(RemovePreparatImage);
            AddNewMenuCommand = new RelayCommand(_ => AddNewMenu());
            EditMenuCommand = new RelayCommand(param => EditMenu(param as MenuViewModel));
            DeleteMenuCommand = new RelayCommand(param => DeleteMenu(param as MenuViewModel));

            PreparatAvailableAlergens = new ObservableCollection<Alergen>();
            PreparatSelectedAlergens = new ObservableCollection<Alergen>();
            PreparatImages = new ObservableCollection<PreparatImageItem>();

            AddNewPreparatCommand = new RelayCommand(_ => AddNewPreparat());
            EditPreparatCommand = new RelayCommand(parameter => EditPreparat(parameter as Preparat));
        }

        #endregion





        // Modify the OnReportTypeSelected method in AdminPanelViewModel.cs
        private void OnReportTypeSelected(ReportType reportType)
        {
            if (reportType == null)
                return;

            // Create a completely new empty collection to force UI refresh
            ReportData = new ObservableCollection<object>();
            HasReportData = false;

            // Reset the report filter flags
            IsLowStockReportSelected = false;
            IsOrdersReportSelected = false;
            IsPopularProductsReportSelected = false;

            // Set the appropriate flag based on the selected report type
            switch (reportType.Id)
            {
                case 1: // Low stock products
                    IsLowStockReportSelected = true;
                    ShowReportFilters = true;
                    break;
                case 2: // Orders per day
                    IsOrdersReportSelected = true;
                    ShowReportFilters = true;
                    break;
                case 3: // Popular products
                    IsPopularProductsReportSelected = true;
                    ShowReportFilters = true;
                    break;
                case 4: // Revenue per category
                    ShowReportFilters = false;
                    break;
                default:
                    ShowReportFilters = false;
                    break;
            }
        }


        private async void GenerateSelectedReport()
        {
            if (SelectedReportType == null)
                return;

            try
            {
                IsReportLoading = true;

                // Create a completely new collection instead of just clearing the existing one
                ReportData = new ObservableCollection<object>();
                HasReportData = false;

                // Create a temporary collection to hold the report results
                var tempResults = new ObservableCollection<object>();

                switch (SelectedReportType.Id)
                {
                    case 1: // Low stock products
                        await GenerateLowStockReportFromDatabase(tempResults);
                        break;
                    case 2: // Orders per day
                        await GenerateOrdersPerDayReportFromDatabase(tempResults);
                        break;
                    case 3: // Popular products
                        await GeneratePopularProductsReportFromDatabase(tempResults);
                        break;
                    case 4: // Revenue per category
                        await GenerateRevenuePerCategoryReportFromDatabase(tempResults);
                        break;
                }

                // Set the ReportData property to the new collection
                ReportData = tempResults;
                HasReportData = ReportData.Count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la generarea raportului: {ex.Message}", "Eroare",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                ReportData = new ObservableCollection<object>();
                HasReportData = false;
            }
            finally
            {
                IsReportLoading = false;
            }
        }

        private async Task GenerateLowStockReportFromDatabase(ObservableCollection<object> results)
        {
            await Task.Run(() =>
            {
                using (var connection = new SqlConnection(GetConnectionString()))
                {
                    connection.Open();
                    using (var command = new SqlCommand("spReportLowStockProducts", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@LowStockThreshold", LowStockThreshold);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var item = new LowStockItemViewModel
                                {
                                    ProductName = reader["ProductName"].ToString(),
                                    CurrentStock = Convert.ToInt32(reader["CurrentStock"]),
                                    Category = reader["Category"].ToString(),
                                    Status = reader["Status"].ToString()
                                };

                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    results.Add(item);
                                });
                            }
                        }
                    }
                }
            });
        }

        private async Task GenerateOrdersPerDayReportFromDatabase(ObservableCollection<object> results)
        {
            await Task.Run(() =>
            {
                using (var connection = new SqlConnection(GetConnectionString()))
                {
                    connection.Open();
                    using (var command = new SqlCommand("spReportOrdersPerDay", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@StartDate", ReportStartDate);
                        command.Parameters.AddWithValue("@EndDate", ReportEndDate);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var item = new OrdersPerDayViewModel
                                {
                                    Date = Convert.ToDateTime(reader["Date"]),
                                    DateFormatted = reader["DateFormatted"].ToString(),
                                    OrderCount = Convert.ToInt32(reader["OrderCount"]),
                                    TotalValue = Convert.ToDecimal(reader["TotalValue"]),
                                    TotalValueFormatted = reader["TotalValueFormatted"].ToString()
                                };

                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    results.Add(item);
                                });
                            }
                        }
                    }
                }
            });
        }

        private async Task GeneratePopularProductsReportFromDatabase(ObservableCollection<object> results)
        {
            await Task.Run(() =>
            {
                using (var connection = new SqlConnection(GetConnectionString()))
                {
                    connection.Open();
                    using (var command = new SqlCommand("spReportPopularProducts", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@TopCount", TopProductsCount);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var item = new PopularProductViewModel
                                {
                                    ProductName = reader["ProductName"].ToString(),
                                    OrderCount = Convert.ToInt32(reader["OrderCount"]),
                                    TotalQuantity = Convert.ToInt32(reader["TotalQuantity"]),
                                    Revenue = Convert.ToDecimal(reader["Revenue"]),
                                    RevenueFormatted = reader["RevenueFormatted"].ToString()
                                };

                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    results.Add(item);
                                });
                            }
                        }
                    }
                }
            });
        }

        private async Task GenerateRevenuePerCategoryReportFromDatabase(ObservableCollection<object> results)
        {
            await Task.Run(() =>
            {
                using (var connection = new SqlConnection(GetConnectionString()))
                {
                    connection.Open();
                    using (var command = new SqlCommand("spReportRevenuePerCategory", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var item = new RevenuePerCategoryViewModel
                                {
                                    CategoryName = reader["CategoryName"].ToString(),
                                    OrderCount = Convert.ToInt32(reader["OrderCount"]),
                                    Revenue = Convert.ToDecimal(reader["Revenue"]),
                                    RevenueFormatted = reader["RevenueFormatted"].ToString(),
                                    PercentageOfTotal = Convert.ToDecimal(reader["PercentageOfTotal"])
                                };

                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    results.Add(item);
                                });
                            }
                        }
                    }
                }
            });
        }


        private int _selectedTabIndex;
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set => SetProperty(ref _selectedTabIndex, value);
        }

        private void ShowLowStockItems()
        {
            try
            {
                // First, switch to the Reports tab (assuming it's the 4th tab, index 3)
                SelectedTabIndex = 5; // Adjust this index if the Reports tab is at a different position

                // Then select the "Produse cu stoc redus" report and generate it
                SelectedReportType = ReportTypes.FirstOrDefault(r => r.Id == 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la afișarea produselor cu stoc redus: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private string GetConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["RestaurantDb"].ConnectionString;
        }

        private void AddNewMenu()
        {
            MenuManagement.Initialize(); // Inițializăm formularul pentru un meniu nou
            MenuManagement.IsAddMode = true;

            // Adăugăm handler pentru evenimentul CancelRequested
            MenuManagement.CancelRequested += MenuManagement_CancelRequested;
        }

        private void EditMenu(MenuViewModel menuViewModel)
        {
            if (menuViewModel == null)
            {
                System.Diagnostics.Debug.WriteLine("AdminPanelViewModel: EditMenu called with null menuViewModel");
                return;
            }

            try
            {
                System.Diagnostics.Debug.WriteLine($"AdminPanelViewModel: EditMenu called for menu ID: {menuViewModel.MenuID}, Name: {menuViewModel.Name}");

                // Obține meniul original din baza de date pentru a avea toate datele complete
                var menuId = menuViewModel.MenuID;
                var menu = _menuService.GetMenuById(menuId);

                if (menu == null)
                {
                    System.Diagnostics.Debug.WriteLine($"AdminPanelViewModel: Failed to load menu with ID {menuId} from database");
                    MessageBox.Show("Nu s-a putut încărca meniul pentru editare.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"AdminPanelViewModel: Menu loaded successfully from database: {menu.Name}");
                System.Diagnostics.Debug.WriteLine($"AdminPanelViewModel: Category: {menu.Category?.Name ?? "null"}");
                System.Diagnostics.Debug.WriteLine($"AdminPanelViewModel: Menu items count: {menu.MenuPreparate?.Count ?? 0}");

                // Eliminăm orice abonament existent pentru a evita dublarea lor
                MenuManagement.CancelRequested -= MenuManagement_CancelRequested;

                // Adăugăm noul handler
                MenuManagement.CancelRequested += MenuManagement_CancelRequested;

                // Inițializăm formularul de editare
                MenuManagement.Initialize(menu);
                MenuManagement.IsEditMode = true;

                System.Diagnostics.Debug.WriteLine("AdminPanelViewModel: Menu edit form initialized");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"AdminPanelViewModel: Error in EditMenu - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"AdminPanelViewModel: {ex.StackTrace}");
                MessageBox.Show($"Eroare la încărcarea meniului pentru editare: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuManagement_CancelRequested(object sender, EventArgs e)
        {
            // Eliminăm handler-ul pentru a evita multiple subscriptions
            MenuManagement.CancelRequested -= MenuManagement_CancelRequested;

            // Resetăm proprietatea pentru a reveni la pagina cu lista de meniuri
            MenuManagement.IsAddMode = false;
            MenuManagement.IsEditMode = false;

            // Reîncărcăm lista de meniuri pentru a ne asigura că este actualizată
            LoadMenus();
        }
        private void DeleteMenu(MenuViewModel menuViewModel)
        {
            if (menuViewModel == null)
                return;

            try
            {
                var result = MessageBox.Show(
                    $"Ești sigur că vrei să ștergi meniul '{menuViewModel.Name}'?",
                    "Confirmare ștergere",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    var menuId = menuViewModel.MenuID; // Sau menuViewModel.Menu.MenuID
                    _menuService.DeleteMenu(menuId);
                    LoadMenus(); // Reîncărcăm lista de meniuri
                    MessageBox.Show("Meniul a fost șters cu succes!", "Succes",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la ștergerea meniului: {ex.Message}", "Eroare",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #region Loading Methods

        public async Task LoadInitialDataAsync()
        {
            IsLoading = true;

            try
            {
                await Task.Run(() =>
                {
                    LoadCategories();
                    LoadPreparate();
                    LoadAlergens();
                    LoadMenus();
                    LoadOrders();
                    CheckLowStockItems();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la încărcarea datelor: {ex.Message}", "Eroare",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void LoadCategories()
        {
            var categories = _categoryService.GetAllCategories();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Categories.Clear();
                foreach (var category in categories)
                {
                    Categories.Add(category);
                }
            });
        }

        private void LoadPreparate()
        {
            var allPreparate = new List<Preparat>();

            foreach (var category in Categories)
            {
                var preparateInCategory = _preparatService.GetPreparateByCategory(category.CategoryId);
                allPreparate.AddRange(preparateInCategory);
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                Preparate.Clear();
                foreach (var preparat in allPreparate)
                {
                    Preparate.Add(preparat);
                }
            });
        }

        private void LoadAlergens()
        {
            var alergens = _alergenService.GetAllAlergens();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Alergens.Clear();
                foreach (var alergen in alergens)
                {
                    Alergens.Add(alergen);
                }
            });
        }

        private void LoadMenus()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Începe încărcarea meniurilor...");

                var menus = _menuService.GetAllMenus(); // Sau GetAllMenusWithCategories() dacă ați implementat-o
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Menus.Clear();
                    foreach (var menu in menus)
                    {
                        var menuVm = new MenuViewModel();
                        menuVm.Menu = menu;

                        // Calcularea prețului pentru meniu (cu reducerea din configurare)
                        decimal calculatedPrice = _menuService.CalculateMenuPrice(menu.MenuID);
                        menuVm.CalculatedPrice = calculatedPrice;

                        Menus.Add(menuVm);
                    }

                    System.Diagnostics.Debug.WriteLine($"Meniuri încărcate: {Menus.Count}");
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Eroare la încărcarea meniurilor: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                MessageBox.Show($"Eroare la încărcarea meniurilor: {ex.Message}", "Eroare",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void LoadOrders()
        {
            if (IsAllOrdersSelected)
            {
                LoadAllOrders();
            }
            else
            {
                LoadActiveOrders();
            }
        }

        private void LoadAllOrders()
        {
            var orders = _orderService.GetAllOrders();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Orders.Clear();
                foreach (var order in orders)
                {
                    Orders.Add(MapOrderToViewModel(order));
                }
            });
        }

        private void LoadActiveOrders()
        {
            var orders = _orderService.GetActiveOrders();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Orders.Clear();
                foreach (var order in orders)
                {
                    Orders.Add(MapOrderToViewModel(order));
                }
            });
        }

        private void CheckLowStockItems()
        {
            // Simulare verificare produse cu stoc redus
            // În mod normal, se folosește un parametru de configurare pentru pragul minim
            const int lowStockThreshold = 5;

            var lowStockItems = Preparate.Where(p => p.QuantityTotal <= lowStockThreshold).ToList();

            Application.Current.Dispatcher.Invoke(() =>
            {
                HasLowStockItems = lowStockItems.Count > 0;
                LowStockCount = lowStockItems.Count;
            });
        }

        #endregion

        #region Category Operations

        private void AddCategory()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(CategoryName))
                    return;

                _categoryService.CreateCategory(CategoryName);
                LoadCategories();
                CategoryName = string.Empty;
                MessageBox.Show("Categoria a fost adăugată cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la adăugarea categoriei: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateCategory()
        {
            try
            {
                if (SelectedCategory == null || string.IsNullOrWhiteSpace(CategoryName))
                    return;

                _categoryService.UpdateCategory(SelectedCategory.CategoryId, CategoryName);
                LoadCategories();
                MessageBox.Show("Categoria a fost actualizată cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la actualizarea categoriei: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteCategory(Category category)
        {
            try
            {
                if (category == null)
                    return;

                var result = MessageBox.Show(
                    $"Ești sigur că vrei să ștergi categoria '{category.Name}'? Această acțiune va șterge toate preparatele și meniurile din această categorie!",
                    "Confirmare ștergere",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _categoryService.DeleteCategory(category.CategoryId);
                    LoadCategories();
                    LoadPreparate();
                    LoadMenus();
                    MessageBox.Show("Categoria a fost ștearsă cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la ștergerea categoriei: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditCategory(Category category)
        {
            if (category == null)
                return;

            SelectedCategory = category;
            CategoryName = category.Name;
        }

        #endregion

        #region Alergen Operations

        private void AddAlergen()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(AlergenName))
                    return;

                _alergenService.CreateAlergen(AlergenName);
                LoadAlergens();
                AlergenName = string.Empty;
                MessageBox.Show("Alergenul a fost adăugat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la adăugarea alergenului: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateAlergen()
        {
            try
            {
                if (SelectedAlergen == null || string.IsNullOrWhiteSpace(AlergenName))
                    return;

                _alergenService.UpdateAlergen(SelectedAlergen.AlergenID, AlergenName);
                LoadAlergens();
                MessageBox.Show("Alergenul a fost actualizat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la actualizarea alergenului: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteAlergen(Alergen alergen)
        {
            try
            {
                if (alergen == null)
                    return;

                var result = MessageBox.Show(
                    $"Ești sigur că vrei să ștergi alergenul '{alergen.Name}'?",
                    "Confirmare ștergere",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _alergenService.DeleteAlergen(alergen.AlergenID);
                    LoadAlergens();
                    MessageBox.Show("Alergenul a fost șters cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la ștergerea alergenului: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditAlergen(Alergen alergen)
        {
            if (alergen == null)
                return;

            SelectedAlergen = alergen;
            AlergenName = alergen.Name;
        }

        #endregion

        #region Preparat Operations


        private void DeletePreparat(Preparat preparat)
        {
            try
            {
                if (preparat == null)
                    return;

                var result = MessageBox.Show(
                    $"Ești sigur că vrei să ștergi preparatul '{preparat.Name}'?",
                    "Confirmare ștergere",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _preparatService.DeletePreparat(preparat.PreparatID);
                    LoadPreparate();
                    MessageBox.Show("Preparatul a fost șters cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la ștergerea preparatului: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion


        #region Order Operations

        private void ViewOrderDetails(OrderViewModel order)
        {
            if (order == null)
                return;

            // În producție, aici s-ar deschide un dialog/view pentru vizualizarea detaliilor comenzii
            MessageBox.Show($"Detalii comandă: {order.OrderCode}\nClient: {order.ClientName}\nTotal: {order.TotalCostFormatted}",
                "Detalii Comandă", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void UpdateOrderStatus(OrderViewModel order)
        {
            if (order == null)
                return;

            try
            {
                // În producție, aici s-ar deschide un dialog pentru selectarea noii stări a comenzii
                var nextStatus = GetNextOrderStatus(order.Status);

                var result = MessageBox.Show(
                    $"Dorești să actualizezi starea comenzii {order.OrderCode} de la '{GetStatusName(order.Status)}' la '{GetStatusName(nextStatus)}'?",
                    "Confirmare actualizare stare comandă",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _orderService.UpdateOrderStatus(order.OrderID, nextStatus);
                    LoadOrders();
                    MessageBox.Show("Starea comenzii a fost actualizată cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la actualizarea stării comenzii: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private OrderStatus GetNextOrderStatus(OrderStatus currentStatus)
        {
            return currentStatus switch
            {
                OrderStatus.Registered => OrderStatus.InPreparation,
                OrderStatus.InPreparation => OrderStatus.OutforDelivery,
                OrderStatus.OutforDelivery => OrderStatus.Delievered,
                _ => OrderStatus.Canceled // Fallback, nu ar trebui să se ajungă aici
            };
        }

        private string GetStatusName(OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Registered => "Înregistrată",
                OrderStatus.InPreparation => "În pregătire",
                OrderStatus.OutforDelivery => "În livrare",
                OrderStatus.Delievered => "Livrată",
                OrderStatus.Canceled => "Anulată",
                _ => "Necunoscut"
            };
        }

        #endregion

        #region Report Operations

        
        

        private void GenerateLowStockReport()
        {
            const int lowStockThreshold = 5;

            var lowStockItems = Preparate
                .Where(p => p.QuantityTotal <= lowStockThreshold)
                .Select(p => new LowStockItemViewModel
                {
                    ProductName = p.Name,
                    CurrentStock = p.QuantityTotal,
                    Category = p.Category?.Name ?? "Necunoscut",
                    Status = p.QuantityTotal == 0 ? "Epuizat" : "Nivel scăzut"
                })
                .OrderBy(item => item.CurrentStock)
                .ToList();

            foreach (var item in lowStockItems)
            {
                ReportData.Add(item);
            }
        }

        private void GenerateOrdersPerDayReport()
        {
            // Grupează comenzile după zi și calculează numărul și valoarea totală
            var ordersPerDay = _orderService.GetAllOrders()
                .GroupBy(o => o.OrderDateTime.Date)
                .Select(g => new OrdersPerDayViewModel
                {
                    Date = g.Key,
                    DateFormatted = g.Key.ToString("dd.MM.yyyy"),
                    OrderCount = g.Count(),
                    TotalValue = g.Sum(o => o.TotalCost),
                    TotalValueFormatted = $"{g.Sum(o => o.TotalCost):N2} Lei"
                })
                .OrderByDescending(item => item.Date)
                .ToList();

            foreach (var item in ordersPerDay)
            {
                ReportData.Add(item);
            }
        }

        private void GeneratePopularProductsReport()
        {
            // Simulare raport de popularitate a produselor (în practică s-ar face o interogare complexă pe baza comenzilor)
            var popularProducts = new List<PopularProductViewModel>
            {
                new PopularProductViewModel { ProductName = "Supă cremă de ciuperci", OrderCount = 45, TotalQuantity = 45, Revenue = 1350.00m, RevenueFormatted = "1,350.00 Lei" },
                new PopularProductViewModel { ProductName = "Pizza Margherita", OrderCount = 38, TotalQuantity = 38, Revenue = 1520.00m, RevenueFormatted = "1,520.00 Lei" },
                new PopularProductViewModel { ProductName = "Paste Carbonara", OrderCount = 32, TotalQuantity = 32, Revenue = 1280.00m, RevenueFormatted = "1,280.00 Lei" },
                new PopularProductViewModel { ProductName = "Limonadă", OrderCount = 30, TotalQuantity = 30, Revenue = 450.00m, RevenueFormatted = "450.00 Lei" },
                new PopularProductViewModel { ProductName = "Salată Caesar", OrderCount = 25, TotalQuantity = 25, Revenue = 875.00m, RevenueFormatted = "875.00 Lei" }
            };

            foreach (var item in popularProducts)
            {
                ReportData.Add(item);
            }
        }

        private void GenerateRevenuePerCategoryReport()
        {
            // Simulare raport de venituri pe categorii (în practică s-ar face o interogare complexă)
            var revenuePerCategory = new List<RevenuePerCategoryViewModel>
            {
                new RevenuePerCategoryViewModel { CategoryName = "Fel principal", OrderCount = 150, Revenue = 12000.00m, RevenueFormatted = "12,000.00 Lei", PercentageOfTotal = 40.0m },
                new RevenuePerCategoryViewModel { CategoryName = "Aperitive", OrderCount = 120, Revenue = 6000.00m, RevenueFormatted = "6,000.00 Lei", PercentageOfTotal = 20.0m },
                new RevenuePerCategoryViewModel { CategoryName = "Desert", OrderCount = 100, Revenue = 3000.00m, RevenueFormatted = "3,000.00 Lei", PercentageOfTotal = 10.0m },
                new RevenuePerCategoryViewModel { CategoryName = "Băuturi", OrderCount = 200, Revenue = 4500.00m, RevenueFormatted = "4,500.00 Lei", PercentageOfTotal = 15.0m },
                new RevenuePerCategoryViewModel { CategoryName = "Meniuri", OrderCount = 80, Revenue = 4500.00m, RevenueFormatted = "4,500.00 Lei", PercentageOfTotal = 15.0m }
            };

            foreach (var item in revenuePerCategory)
            {
                ReportData.Add(item);
            }
        }

        #endregion

        #region Helper Methods
        private string GetStatusText(OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Registered => "Înregistrată",
                OrderStatus.InPreparation => "În pregătire",
                OrderStatus.OutforDelivery => "În livrare",
                OrderStatus.Delievered => "Livrată",
                OrderStatus.Canceled => "Anulată",
                _ => "Necunoscut"
            };
        }

        private string GetStatusBackgroundColor(OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Registered => "#2196F3", // albastru
                OrderStatus.InPreparation => "#FF9800", // portocaliu
                OrderStatus.OutforDelivery => "#673AB7", // mov
                OrderStatus.Delievered => "#4CAF50", // verde
                OrderStatus.Canceled => "#F44336", // roșu
                _ => "#9E9E9E" // gri
            };
        }
        private OrderViewModel MapOrderToViewModel(Domain.Entities.Order order)
        {
            var user = _userService.GetUserById(order.UserID);

            return new OrderViewModel
            {
                OrderID = order.OrderID,
                OrderCode = order.OrderCode,
                OrderDateTime = order.OrderDateTime,
                OrderDateFormatted = order.OrderDateTime.ToString("dd.MM.yyyy HH:mm"),
                ClientName = user != null ? $"{user.FirstName} {user.LastName}" : "Necunoscut",
                Status = order.Status,
                StatusText = GetStatusText(order.Status),
                StatusBackground = GetStatusBackgroundColor(order.Status),
                TotalCost = order.TotalCost,
                TotalCostFormatted = $"{order.TotalCost:N2} Lei",
                CanUpdateStatus = order.Status != OrderStatus.Delievered && order.Status != OrderStatus.Canceled
            };
        }

        #endregion
    }

   

   

  

    
}