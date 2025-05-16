using System;
using System.Collections.Generic;
using Restaurant.Domain.Entities;
using Restaurant.Services.Interfaces;
using Restaurant.DataAccess.Interfaces;

namespace Restaurant.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
            => _repo = repo;

        public IEnumerable<User> GetAllUsers()
            => _repo.GetAll();

        public User GetUserById(int id)
        {
            if (id <= 0) throw new ArgumentException("ID invalid", nameof(id));
            return _repo.GetById(id);
        }

        public User GetUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email obligatoriu", nameof(email));
            return _repo.GetByEmail(email);
        }

        public void CreateUser(
            string firstName,
            string lastName,
            string email,
            string phone,
            string address,
            string passwordHash,
            int role)
        {
            if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("Prenume obligatoriu", nameof(firstName));
            if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Nume obligatoriu", nameof(lastName));
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email obligatoriu", nameof(email));
            if (string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentException("Parola obligatorie", nameof(passwordHash));
            _repo.Insert(firstName, lastName, email, phone, address, passwordHash, role);
        }

        public void UpdateUser(
            int id,
            string firstName,
            string lastName,
            string email,
            string phone,
            string address,
            int role)
        {
            if (id <= 0) throw new ArgumentException("ID invalid", nameof(id));
            if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("Prenume obligatoriu", nameof(firstName));
            if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Nume obligatoriu", nameof(lastName));
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email obligatoriu", nameof(email));
            _repo.Update(id, firstName, lastName, email, phone, address, role);
        }

        public void DeleteUser(int id)
        {
            if (id <= 0) throw new ArgumentException("ID invalid", nameof(id));
            _repo.Delete(id);
        }
    }
}
