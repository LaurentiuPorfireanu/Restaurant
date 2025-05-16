using System.Collections.Generic;
using Restaurant.Domain.Entities;

namespace Restaurant.Services.Interfaces
{
    public interface IMenuService
    {
        IEnumerable<Menu> GetAllMenus();
        IEnumerable<Menu> GetMenusByCategory(int categoryId);
        Menu GetMenuById(int menuId);
        void CreateMenu(string name, int categoryId);
        void UpdateMenu(int menuId, string name, int categoryId);
        void DeleteMenu(int menuId);
    }
}
