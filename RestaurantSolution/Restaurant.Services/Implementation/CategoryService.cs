using System.Collections.Generic;
using Restaurant.Domain.Entities;
using Restaurant.Services.Interfaces;
using Restaurant.DataAccess.Interfaces;

namespace Restaurant.Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;

        public CategoryService(ICategoryRepository repo)
            => _repo = repo;

        public IEnumerable<Category> GetAllCategories()
            => _repo.GetAll();

        public Category GetCategoryById(int id)
            => _repo.GetById(id);

        public void CreateCategory(string name)
        {
            
            _repo.Insert(name);
        }

        public void UpdateCategory(int id, string name)
        {
            
            _repo.Update(id, name);
        }

        public void DeleteCategory(int id)
        {
            
            _repo.Delete(id);
        }
    }
}
