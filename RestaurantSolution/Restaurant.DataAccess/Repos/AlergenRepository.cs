using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Restaurant.Data.Context;
using Restaurant.DataAccess.Interfaces;
using Restaurant.Domain.Entities;

namespace Restaurant.DataAccess.Repositories
{
    public class AlergenRepository : IAlergenRepository
    {
        private readonly RestaurantContext _context;
        public AlergenRepository(RestaurantContext ctx) => _context = ctx;

        public IEnumerable<Alergen> GetAll()
            => _context.Alergens
                   .FromSqlRaw("EXEC spGetAllAlergens")
                   .AsNoTracking()
                   .ToList();

        public Alergen GetById(int id)
            => _context.Alergens
                   .FromSqlRaw("EXEC spGetAlergenById @p0", id)
                   .AsNoTracking()
                   .FirstOrDefault();

        public void Insert(string name)
            => _context.Database.ExecuteSqlRaw(
                "EXEC spInsertAlergen @Name = {0}", name);

        public void Update(int id, string name)
            => _context.Database.ExecuteSqlRaw(
                "EXEC spUpdateAlergen @AlergenID = {0}, @Name = {1}", id, name);

        public void Delete(int id)
            => _context.Database.ExecuteSqlRaw(
                "EXEC spDeleteAlergen @AlergenID = {0}", id);
    }
}
