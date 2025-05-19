using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Services.Interfaces;
using Restaurant.UI.Views.Admin;
using Restaurant.UI.Views.Menu;
using Restaurant.UI.Views.Order;
using Restaurant.UI.Views.Search;
using Restaurant.ViewModels.Main;

namespace Restaurant.UI.Views
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;
        private readonly IServiceProvider _serviceProvider;

        public MainWindow(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;

            _viewModel = new MainWindowViewModel();
            DataContext = _viewModel;

      
            _viewModel.NavigationRequested += ViewModel_NavigationRequested;
            _viewModel.LoginRequested += ViewModel_LoginRequested;
            _viewModel.LogoutConfirmed += ViewModel_LogoutConfirmed;
        }

        private void ViewModel_NavigationRequested(object sender, string target)
        {
            switch (target)
            {
                case "Menu":
                    var menuView = _serviceProvider.GetRequiredService<RestaurantMenuView>();
                    _viewModel.SetCurrentView(menuView);
                    break;
                case "Search":
                    var searchView = _serviceProvider.GetRequiredService<SearchView>();
                    _viewModel.SetCurrentView(searchView);
                    break;
                case "MyOrders":
                    var ordersView = _serviceProvider.GetRequiredService<MyOrdersView>();
                    _viewModel.SetCurrentView(ordersView);
                    break;
                case "Admin":
                    var adminView = _serviceProvider.GetRequiredService<AdminPanelView>();
                    _viewModel.SetCurrentView(adminView);
                    break;
            }
        }

        private void ViewModel_LoginRequested(object sender, EventArgs e)
        {
            var loginView = _serviceProvider.GetRequiredService<LoginView>();
            loginView.ShowDialog();
        }

        private void ViewModel_LogoutConfirmed(object sender, EventArgs e)
        {
        
            ViewModel_NavigationRequested(sender, "Menu");
        }
    }
}