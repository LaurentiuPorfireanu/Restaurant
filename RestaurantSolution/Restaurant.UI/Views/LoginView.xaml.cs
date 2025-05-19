using Microsoft.Extensions.DependencyInjection;
using Restaurant.Services.Interfaces;
using Restaurant.ViewModels.Login;
using Restaurant.ViewModels.State;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Restaurant.UI.Views
{
    public partial class LoginView : Window
    {
        private readonly LoginViewModel _viewModel;

        public LoginView(IAuthenticationService authService)
        {
            InitializeComponent();

            _viewModel = new LoginViewModel(authService);
            DataContext = _viewModel;

          
            PasswordBox.PasswordChanged += PasswordBox_PasswordChanged;

   
            _viewModel.LoginSuccessful += ViewModel_LoginSuccessful;
            _viewModel.ContinueAsGuestRequested += ViewModel_ContinueAsGuestRequested;
            _viewModel.CreateAccountRequested += ViewModel_CreateAccountRequested;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.Password = PasswordBox.Password;
        }

        private void ViewModel_LoginSuccessful(object sender, EventArgs e)
        {

            var mainWindow = App.ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
            Close();
        }

        private void ViewModel_ContinueAsGuestRequested(object sender, EventArgs e)
        {
            CurrentUserState.Instance.CurrentUser = null;
            var mainWindow = App.ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
            Close();
        }

        private void ViewModel_CreateAccountRequested(object sender, EventArgs e)
        {
       
            var registrationView = App.ServiceProvider.GetRequiredService<RegistrationView>();
            registrationView.Show();
            Close();
        }
    }
}