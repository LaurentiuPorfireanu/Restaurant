using Restaurant.Domain.Enums;
using Restaurant.Services.Interfaces;
using Restaurant.ViewModels.Base;
using Restaurant.ViewModels.Commands;
using System;
using System.Windows;
using System.Windows.Input;

namespace Restaurant.ViewModels.Registration
{
    public class RegistrationViewModel : ViewModelBase
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authService;

        // Cheia de admin - în producție, aceasta ar trebui să fie stocată securizat
        // sau verificată printr-un serviciu extern
        private const string ADMIN_KEY = "admin123";

        private string _firstName;
        private string _lastName;
        private string _email;
        private string _phone;
        private string _address;
        private string _password;
        private string _confirmPassword;
        private string _adminKey;
        private string _errorMessage;
        private bool _isLoading;

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
                OnPropertyChanged(nameof(CanRegister));
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
                OnPropertyChanged(nameof(CanRegister));
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
                OnPropertyChanged(nameof(CanRegister));
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged(nameof(Phone));
                OnPropertyChanged(nameof(CanRegister));
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
                OnPropertyChanged(nameof(CanRegister));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
                OnPropertyChanged(nameof(CanRegister));
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
                OnPropertyChanged(nameof(CanRegister));
            }
        }

        public string AdminKey
        {
            get => _adminKey;
            set
            {
                _adminKey = value;
                OnPropertyChanged(nameof(AdminKey));
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
                OnPropertyChanged(nameof(CanRegister));
            }
        }

        public bool CanRegister =>
            !string.IsNullOrWhiteSpace(FirstName) &&
            !string.IsNullOrWhiteSpace(LastName) &&
            !string.IsNullOrWhiteSpace(Email) &&
            !string.IsNullOrWhiteSpace(Password) &&
            !string.IsNullOrWhiteSpace(ConfirmPassword) &&
            !IsLoading;

        public ICommand RegisterCommand { get; }
        public ICommand BackToLoginCommand { get; }

        public event EventHandler RegistrationSuccessful;
        public event EventHandler BackToLoginRequested;

        public RegistrationViewModel(IUserService userService, IAuthenticationService authService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));

            RegisterCommand = new RelayCommand(ExecuteRegister, _ => CanRegister);
            BackToLoginCommand = new RelayCommand(ExecuteBackToLogin);
        }

        private void ExecuteRegister(object parameter)
        {
            try
            {
                // Resetarea mesajului de eroare
                ErrorMessage = string.Empty;
                IsLoading = true;

                // Validarea datelor
                if (!ValidateInput())
                    return;

                // Verificarea dacă emailul există deja
                var existingUser = _userService.GetUserByEmail(Email);
                if (existingUser != null)
                {
                    ErrorMessage = "Există deja un cont cu această adresă de email.";
                    return;
                }

                // Generarea hash-ului pentru parolă
                string passwordHash = _authService.HashPassword(Password);

                // Stabilirea rolului în funcție de cheia de admin
                UserRole role = UserRole.Registered; // Valoarea implicită: utilizator normal

                // Verifică dacă utilizatorul a introdus cheia de admin corectă
                if (!string.IsNullOrWhiteSpace(AdminKey) && AdminKey == ADMIN_KEY)
                {
                    role = UserRole.Admin;
                }

                // Crearea contului de utilizator
                _userService.CreateUser(
                    FirstName,
                    LastName,
                    Email,
                    Phone,
                    Address,
                    passwordHash,
                    (int)role);

                // Notificare personalizată în funcție de rol
                string rolMessage = role == UserRole.Admin ? "administrator" : "client";

                MessageBox.Show(
                    $"Contul de {rolMessage} a fost creat cu succes! Acum te poți autentifica.",
                    "Înregistrare reușită",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                // Declanșarea evenimentului de înregistrare reușită
                RegistrationSuccessful?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Eroare la crearea contului: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private bool ValidateInput()
        {
            // Validare prenume
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                ErrorMessage = "Prenumele este obligatoriu.";
                return false;
            }

            // Validare nume
            if (string.IsNullOrWhiteSpace(LastName))
            {
                ErrorMessage = "Numele este obligatoriu.";
                return false;
            }

            // Validare email
            if (string.IsNullOrWhiteSpace(Email))
            {
                ErrorMessage = "Adresa de email este obligatorie.";
                return false;
            }

            if (!IsValidEmail(Email))
            {
                ErrorMessage = "Adresa de email nu este validă.";
                return false;
            }

            // Validare parolă
            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Parola este obligatorie.";
                return false;
            }

            if (Password.Length < 6)
            {
                ErrorMessage = "Parola trebuie să aibă minim 6 caractere.";
                return false;
            }

            // Validare confirmare parolă
            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Parolele nu corespund.";
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void ExecuteBackToLogin(object parameter)
        {
            // Declanșarea evenimentului pentru navigarea înapoi la login
            BackToLoginRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}