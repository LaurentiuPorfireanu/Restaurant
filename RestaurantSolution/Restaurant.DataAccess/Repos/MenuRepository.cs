using Microsoft.EntityFrameworkCore;
using Restaurant.Data.Context;
using Restaurant.DataAccess.Interfaces;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DataAccess.Repos
{
    public class MenuRepository : IMenuRepository
    {
        private readonly RestaurantContext _context;

        public MenuRepository(RestaurantContext context)
        {
            _context = context;
        }

        public IEnumerable<Menu> GetAll()
        {
            return _context.Menus.FromSqlRaw("EXEC spGetAllMenus").AsNoTracking().ToList();
        }

        public IEnumerable<Menu> GetByCategory(int categoryId)
        {
            return _context.Menus
                       .FromSqlRaw("EXEC spGetMenusByCategory @p0", categoryId)
                       .AsNoTracking()
                       .ToList();
        }


        public Menu GetById(int menuId)
        {
            return _context.Menus
                       .FromSqlRaw("EXEC spGetMenuById @p0", menuId)
                       .AsNoTracking()
                       .FirstOrDefault();
        }


        public void Insert(string name, int categoryId)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC spInsertMenu @Name = {0}, @CategoryID = {1}",
                name, categoryId);
        }

        public void Update(int menuId, string name, int categoryId)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC spUpdateMenu @MenuID = {0}, @Name = {1}, @CategoryID = {2}",
                menuId, name, categoryId);
        }


        public void Delete(int menuId)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC spDeleteMenu @MenuID = {0}",
                menuId);
        }

    }
}
