using System.Collections.Generic;
using Restaurant.Domain.Entities;

namespace Restaurant.DataAccess.Interfaces
{
    public interface IMenuRepository
    {
        IEnumerable<Menu> GetAll();
        IEnumerable<Menu> GetByCategory(int categoryId);
        Menu GetById(int menuId);
        void Insert(string name, int categoryId);
        void Update(int menuId, string name, int categoryId);
        void Delete(int menuId);
    }
}
