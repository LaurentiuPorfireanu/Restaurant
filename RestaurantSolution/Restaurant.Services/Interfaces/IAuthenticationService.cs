using Restaurant.Domain.Entities;
using System.Threading.Tasks;

namespace Restaurant.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<User> AuthenticateAsync(string email, string password);
        bool VerifyPassword(string passwordHash, string password);
        string HashPassword(string password);
    }
}