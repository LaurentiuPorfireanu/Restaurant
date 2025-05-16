using System.Collections.Generic;
using Restaurant.Domain.Entities;

namespace Restaurant.Services.Interfaces
{
    public interface IUserService
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(int id);
        User GetUserByEmail(string email);
        void CreateUser(
            string firstName,
            string lastName,
            string email,
            string phone,
            string address,
            string passwordHash,
            int role);
        void UpdateUser(
            int id,
            string firstName,
            string lastName,
            string email,
            string phone,
            string address,
            int role);
        void DeleteUser(int id);
    }
}
