using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Restaurant.Data.Context;
using Restaurant.DataAccess.Interfaces;
using Restaurant.Domain.Entities;

namespace Restaurant.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly RestaurantContext _context;
        public UserRepository(RestaurantContext ctx) => _context = ctx;

        public IEnumerable<User> GetAll()
            => _context.Users
                   .FromSqlRaw("EXEC spGetAllUsers")
                   .AsNoTracking()
                   .ToList();

        public User GetById(int id)
            => _context.Users
                   .FromSqlRaw("EXEC spGetUserById @p0", id)
                   .AsNoTracking()
                   .FirstOrDefault();

        public User GetByEmail(string email)
            => _context.Users
                   .FromSqlRaw("EXEC spGetUserByEmail @p0", email)
                   .AsNoTracking()
                    .AsEnumerable()
                   .FirstOrDefault();

        public void Insert(
            string firstName, string lastName, string email,
            string phone, string address, string passwordHash, int role)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC spInsertUser " +
                "@FirstName = {0}, @LastName = {1}, @Email = {2}, " +
                "@Phone = {3}, @Address = {4}, @PasswordHash = {5}, @Role = {6}",
                firstName, lastName, email, phone, address, passwordHash, role);
        }

        public void Update(
            int id, string firstName, string lastName,
            string email, string phone, string address, int role)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC spUpdateUser " +
                "@UserID = {0}, @FirstName = {1}, @LastName = {2}, " +
                "@Email = {3}, @Phone = {4}, @Address = {5}, @Role = {6}",
                id, firstName, lastName, email, phone, address, role);
        }

        public void Delete(int id)
            => _context.Database.ExecuteSqlRaw(
                "EXEC spDeleteUser @UserID = {0}", id);
    }
}
