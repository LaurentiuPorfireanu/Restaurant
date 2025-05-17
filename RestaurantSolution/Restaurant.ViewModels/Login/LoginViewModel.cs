using Restaurant.Services.Interfaces;
using Restaurant.ViewModels.Base;
using Restaurant.ViewModels.Commands;
using Restaurant.ViewModels.State;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Security.Cryptography;

namespace Restaurant.ViewModels.Login
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authService;

        private string _email;
        private string _password;
        private string _errorMessage;
        private bool _isLoading;

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

        public ICommand LoginCommand { get; }

        public event EventHandler LoginSuccessful;

        public LoginViewModel(IAuthenticationService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            LoginCommand = new RelayCommand(ExecuteLogin, _ => CanLogin);
        }
        public string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password cannot be null or empty", nameof(password));

            // Folosim SHA256 simplu, exact ca în scriptul SQL
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
        private async void ExecuteLogin(object parameter)
        {
            try
            {
                ErrorMessage = string.Empty;
                IsLoading = true;

                // Debugging info - ce date sunt trimise
                MessageBox.Show($"Încercare autentificare cu:\nEmail: {Email}\nParolă: {Password}", "Date Login");
                string hash = HashPassword("Lau2905");
                Console.WriteLine($"Hash pentru Lau2905: {hash}");

                var user = await _authService.AuthenticateAsync(Email, Password);

                // Debugging info - rezultatul autentificării
                if (user != null)
                {
                    MessageBox.Show($"Autentificare reușită!\nUtilizator: {user.FirstName} {user.LastName}\nEmail: {user.Email}\nRol: {user.Role}",
                                   "Autentificare Reușită");

                    CurrentUserState.Instance.CurrentUser = user;
                    LoginSuccessful?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    MessageBox.Show("Autentificare eșuată - utilizatorul nu a fost găsit sau parola este incorectă",
                                  "Autentificare Eșuată");
                    ErrorMessage = "Email sau parolă incorecte.";
                }
            }
            catch (Exception ex)
            {
                // Debugging info - excepția detaliată
                MessageBox.Show($"Excepție: {ex.ToString()}", "Eroare Autentificare");
                ErrorMessage = $"Eroare la autentificare: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }


    }
}