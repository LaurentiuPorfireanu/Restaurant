using System.Collections.Generic;
using Restaurant.Domain.Entities;

namespace Restaurant.Services.Interfaces
{
    public interface IMenuService
    {
        
        IEnumerable<Menu> GetAllMenus();

       
        IEnumerable<Menu> GetMenusByCategory(int categoryId);

      
        Menu GetMenuById(int menuId);
        IEnumerable<Menu> GetAllMenusWithCategories();

        int CreateMenu(string name, int categoryId);

       
        void UpdateMenu(int menuId, string name, int categoryId);

       
        void DeleteMenu(int menuId);

      
        void AddPreparatToMenu(int menuId, int preparatId, int quantityMenuPortie);

        
        void RemovePreparatFromMenu(int menuId, int preparatId);

        
        void RemoveAllPreparateFromMenu(int menuId);

        
        decimal CalculateMenuPrice(int menuId);

        
        decimal CalculateMenuPrice(int menuId, decimal discountPercentage);
    }
}