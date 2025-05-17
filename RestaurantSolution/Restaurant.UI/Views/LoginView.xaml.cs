using Microsoft.Extensions.DependencyInjection;
using Restaurant.Services.Interfaces;
using Restaurant.ViewModels.Login;
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

            // Handle password changes (since PasswordBox doesn't support binding)
            PasswordBox.PasswordChanged += PasswordBox_PasswordChanged;

            // Handle successful login
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
            MessageBox.Show("Login successful! Redirect to main window will be implemented later.");
            // TODO: Redirect to main window
        }

        private void ViewModel_ContinueAsGuestRequested(object sender, EventArgs e)
        {
            MessageBox.Show("Continue as guest! Redirect to main window will be implemented later.");
            // TODO: Redirect to main window as guest
        }

        private void ViewModel_CreateAccountRequested(object sender, EventArgs e)
        {
            // Deschide fereastra pentru crearea contului
            var registrationView = App.ServiceProvider.GetRequiredService<RegistrationView>();
            registrationView.Show();
            Close();
        }
    }
}