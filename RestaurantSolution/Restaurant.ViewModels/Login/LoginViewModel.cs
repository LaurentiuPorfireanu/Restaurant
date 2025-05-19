using Restaurant.Services.Interfaces;
using Restaurant.ViewModels.Base;
using Restaurant.ViewModels.Commands;
using Restaurant.ViewModels.State;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Restaurant.ViewModels.Login
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authService;

        private string _email;
        private string _password;
        private string _errorMessage;
        private bool _isLoading;

        public ICommand LoginCommand { get; }
        public ICommand ContinueAsGuestCommand { get; }
        public ICommand CreateAccountCommand { get; }

        public event EventHandler LoginSuccessful;
        public event EventHandler ContinueAsGuestRequested;
        public event EventHandler CreateAccountRequested;

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
                OnPropertyChanged(nameof(CanLogin));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
                OnPropertyChanged(nameof(CanLogin));
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
                OnPropertyChanged(nameof(HasError));
            }
        }

        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public bool CanLogin => !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password) && !IsLoading;


        public LoginViewModel(IAuthenticationService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));

            LoginCommand = new RelayCommand(ExecuteLogin, _ => CanLogin);
            ContinueAsGuestCommand = new RelayCommand(ExecuteContinueAsGuest);
            CreateAccountCommand = new RelayCommand(ExecuteCreateAccount);
        }

        private async void ExecuteLogin(object parameter)
        {
            try
            {
                ErrorMessage = string.Empty;
                IsLoading = true;

                var user = await _authService.AuthenticateAsync(Email, Password);
                if (user != null)
                {
                    
                    MessageBox.Show("Login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    CurrentUserState.Instance.CurrentUser = user;
                    LoginSuccessful?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    ErrorMessage = "Email sau parolă incorecte.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Eroare la autentificare: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ExecuteContinueAsGuest(object parameter)
        {
            
            CurrentUserState.Instance.CurrentUser = null;

           
            ContinueAsGuestRequested?.Invoke(this, EventArgs.Empty);
        }

        private void ExecuteCreateAccount(object parameter)
        {
            
            CreateAccountRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}