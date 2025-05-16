using System.Collections.Generic;
using Restaurant.Domain.Entities;

namespace Restaurant.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User GetById(int id);
        User GetByEmail(string email);
        void Insert(
            string firstName, string lastName, string email,
            string phone, string address, string passwordHash, int role);
        void Update(
            int id, string firstName, string lastName,
            string email, string phone, string address, int role);
        void Delete(int id);
    }
}
