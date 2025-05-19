using System.Collections.Generic;
using Restaurant.Domain.Entities;

namespace Restaurant.Services.Interfaces
{
    public interface IMenuService
    {
        IEnumerable<Menu> GetAllMenus();
        IEnumerable<Menu> GetMenusByCategory(int categoryId);
        Menu GetMenuById(int menuId);
        int CreateMenu(string name, int categoryId);  // Modificat pentru a returna ID-ul meniului creat
        void UpdateMenu(int menuId, string name, int categoryId);
        void DeleteMenu(int menuId);

        // Metode noi pentru gestionarea legăturii între meniuri și preparate
        void AddPreparatToMenu(int menuId, int preparatId, int quantityMenuPortie);
        void RemovePreparatFromMenu(int menuId, int preparatId);
        void RemoveAllPreparateFromMenu(int menuId);

        // Calcularea prețului meniului (cu reducere)
        decimal CalculateMenuPrice(int menuId, decimal discountPercentage);
    }
}