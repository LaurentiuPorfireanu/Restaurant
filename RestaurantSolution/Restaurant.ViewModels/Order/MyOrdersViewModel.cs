using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Enums;
using Restaurant.Services.Interfaces;
using Restaurant.ViewModels.Base;
using Restaurant.ViewModels.Commands;

namespace Restaurant.ViewModels.Order
{
    public class MyOrdersViewModel : ViewModelBase
    {
        private readonly IOrderService _orderService;
        private readonly ICategoryService _categoryService;
        private readonly IPreparatService _preparatService;
        private readonly IMenuService _menuService;
        private readonly User _currentUser;

        private ObservableCollection<OrderViewModel> _orders;
        private bool _isLoading;
        private bool _isAllOrdersSelected;
        private bool _isActiveOrdersSelected;
        private bool _hasNoOrders;

        public ObservableCollection<OrderViewModel> Orders
        {
            get => _orders;
            set
            {
                _orders = value;
                OnPropertyChanged(nameof(Orders));
                HasNoOrders = _orders.Count == 0;
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

        public bool IsAllOrdersSelected
        {
            get => _isAllOrdersSelected;
            set
            {
                _isAllOrdersSelected = value;
                OnPropertyChanged(nameof(IsAllOrdersSelected));

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
                _isActiveOrdersSelected = value;
                OnPropertyChanged(nameof(IsActiveOrdersSelected));

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

        public bool HasNoOrders
        {
            get => _hasNoOrders;
            set
            {
                _hasNoOrders = value;
                OnPropertyChanged(nameof(HasNoOrders));
            }
        }

        public ICommand CancelOrderCommand { get; }
        public ICommand NewOrderCommand { get; }

        public MyOrdersViewModel(IOrderService orderService,
                               ICategoryService categoryService,
                               IPreparatService preparatService,
                               IMenuService menuService,
                               User currentUser)
        {
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _preparatService = preparatService ?? throw new ArgumentNullException(nameof(preparatService));
            _menuService = menuService ?? throw new ArgumentNullException(nameof(menuService));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));

            Orders = new ObservableCollection<OrderViewModel>();

            // Inițializare comenzi
            CancelOrderCommand = new RelayCommand(ExecuteCancelOrder);
            NewOrderCommand = new RelayCommand(_ => ExecuteNewOrder());

            // Setează filtrarea implicită la toate comenzile
            IsAllOrdersSelected = true;
        }

        public async Task LoadOrdersAsync()
        {
            if (IsAllOrdersSelected)
            {
                await LoadAllOrdersAsync();
            }
            else
            {
                await LoadActiveOrdersAsync();
            }
        }

        private async void LoadAllOrders()
        {
            await LoadAllOrdersAsync();
        }

        private async void LoadActiveOrders()
        {
            await LoadActiveOrdersAsync();
        }

        private async Task LoadAllOrdersAsync()
        {
            try
            {
                IsLoading = true;
                Orders.Clear();

                var orders = _orderService.GetOrdersByUser(_currentUser.UserID);
                await PopulateOrdersCollectionAsync(orders);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la încărcarea comenzilor: {ex.Message}", "Eroare",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadActiveOrdersAsync()
        {
            try
            {
                IsLoading = true;
                Orders.Clear();

                var orders = _orderService.GetOrdersByUser(_currentUser.UserID)
                    .Where(o => o.Status != OrderStatus.Delievered && o.Status != OrderStatus.Canceled);

                await PopulateOrdersCollectionAsync(orders);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la încărcarea comenzilor active: {ex.Message}", "Eroare",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task PopulateOrdersCollectionAsync(IEnumerable<Domain.Entities.Order> orders)
        {
            foreach (var order in orders)
            {
                var orderViewModel = new OrderViewModel
                {
                    OrderID = order.OrderID,
                    OrderCode = order.OrderCode,
                    OrderDateTime = order.OrderDateTime,
                    OrderDateFormatted = order.OrderDateTime.ToString("dd.MM.yyyy HH:mm"),
                    Status = order.Status,
                    StatusText = GetStatusText(order.Status),
                    StatusBackground = GetStatusBackgroundColor(order.Status),
                    EstimatedDelivery = order.EstimatedDelivery,
                    EstimatedDeliveryFormatted = order.EstimatedDelivery?.ToString("dd.MM.yyyy HH:mm") ?? "Nespecificat",
                    DeliveryAddress = _currentUser.Address,
                    Discount = order.Discount,
                    DiscountFormatted = $"-{order.Discount:N2} Lei",
                    HasDiscount = order.Discount > 0,
                    DeliveryCost = order.DeliveryCost,
                    DeliveryCostFormatted = $"{order.DeliveryCost:N2} Lei",
                    TotalCost = order.TotalCost,
                    TotalCostFormatted = $"{order.TotalCost:N2} Lei",
                    CanCancel = CanCancelOrder(order.Status),
                    OrderItems = new ObservableCollection<OrderItemViewModel>()
                };

                // Adaugă preparatele din comandă
                if (order.OrderDishes != null)
                {
                    foreach (var dish in order.OrderDishes)
                    {
                        orderViewModel.OrderItems.Add(new OrderItemViewModel
                        {
                            Name = dish.Preparat?.Name ?? "Preparat necunoscut",
                            Quantity = dish.Quantity,
                            UnitPrice = dish.UnitPrice,
                            UnitPriceFormatted = $"{dish.UnitPrice:N2} Lei",
                            TotalPrice = dish.Quantity * dish.UnitPrice,
                            TotalPriceFormatted = $"{dish.Quantity * dish.UnitPrice:N2} Lei"
                        });
                    }
                }

                // Adaugă meniurile din comandă
                if (order.OrderMenus != null)
                {
                    foreach (var menu in order.OrderMenus)
                    {
                        orderViewModel.OrderItems.Add(new OrderItemViewModel
                        {
                            Name = $"Meniu: {menu.Menu?.Name ?? "Necunoscut"}",
                            Quantity = menu.Quantity,
                            UnitPrice = menu.UnitPrice,
                            UnitPriceFormatted = $"{menu.UnitPrice:N2} Lei",
                            TotalPrice = menu.Quantity * menu.UnitPrice,
                            TotalPriceFormatted = $"{menu.Quantity * menu.UnitPrice:N2} Lei"
                        });
                    }
                }

                // Calculează costul produselor (fără transport și discount)
                orderViewModel.ItemsCost = orderViewModel.OrderItems.Sum(item => item.TotalPrice);
                orderViewModel.ItemsCostFormatted = $"{orderViewModel.ItemsCost:N2} Lei";

                Orders.Add(orderViewModel);
            }

            // Sortăm comenzile după dată (cele mai recente primele)
            var sortedOrders = new ObservableCollection<OrderViewModel>(
                Orders.OrderByDescending(o => o.OrderDateTime));

            Orders = sortedOrders;
        }

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

        private bool CanCancelOrder(OrderStatus status)
        {
            // O comandă poate fi anulată doar dacă este înregistrată sau în pregătire
            return status == OrderStatus.Registered || status == OrderStatus.InPreparation;
        }

        private void ExecuteCancelOrder(object parameter)
        {
            if (parameter is not int orderId)
                return;

            try
            {
                // Cere confirmarea utilizatorului
                var result = MessageBox.Show(
                    "Ești sigur că vrei să anulezi această comandă?",
                    "Confirmare anulare",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Anulează comanda
                    _orderService.UpdateOrderStatus(orderId, OrderStatus.Canceled);

                    // Reîncarcă lista de comenzi
                    LoadOrdersAsync();

                    MessageBox.Show(
                        "Comanda a fost anulată cu succes.",
                        "Anulare reușită",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Eroare la anularea comenzii: {ex.Message}",
                    "Eroare",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
        private OrderManagementViewModel _orderManagement;

        // Add this property to the public properties section
        public OrderManagementViewModel OrderManagement
        {
            get => _orderManagement;
            set
            {
                _orderManagement = value;
                OnPropertyChanged(nameof(OrderManagement));
            }
        }

        private void ExecuteNewOrder()
        {
            // Initialize the OrderManagement for a new order
            OrderManagement = new OrderManagementViewModel(
                _orderService,
                _preparatService,
                _menuService,
                _currentUser,
                _categoryService);

            // Load categories and available items
            OrderManagement.Initialize();

            // Switch to order creation mode
            OrderManagement.IsAddMode = true;

            // Subscribe to events
            OrderManagement.OrderPlaced += OrderManagement_OrderPlaced;
            OrderManagement.CancelRequested += OrderManagement_CancelRequested;
        }

        private void OrderManagement_OrderPlaced(object sender, EventArgs e)
        {
            // Reload orders after a new order is placed
            LoadOrdersAsync();

            // Reset view
            OrderManagement.IsAddMode = false;

            // Unsubscribe from events
            OrderManagement.OrderPlaced -= OrderManagement_OrderPlaced;
            OrderManagement.CancelRequested -= OrderManagement_CancelRequested;
        }

        private void OrderManagement_CancelRequested(object sender, EventArgs e)
        {
            // Reset view
            OrderManagement.IsAddMode = false;

            // Unsubscribe from events
            OrderManagement.OrderPlaced -= OrderManagement_OrderPlaced;
            OrderManagement.CancelRequested -= OrderManagement_CancelRequested;
        }
    }

    
    
}