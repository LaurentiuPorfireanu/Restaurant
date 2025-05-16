using System.Collections.Generic;
using Restaurant.Domain.Entities;

namespace Restaurant.Services.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAllCategories();
        Category GetCategoryById(int id);
        void CreateCategory(string name);
        void UpdateCategory(int id, string name);
        void DeleteCategory(int id);
    }
}
