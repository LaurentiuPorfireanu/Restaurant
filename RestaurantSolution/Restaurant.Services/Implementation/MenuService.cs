using System;
using System.Collections.Generic;
using Restaurant.Domain.Entities;
using Restaurant.Services.Interfaces;
using Restaurant.DataAccess.Interfaces;

namespace Restaurant.Services.Implementation
{
    internal class MenuService:IMenuService
    {

        private readonly IMenuRepository _repo;

        public MenuService(IMenuRepository repo)
        {
            _repo = repo;
        }
        public IEnumerable<Menu> GetAllMenus()
            => _repo.GetAll();


        public IEnumerable<Menu> GetMenusByCategory(int categoryId)
        {
            if (categoryId <= 0)
                throw new ArgumentException("Category ID must be positive", nameof(categoryId));
            return _repo.GetByCategory(categoryId);
        }

        public Menu GetMenuById(int menuId)
        {
            if (menuId <= 0)
                throw new ArgumentException("Menu ID must be positive", nameof(menuId));
            return _repo.GetById(menuId);
        }


        public void CreateMenu(string name, int categoryId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Denumire obligatorie", nameof(name));
            if (categoryId <= 0)
                throw new ArgumentException("Categorie invalidă", nameof(categoryId));
            _repo.Insert(name, categoryId);
        }

        public void UpdateMenu(int menuId, string name, int categoryId)
        {
            if (menuId <= 0)
                throw new ArgumentException("Menu ID invalid", nameof(menuId));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Denumire obligatorie", nameof(name));
            if (categoryId <= 0)
                throw new ArgumentException("Categorie invalidă", nameof(categoryId));
            _repo.Update(menuId, name, categoryId);
        }



        public void DeleteMenu(int menuId)
        {
            if (menuId <= 0)
                throw new ArgumentException("Menu ID invalid", nameof(menuId));
            _repo.Delete(menuId);
        }

    }
}
