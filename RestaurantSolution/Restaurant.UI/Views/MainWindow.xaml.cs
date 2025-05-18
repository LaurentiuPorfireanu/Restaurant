using Microsoft.Extensions.DependencyInjection;
using Restaurant.ViewModels.Main;
using Restaurant.ViewModels.State;
using System;
using System.Windows;

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

            _viewModel = new MainWindowViewModel(serviceProvider);

            // Abonare la evenimentul de cerere afișare dialog login
            _viewModel.ShowLoginRequested += ViewModel_ShowLoginRequested;

            // Verificăm dacă există un utilizator curent (login/guest)
            if (CurrentUserState.Instance.CurrentUser != null)
            {
                _viewModel.InitializeForUser(CurrentUserState.Instance.CurrentUser);
            }
            else
            {
                _viewModel.InitializeAsGuest();
            }

            // Subscriere la evenimentul de schimbare a utilizatorului
            CurrentUserState.Instance.UserChanged += (s, e) =>
            {
                if (e.User != null)
                {
                    _viewModel.InitializeForUser(e.User);
                }
                else
                {
                    _viewModel.InitializeAsGuest();
                }
            };

            DataContext = _viewModel;
        }

        private void ViewModel_ShowLoginRequested(object sender, EventArgs e)
        {
            var loginView = _serviceProvider.GetRequiredService<LoginView>();
            loginView.ShowDialog();
        }
    }
}