using System.Collections.Generic;
using Restaurant.Domain.Entities;

namespace Restaurant.DataAccess.Interfaces
{
    public interface IMenuRepository
    {
        IEnumerable<Menu> GetAll();
        IEnumerable<Menu> GetByCategory(int categoryId);
        Menu GetById(int menuId);
        int Insert(string name, int categoryId);
        void Update(int menuId, string name, int categoryId);
        void Delete(int menuId);

        public IEnumerable<Menu> GetAllWithCategories();
        void AddPreparat(int menuId, int preparatId, int quantityMenuPortie);
        void RemovePreparat(int menuId, int preparatId);
        void RemoveAllPreparate(int menuId);
        IEnumerable<MenuPreparat> GetMenuPreparate(int menuId);
    }
}
