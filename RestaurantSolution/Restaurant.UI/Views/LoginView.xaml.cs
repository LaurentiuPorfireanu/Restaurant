using Restaurant.Services.Implementation;
using Restaurant.Services.Interfaces;
using Restaurant.ViewModels.Login;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.Password = PasswordBox.Password;
        }

        private void ViewModel_LoginSuccessful(object sender, EventArgs e)
        {
            MessageBox.Show("Login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}