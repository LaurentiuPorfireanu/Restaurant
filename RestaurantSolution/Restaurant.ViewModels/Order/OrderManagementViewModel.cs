using Restaurant.Domain.Entities;
using Restaurant.Domain.Enums;
using Restaurant.Services.Interfaces;
using Restaurant.ViewModels.Base;
using Restaurant.ViewModels.Commands;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows;
using System;
using System.Configuration;
using Restaurant.ViewModels.Menu.Extras;
using Restaurant.ViewModels.Order.Extras;

public class OrderManagementViewModel : ViewModelBase
{
    private readonly IOrderService _orderService;
    private readonly IPreparatService _preparatService;
    private readonly IMenuService _menuService;
    private readonly ICategoryService _categoryService;
    private readonly User _currentUser;

    private bool _isAddMode;
    private bool _isLoading;
    private string _deliveryAddress;
    private ObservableCollection<CategoryViewModel> _categories;
    private ObservableCollection<CartItemViewModel> _cartItems;
    private decimal _subtotal;
    private decimal _discount;
    private decimal _deliveryCost;
    private decimal _totalCost;

    public ICommand AddToCartCommand { get; }
    public ICommand RemoveFromCartCommand { get; }
    public ICommand IncrementQuantityCommand { get; }
    public ICommand DecrementQuantityCommand { get; }
    public ICommand PlaceOrderCommand { get; }
    public ICommand CancelOrderCommand { get; }

    public event EventHandler OrderPlaced;
    public event EventHandler CancelRequested;

    // Properties
    public bool IsAddMode
    {
        get => _isAddMode;
        set => SetProperty(ref _isAddMode, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public string DeliveryAddress
    {
        get => _deliveryAddress;
        set => SetProperty(ref _deliveryAddress, value);
    }

    public ObservableCollection<CategoryViewModel> Categories
    {
        get => _categories;
        set => SetProperty(ref _categories, value);
    }

    public ObservableCollection<CartItemViewModel> CartItems
    {
        get => _cartItems;
        set => SetProperty(ref _cartItems, value);
    }

    public decimal Subtotal
    {
        get => _subtotal;
        set
        {
            SetProperty(ref _subtotal, value);
            RecalculateTotal();
            OnPropertyChanged(nameof(SubtotalFormatted));
        }
    }

    public string SubtotalFormatted => $"{Subtotal:N2} Lei";

    public decimal Discount
    {
        get => _discount;
        set
        {
            SetProperty(ref _discount, value);
            RecalculateTotal();
            OnPropertyChanged(nameof(DiscountFormatted));
            OnPropertyChanged(nameof(HasDiscount));
        }
    }

    public string DiscountFormatted => $"-{Discount:N2} Lei";

    public bool HasDiscount => Discount > 0;

    public decimal DeliveryCost
    {
        get => _deliveryCost;
        set
        {
            SetProperty(ref _deliveryCost, value);
            RecalculateTotal();
            OnPropertyChanged(nameof(DeliveryCostFormatted));
            OnPropertyChanged(nameof(HasDeliveryCost));
        }
    }

    public string DeliveryCostFormatted => $"{DeliveryCost:N2} Lei";

    public bool HasDeliveryCost => DeliveryCost > 0;

    public decimal TotalCost
    {
        get => _totalCost;
        set
        {
            SetProperty(ref _totalCost, value);
            OnPropertyChanged(nameof(TotalCostFormatted));
        }
    }

    public string TotalCostFormatted => $"{TotalCost:N2} Lei";

    public DateTime? EstimatedDelivery { get; private set; }

    public string EstimatedDeliveryFormatted => EstimatedDelivery?.ToString("dd.MM.yyyy HH:mm") ?? "Nespecificat";

    
  


    public OrderManagementViewModel(
        IOrderService orderService,
        IPreparatService preparatService,
        IMenuService menuService,
        User currentUser,
        ICategoryService categoryService)
    {
        _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        _preparatService = preparatService ?? throw new ArgumentNullException(nameof(preparatService));
        _menuService = menuService ?? throw new ArgumentNullException(nameof(menuService));
        _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));

        Categories = new ObservableCollection<CategoryViewModel>();
        CartItems = new ObservableCollection<CartItemViewModel>();

        AddToCartCommand = new RelayCommand(param => AddToCart(param));
        RemoveFromCartCommand = new RelayCommand(param => RemoveFromCart(param as CartItemViewModel));
        IncrementQuantityCommand = new RelayCommand(param => IncrementQuantity(param as CartItemViewModel));
        DecrementQuantityCommand = new RelayCommand(param => DecrementQuantity(param as CartItemViewModel));
        PlaceOrderCommand = new RelayCommand(_ => PlaceOrder(), _ => CanPlaceOrder());
        CancelOrderCommand = new RelayCommand(_ => CancelOrder());

       
        DeliveryAddress = _currentUser.Address;

        
        EstimatedDelivery = DateTime.Now.AddHours(1);
    }


    public void Initialize()
    {
        IsLoading = true;

        try
        {
           
            LoadCategories();

           
            CartItems.Clear();

           
            RecalculateSubtotal();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error initializing order: {ex.Message}");
            MessageBox.Show($"Error initializing order: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void LoadCategories()
    {
        Categories.Clear();

        var categories = _categoryService.GetAllCategories();

        foreach (var category in categories)
        {
            var categoryVm = new CategoryViewModel
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Preparate = new ObservableCollection<PreparatViewModel>(),
                Menus = new ObservableCollection<MenuItemViewModel>()
            };

            
            var preparate = _preparatService.GetPreparateByCategory(category.CategoryId);
            foreach (var preparat in preparate)
            {
                if (preparat.QuantityTotal > 0) 
                {
                    categoryVm.Preparate.Add(new PreparatViewModel
                    {
                        PreparatId = preparat.PreparatID,
                        Name = preparat.Name,
                        Price = preparat.Price,
                        PriceFormatted = $"{preparat.Price:N2} Lei",
                        IsAvailable = true,
                        Preparat = preparat
                    });
                }
            }

           
            var menus = _menuService.GetMenusByCategory(category.CategoryId);
            foreach (var menu in menus)
            {
                bool isAvailable = IsMenuAvailable(menu);
                if (isAvailable)
                {
                    var price = CalculateMenuPrice(menu);

                    categoryVm.Menus.Add(new MenuItemViewModel
                    {
                        MenuId = menu.MenuID,
                        Name = menu.Name,
                        PriceFormatted = $"{price:N2} Lei",
                        IsAvailable = true,
                        Menu = menu,
                        Price = price
                    });
                }
            }

            
            if (categoryVm.Preparate.Count > 0 || categoryVm.Menus.Count > 0)
            {
                categoryVm.HasPreparate = categoryVm.Preparate.Count > 0;
                categoryVm.HasMenus = categoryVm.Menus.Count > 0;
                Categories.Add(categoryVm);
            }
        }
    }

    private bool IsMenuAvailable(Restaurant.Domain.Entities.Menu menu)
    {
        if (menu.MenuPreparate == null || !menu.MenuPreparate.Any())
            return false;

        return menu.MenuPreparate.All(mp => (mp.Preparat?.QuantityTotal ?? 0) > 0);
    }

    private decimal CalculateMenuPrice(Restaurant.Domain.Entities.Menu menu)
    {
        decimal discountPercentage = 10; 

        
        string configDiscount = ConfigurationManager.AppSettings["MenuDiscountPercentage"];
        if (!string.IsNullOrEmpty(configDiscount) && decimal.TryParse(configDiscount, out decimal parsedDiscount))
        {
            discountPercentage = parsedDiscount;
        }

        decimal totalPrice = 0;

        if (menu.MenuPreparate != null)
        {
            totalPrice = menu.MenuPreparate.Sum(mp => mp.Preparat?.Price ?? 0);
        }

        return Math.Round(totalPrice * (1 - discountPercentage / 100), 2);
    }

    public void AddToCart(object parameter)
    {
        if (parameter is PreparatViewModel preparatVm && preparatVm.Preparat != null)
        {
            var existingItem = CartItems.FirstOrDefault(i =>
                i.ItemType == CartItemType.Dish && i.ItemId == preparatVm.PreparatId);

            // Calculate how many more portions we can add based on available stock
            int currentOrderQuantity = existingItem?.Quantity ?? 0;
            int maxAvailablePortions = preparatVm.Preparat.QuantityTotal / preparatVm.Preparat.QuantityPortie;

            if (currentOrderQuantity >= maxAvailablePortions)
            {
                MessageBox.Show($"Nu mai există suficiente porții disponibile pentru '{preparatVm.Name}'.\nStoc disponibil: {maxAvailablePortions} porții.",
                    "Stoc insuficient", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (existingItem != null)
            {
                // Increment quantity
                existingItem.Quantity++;
            }
            else
            {
                // Add new item
                CartItems.Add(new CartItemViewModel
                {
                    ItemId = preparatVm.PreparatId,
                    ItemType = CartItemType.Dish,
                    Name = preparatVm.Name,
                    UnitPrice = preparatVm.Price,
                    Quantity = 1,
                    TotalPrice = preparatVm.Price,
                    UnitPriceFormatted = preparatVm.PriceFormatted,
                    TotalPriceFormatted = preparatVm.PriceFormatted,
                    Preparat = preparatVm.Preparat,
                    MaxAvailableQuantity = maxAvailablePortions
                });
            }
        }
        else if (parameter is MenuItemViewModel menuVm && menuVm.Menu != null)
        {
            // For menus, we need to check each ingredient
            bool insufficientStock = false;
            string insufficientItem = "";
            int availableMenuCount = int.MaxValue;

            // Check each preparat in the menu
            if (menuVm.Menu.MenuPreparate != null)
            {
                foreach (var menuPreparat in menuVm.Menu.MenuPreparate)
                {
                    if (menuPreparat.Preparat == null)
                        continue;

                    // Calculate how many menu portions we can make from this ingredient
                    int menuPortionsFromThisIngredient = menuPreparat.Preparat.QuantityTotal / menuPreparat.QuantityMenuPortie;

                    if (menuPortionsFromThisIngredient < availableMenuCount)
                    {
                        availableMenuCount = menuPortionsFromThisIngredient;
                        if (availableMenuCount == 0)
                        {
                            insufficientStock = true;
                            insufficientItem = menuPreparat.Preparat.Name;
                            break;
                        }
                    }
                }
            }

            // Check if menu already in cart
            var existingItem = CartItems.FirstOrDefault(i =>
                i.ItemType == CartItemType.Menu && i.ItemId == menuVm.MenuId);

            int currentOrderQuantity = existingItem?.Quantity ?? 0;

            if (insufficientStock || currentOrderQuantity >= availableMenuCount)
            {
                MessageBox.Show($"Nu mai există suficiente ingrediente pentru '{menuVm.Name}'.\n" +
                                (insufficientItem != "" ? $"Ingredient lipsă: {insufficientItem}" : $"Cantitate maximă disponibilă: {availableMenuCount}"),
                                "Stoc insuficient", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (existingItem != null)
            {
                // Increment quantity
                existingItem.Quantity++;
            }
            else
            {
                // Add new item
                CartItems.Add(new CartItemViewModel
                {
                    ItemId = menuVm.MenuId,
                    ItemType = CartItemType.Menu,
                    Name = menuVm.Name,
                    UnitPrice = menuVm.Price,
                    Quantity = 1,
                    TotalPrice = menuVm.Price,
                    UnitPriceFormatted = $"{menuVm.Price:N2} Lei",
                    TotalPriceFormatted = $"{menuVm.Price:N2} Lei",
                    Menu = menuVm.Menu,
                    MaxAvailableQuantity = availableMenuCount
                });
            }
        }

        // Recalculate totals
        RecalculateSubtotal();
    }

    private void IncrementQuantity(CartItemViewModel item)
    {
        if (item != null)
        {
            // Check if we're trying to exceed the available quantity
            if (item.Quantity >= item.MaxAvailableQuantity)
            {
                MessageBox.Show($"Nu mai există suficient stoc pentru '{item.Name}'.\nCantitate maximă disponibilă: {item.MaxAvailableQuantity}.",
                    "Stoc insuficient", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            item.Quantity++;
            item.TotalPrice = item.UnitPrice * item.Quantity;
            item.TotalPriceFormatted = $"{item.TotalPrice:N2} Lei";

            RecalculateSubtotal();
        }
    }





    private void RemoveFromCart(CartItemViewModel item)
    {
        if (item != null)
        {
            CartItems.Remove(item);
            RecalculateSubtotal();
        }
    }

    

    private void DecrementQuantity(CartItemViewModel item)
    {
        if (item != null && item.Quantity > 1)
        {
            item.Quantity--;
            item.TotalPrice = item.UnitPrice * item.Quantity;
            item.TotalPriceFormatted = $"{item.TotalPrice:N2} Lei";

            RecalculateSubtotal();
        }
        else if (item != null && item.Quantity == 1)
        {
            // Remove item if quantity drops to 0
            RemoveFromCart(item);
        }
    }

    private void RecalculateSubtotal()
    {
        Subtotal = CartItems.Sum(item => item.TotalPrice);

        
        ApplyOrderDiscount();
        ApplyDeliveryCost();
    }

    private void ApplyOrderDiscount()
    {
        // Default values
        decimal thresholdAmount = 100;
        decimal discountPercentage = 5;

        // Try to get values from configuration
        string configThreshold = ConfigurationManager.AppSettings["OrderDiscountThreshold"];
        if (!string.IsNullOrEmpty(configThreshold) && decimal.TryParse(configThreshold, out decimal parsedThreshold))
        {
            thresholdAmount = parsedThreshold;
        }

        string configDiscount = ConfigurationManager.AppSettings["OrderDiscountPercentage"];
        if (!string.IsNullOrEmpty(configDiscount) && decimal.TryParse(configDiscount, out decimal parsedDiscount))
        {
            discountPercentage = parsedDiscount;
        }

        // Apply discount if subtotal exceeds threshold
        if (Subtotal >= thresholdAmount)
        {
            Discount = Math.Round(Subtotal * (discountPercentage / 100), 2);
        }
        else
        {
            Discount = 0;
        }
    }

    private void ApplyDeliveryCost()
    {
        // Default values
        decimal freeDeliveryThreshold = 50;
        decimal deliveryCostValue = 15;

        // Try to get values from configuration
        string configThreshold = ConfigurationManager.AppSettings["FreeDeliveryThreshold"];
        if (!string.IsNullOrEmpty(configThreshold) && decimal.TryParse(configThreshold, out decimal parsedThreshold))
        {
            freeDeliveryThreshold = parsedThreshold;
        }

        string configCost = ConfigurationManager.AppSettings["DeliveryCost"];
        if (!string.IsNullOrEmpty(configCost) && decimal.TryParse(configCost, out decimal parsedCost))
        {
            deliveryCostValue = parsedCost;
        }

        // Apply delivery cost if subtotal is below threshold
        if (Subtotal < freeDeliveryThreshold)
        {
            DeliveryCost = deliveryCostValue;
        }
        else
        {
            DeliveryCost = 0;
        }
    }

    private void RecalculateTotal()
    {
        TotalCost = Subtotal - Discount + DeliveryCost;
    }

    private bool CanPlaceOrder()
    {
        return CartItems.Count > 0 &&
               !string.IsNullOrWhiteSpace(DeliveryAddress) &&
               !IsLoading;
    }

    private void PlaceOrder()
    {
        if (!CanPlaceOrder())
            return;

        IsLoading = true;

        try
        {
            // Generate a unique order code
            string orderCode = GenerateOrderCode();

            // Get current date/time
            DateTime orderDateTime = DateTime.Now;

            // Create the order
            _orderService.CreateOrder(
                orderCode,
                _currentUser.UserID,
                orderDateTime,
                OrderStatus.Registered,
                EstimatedDelivery,
                Discount,
                DeliveryCost,
                TotalCost);

            // Get the newly created order ID
            var newOrder = _orderService.GetOrdersByUser(_currentUser.UserID)
                .FirstOrDefault(o => o.OrderCode == orderCode);

            if (newOrder != null)
            {
                // Add OrderDish items
                using (var context = new Restaurant.Data.Context.RestaurantContext())
                {
                    // First add all order dishes
                    foreach (var item in CartItems.Where(i => i.ItemType == CartItemType.Dish))
                    {
                        if (item.Preparat != null)
                        {
                            var orderDish = new Restaurant.Domain.Entities.OrderDish
                            {
                                OrderID = newOrder.OrderID,
                                PreparatID = item.Preparat.PreparatID,
                                Quantity = item.Quantity,
                                UnitPrice = item.UnitPrice
                            };

                            context.OrderDishes.Add(orderDish);

                            // Update the preparat's stock quantity
                            var preparat = context.Preparate.Find(item.Preparat.PreparatID);
                            if (preparat != null)
                            {
                                preparat.QuantityTotal -= item.Quantity * preparat.QuantityPortie;
                                if (preparat.QuantityTotal < 0)
                                    preparat.QuantityTotal = 0;
                            }
                        }
                    }

                    // Then add all order menus
                    foreach (var item in CartItems.Where(i => i.ItemType == CartItemType.Menu))
                    {
                        if (item.Menu != null)
                        {
                            var orderMenu = new Restaurant.Domain.Entities.OrderMenu
                            {
                                OrderID = newOrder.OrderID,
                                MenuID = item.Menu.MenuID,
                                Quantity = item.Quantity,
                                UnitPrice = item.UnitPrice
                            };

                            context.OrderMenus.Add(orderMenu);

                            // Update the stock quantities for all preparate in the menu
                            foreach (var menuPreparat in item.Menu.MenuPreparate)
                            {
                                var preparat = context.Preparate.Find(menuPreparat.PreparatID);
                                if (preparat != null)
                                {
                                    preparat.QuantityTotal -= item.Quantity * menuPreparat.QuantityMenuPortie;
                                    if (preparat.QuantityTotal < 0)
                                        preparat.QuantityTotal = 0;
                                }
                            }
                        }
                    }

                    // Save all changes in a single transaction
                    context.SaveChanges();
                }

                MessageBox.Show(
                    "Comanda a fost plasată cu succes!",
                    "Succes",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                // Notify listeners that order has been placed
                OrderPlaced?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                MessageBox.Show(
                    "Comanda a fost creată, dar nu s-a putut găsi pentru a adăuga produsele.",
                    "Avertisment",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Eroare la plasarea comenzii: {ex.Message}",
                "Eroare",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }
    private string GenerateOrderCode()
    {
        // Generate a unique order code (e.g., ORD-YYYYMMDD-XXXX)
        string dateFormat = DateTime.Now.ToString("yyyyMMdd");
        string randomPart = new Random().Next(1000, 9999).ToString();
        return $"ORD-{dateFormat}-{randomPart}";
    }

    private void CancelOrder()
    {
        // Notify listeners that order creation has been canceled
        CancelRequested?.Invoke(this, EventArgs.Empty);
    }
}

