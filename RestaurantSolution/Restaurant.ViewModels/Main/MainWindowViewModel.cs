using Restaurant.Domain.Entities;
using Restaurant.Domain.Enums;
using Restaurant.ViewModels.Base;
using Restaurant.ViewModels.Commands;
using Restaurant.ViewModels.RestaurantMenu;
using Restaurant.ViewModels.Search;
using Restaurant.ViewModels.Order;
using Restaurant.ViewModels.Admin;
using Restaurant.ViewModels.State;
using System;
using System.Windows;
using System.Windows.Input;

namespace Restaurant.ViewModels.Main
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider;
        private object _currentView;
        private string _userFullName;
        private bool _isUserLoggedIn;
        private bool _isClientLoggedIn;
        private bool _isEmployeeLoggedIn;

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public string UserFullName
        {
            get => _userFullName;
            set
            {
                _userFullName = value;
                OnPropertyChanged(nameof(UserFullName));
            }
        }

        public bool IsUserLoggedIn
        {
            get => _isUserLoggedIn;
            set
            {
                _isUserLoggedIn = value;
                OnPropertyChanged(nameof(IsUserLoggedIn));
            }
        }

        public bool IsClientLoggedIn
        {
            get => _isClientLoggedIn;
            set
            {
                _isClientLoggedIn = value;
                OnPropertyChanged(nameof(IsClientLoggedIn));
            }
        }

        public bool IsEmployeeLoggedIn
        {
            get => _isEmployeeLoggedIn;
            set
            {
                _isEmployeeLoggedIn = value;
                OnPropertyChanged(nameof(IsEmployeeLoggedIn));
            }
        }

        // Comenzi pentru navigare
        public ICommand NavigateToMenuCommand { get; }
        public ICommand NavigateToSearchCommand { get; }
        public ICommand NavigateToMyOrdersCommand { get; }
        public ICommand NavigateToAdminCommand { get; }
        public ICommand LoginCommand { get; }
        public ICommand LogoutCommand { get; }

        public MainWindowViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            // Inițializare comenzi
            NavigateToMenuCommand = new RelayCommand(_ => NavigateToMenu());
            NavigateToSearchCommand = new RelayCommand(_ => NavigateToSearch());
            NavigateToMyOrdersCommand = new RelayCommand(_ => NavigateToMyOrders(), _ => IsClientLoggedIn);
            NavigateToAdminCommand = new RelayCommand(_ => NavigateToAdmin(), _ => IsEmployeeLoggedIn);
            LoginCommand = new RelayCommand(_ => ShowLoginDialog());
            LogoutCommand = new RelayCommand(_ => Logout());

            // Setează pagina implicită la meniul restaurantului
            NavigateToMenu();
        }

        public void InitializeForUser(User user)
        {
            if (user != null)
            {
                UserFullName = $"{user.FirstName} {user.LastName}";
                IsUserLoggedIn = true;
                IsClientLoggedIn = user.Role == UserRole.Registered || user.Role == UserRole.Admin;
                IsEmployeeLoggedIn = user.Role == UserRole.Admin;
            }
        }

        public void InitializeAsGuest()
        {
            UserFullName = string.Empty;
            IsUserLoggedIn = false;
            IsClientLoggedIn = false;
            IsEmployeeLoggedIn = false;
        }

        private void NavigateToMenu()
        {
            var menuViewModel = _serviceProvider.GetService(typeof(RestaurantMenuViewModel));
            CurrentView = menuViewModel;
        }

        private void NavigateToSearch()
        {
            var searchViewModel = _serviceProvider.GetService(typeof(SearchViewModel));
            CurrentView = searchViewModel;
        }

        private void NavigateToMyOrders()
        {
            var ordersViewModel = _serviceProvider.GetService(typeof(MyOrdersViewModel));
            CurrentView = ordersViewModel;
        }

        private void NavigateToAdmin()
        {
            var adminViewModel = _serviceProvider.GetService(typeof(AdminPanelViewModel));
            
            CurrentView = adminViewModel;
            
        }

        private void ShowLoginDialog()
        {
            // Aici folosim o acțiune definită în MainWindow.xaml.cs pentru a afișa dialogul de login
            // Putem folosi un mecanism de mesagerie sau un eveniment pentru a notifica MainWindow
            ShowLoginRequested?.Invoke(this, EventArgs.Empty);
        }

        private void Logout()
        {
            MessageBoxResult result = MessageBox.Show(
                "Ești sigur că vrei să te deconectezi?",
                "Confirmare deconectare",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                CurrentUserState.Instance.Logout();
                InitializeAsGuest();
                NavigateToMenu(); // Navigare înapoi la pagina principală
            }
        }

        // Eveniment pentru notificarea MainWindow să afișeze dialogul de login
        public event EventHandler ShowLoginRequested;
    }
}