using Restaurant.DataAccess.Interfaces;
using Restaurant.Domain.Entities;
using Restaurant.Services.Interfaces;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Services.Implementation
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;

        public AuthenticationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            // Debugging info - parametrii primiți
            Console.WriteLine($"AuthService: Încercare autentificare cu:\nEmail: {email}\nParolă: {password}", "AuthService");

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("AuthService: Email sau parolă goală", "AuthService");
                return null;
            }

            var user = _userRepository.GetByEmail(email);

            // Debugging info - rezultatul căutării utilizatorului
            if (user == null)
            {
                Console.WriteLine($"AuthService: Utilizatorul cu email {email} nu a fost găsit în baza de date", "AuthService");
                return null;
            }
            else
            {
                Console.WriteLine($"AuthService: Utilizator găsit!\nID: {user.UserID}\nNume: {user.FirstName} {user.LastName}\nEmail: {user.Email}\nHash: {user.PasswordHash}", "AuthService");
            }

            // Calculează hash-ul parolei introduse pentru comparație
            string computedHash = HashPassword(password);

            // Debugging info - compararea hash-urilor
            Console.WriteLine($"AuthService: Comparare hash-uri:\nHash stocat în DB: {user.PasswordHash}\nHash calculat pentru parola introdusă: {computedHash}", "AuthService");

            // Verifică parola
            bool isMatch = VerifyPassword(user.PasswordHash, password);

            // Debugging info - rezultat verificare
            Console.WriteLine($"AuthService: Verificare parolă: {(isMatch ? "REUȘITĂ" : "EȘUATĂ")}", "AuthService");

            if (!isMatch)
                return null;

            return user;
        }


        public bool VerifyPassword(string passwordHash, string password)
        {
            if (string.IsNullOrEmpty(passwordHash) || string.IsNullOrEmpty(password))
                return false;

            // Compare the stored hash with a newly computed hash of the password
            string computedHash = HashPassword(password);
            return computedHash.Equals(passwordHash);
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
    }
}