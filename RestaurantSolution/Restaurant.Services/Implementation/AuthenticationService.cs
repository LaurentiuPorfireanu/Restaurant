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

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("AuthService: Email sau parolă goală", "AuthService");
                return null;
            }

            var user = _userRepository.GetByEmail(email);

          
            if (user == null)
            {
                Console.WriteLine($"AuthService: Utilizatorul cu email {email} nu a fost găsit în baza de date", "AuthService");
                return null;
            }
            else
            {
                Console.WriteLine($"AuthService: Utilizator găsit!\nID: {user.UserID}\nNume: {user.FirstName} {user.LastName}\nEmail: {user.Email}\nHash: {user.PasswordHash}", "AuthService");
            }

           
            string computedHash = HashPassword(password);

            
            Console.WriteLine($"AuthService: Comparare hash-uri:\nHash stocat în DB: {user.PasswordHash}\nHash calculat pentru parola introdusă: {computedHash}", "AuthService");

           
            bool isMatch = VerifyPassword(user.PasswordHash, password);

         
            Console.WriteLine($"AuthService: Verificare parolă: {(isMatch ? "REUȘITĂ" : "EȘUATĂ")}", "AuthService");

            if (!isMatch)
                return null;

            return user;
        }


        public bool VerifyPassword(string passwordHash, string password)
        {
            if (string.IsNullOrEmpty(passwordHash) || string.IsNullOrEmpty(password))
                return false;

           
            string computedHash = HashPassword(password);
            return computedHash.Equals(passwordHash);
        }

        public string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password cannot be null or empty", nameof(password));

            
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}