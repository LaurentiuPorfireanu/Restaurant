using Restaurant.Data.Context;
using Restaurant.DataAccess.Interfaces;
using Restaurant.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Restaurant.DataAccess.Repos
{
    public class PreparatRepository: IPreparatRepository
    {
        private readonly RestaurantContext _context;


        public PreparatRepository(RestaurantContext ctx) => _context = ctx;

        public IEnumerable<Preparat> GetByCategory(int categoryId)
        {
            return _context.Preparate
                .Where(p => p.CategoryID == categoryId)
                .Include(p => p.Fotos)  // Includerea relației cu fotografiile
                .Include(p => p.PreparatAlergens)  // Includerea relației cu alergenii
                    .ThenInclude(pa => pa.Alergen)  // Includerea entității Alergen
                .AsNoTracking()
                .ToList();
        }


        public void Insert(
              string Name,
            decimal Price,
            int QuantityPortie,
            int QuantityTotal,
            int CategoryID)
        {

            _context.Database.ExecuteSqlRaw(
                "EXEC spInsertPreparat " +
                "@Denumire = {0}, @Price = {1}, " +
                "@QuantityPortie = {2}, @QuantityTotal = {3}, " +
                "@CategoryID = {4}",
                Name, Price, QuantityPortie, QuantityTotal, CategoryID);
        }


        public void Update(
            int PreparatID,
            string Name,
            decimal Price,
            int QuantityPortie,
            int QuantityTotal,
            int CategoryID)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC spUpdatePreparat " +
                "@PreparatID = {0}, @Name = {1}, @Price = {2}, " +
                "@QuantityPortiee = {3}, @QuantityTotal = {4}, @CategoryId = {5}",
                PreparatID, Name, Price, QuantityPortie, QuantityTotal, CategoryID);
        }


        public void Delete(int preparatId)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC spDeletePreparat @PreparatID = {0}",
                preparatId);
        }

    }
}
