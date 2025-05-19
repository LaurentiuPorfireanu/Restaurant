using Restaurant.Domain.Entities;
using Restaurant.Domain.Enums;
using Restaurant.ViewModels.Base;
using Restaurant.ViewModels.Commands;
using Restaurant.ViewModels.State;
using System;
using System.Windows;
using System.Windows.Input;

namespace Restaurant.ViewModels.Main
{
    public class MainWindowViewModel : ViewModelBase
    {
        private object _currentView;
        private string _userFullName;
        private bool _isUserLoggedIn;
        private bool _isClientLoggedIn;
        private bool _isEmployeeLoggedIn;

        // Proprietăți
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

        // Comenzi
        public ICommand NavigateToMenuCommand { get; }
        public ICommand NavigateToSearchCommand { get; }
        public ICommand NavigateToMyOrdersCommand { get; }
        public ICommand NavigateToAdminCommand { get; }
        public ICommand LoginCommand { get; }
        public ICommand LogoutCommand { get; }

        // Evenimente
        public event EventHandler<string> NavigationRequested;
        public event EventHandler LoginRequested;
        public event EventHandler LogoutConfirmed;

        public MainWindowViewModel()
        {
            
            NavigateToMenuCommand = new RelayCommand(_ => RequestNavigation("Menu"));
            NavigateToSearchCommand = new RelayCommand(_ => RequestNavigation("Search"));
            NavigateToMyOrdersCommand = new RelayCommand(_ => RequestNavigation("MyOrders"), _ => IsClientLoggedIn);
            NavigateToAdminCommand = new RelayCommand(_ => RequestNavigation("Admin"), _ => IsEmployeeLoggedIn);
            LoginCommand = new RelayCommand(_ => LoginRequested?.Invoke(this, EventArgs.Empty));
            LogoutCommand = new RelayCommand(_ => ConfirmLogout());

           
            if (CurrentUserState.Instance.CurrentUser != null)
            {
                InitializeForUser(CurrentUserState.Instance.CurrentUser);
            }
            else
            {
                InitializeAsGuest();
            }

            // Abonare la evenimentul de schimbare a utilizatorului
            CurrentUserState.Instance.UserChanged += (s, e) =>
            {
                if (e.User != null)
                {
                    InitializeForUser(e.User);
                }
                else
                {
                    InitializeAsGuest();
                }
            };

            // Navigare implicită
            RequestNavigation("Menu");
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

        public void SetCurrentView(object view)
        {
            CurrentView = view;
        }

        private void RequestNavigation(string target)
        {
            NavigationRequested?.Invoke(this, target);
        }

        private void ConfirmLogout()
        {
            MessageBoxResult result = MessageBox.Show(
                "Ești sigur că vrei să te deconectezi?",
                "Confirmare deconectare",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                CurrentUserState.Instance.Logout();
                LogoutConfirmed?.Invoke(this, EventArgs.Empty);
                RequestNavigation("Menu");
            }
        }
    }
}