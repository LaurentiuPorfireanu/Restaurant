using Microsoft.Extensions.DependencyInjection;
using Restaurant.Services.Interfaces;
using Restaurant.ViewModels.Registration;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Restaurant.UI.Views
{
    public partial class RegistrationView : Window
    {
        private readonly RegistrationViewModel _viewModel;

        public RegistrationView(IUserService userService, IAuthenticationService authService)
        {
            InitializeComponent();

            _viewModel = new RegistrationViewModel(userService, authService);
            DataContext = _viewModel;

            // Gestionarea evenimentelor pentru PasswordBox și ConfirmPasswordBox
            PasswordBox.PasswordChanged += PasswordBox_PasswordChanged;
            ConfirmPasswordBox.PasswordChanged += ConfirmPasswordBox_PasswordChanged;
            AdminKeyBox.PasswordChanged += AdminKeyBox_PasswordChanged;

            // Abonare la evenimentele din ViewModel
            _viewModel.RegistrationSuccessful += ViewModel_RegistrationSuccessful;
            _viewModel.BackToLoginRequested += ViewModel_BackToLoginRequested;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.Password = PasswordBox.Password;
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.ConfirmPassword = ConfirmPasswordBox.Password;
        }

        private void AdminKeyBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.AdminKey = AdminKeyBox.Password;
        }

        private void ViewModel_RegistrationSuccessful(object sender, EventArgs e)
        {
            // Navigare înapoi la fereastra de login
            var loginView = App.ServiceProvider.GetRequiredService<LoginView>();
            loginView.Show();
            Close();
        }

        private void ViewModel_BackToLoginRequested(object sender, EventArgs e)
        {
            // Navigare înapoi la fereastra de login
            var loginView = App.ServiceProvider.GetRequiredService<LoginView>();
            loginView.Show();
            Close();
        }
    }
}