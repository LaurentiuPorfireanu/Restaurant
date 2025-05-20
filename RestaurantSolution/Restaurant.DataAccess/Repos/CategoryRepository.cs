using Microsoft.EntityFrameworkCore;
using Restaurant.Data.Context;
using Restaurant.DataAccess.Interfaces;
using Restaurant.Domain.Entities;


namespace Restaurant.DataAccess.Repos
{
    public class CategoryRepository:ICategoryRepository
    {

        private readonly RestaurantContext _context;

        public CategoryRepository(RestaurantContext context)
        {
            _context = context;
        }


        public IEnumerable<Category> GetAll()
        {
            return _context.Categories
                       .FromSqlRaw("EXEC spGetAllCategories")
                       .AsNoTracking()
                       .ToList();
        }

        public Category GetById(int id)
        {
            return _context.Categories
                       .FromSqlRaw("EXEC spGetCategoryById @p0", id)
                       .AsNoTracking()
                       .FirstOrDefault();
        }

        public void Insert(string name)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC spInsertCategory @Name = {0}",
                name);
        }


        public void Update(int id, string name)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC spUpdateCategory @CategoryID = {0}, @Name = {1}",
                id, name);
        }

        public void Delete(int id)
        {
            try
            {
                
                var result = _context.Database.ExecuteSqlRaw(
                    "EXEC spDeleteCategoryWithCascade @CategoryId = {0}", id);

                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in CategoryRepository.Delete: {ex.Message}");
                throw;
            }
        }

    }





}
