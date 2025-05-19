using Microsoft.EntityFrameworkCore;
using Restaurant.Data.Context;
using Restaurant.DataAccess.Interfaces;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant.DataAccess.Repos
{
    public class MenuRepository : IMenuRepository
    {
        private readonly RestaurantContext _context;

        public MenuRepository(RestaurantContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<Menu> GetAll()
        {
            // Use the stored procedure but with better entity tracking
            var menus = _context.Menus
                .FromSqlRaw("EXEC spGetAllMenus")
                .AsNoTracking()
                .ToList();

            // Load related entities for each menu
            foreach (var menu in menus)
            {
                // Load the category
                menu.Category = _context.Categories.FirstOrDefault(c => c.CategoryId == menu.CategoryID);

                // Load menu items
                menu.MenuPreparate = _context.MenuPreparate
                    .Include(mp => mp.Preparat)
                    .Where(mp => mp.MenuID == menu.MenuID)
                    .ToList();
            }

            return menus;
        }

        public IEnumerable<Menu> GetAllWithCategories()
        {
            try
            {
                // Use the stored procedure but with proper includes
                var menus = _context.Menus
                    .FromSqlRaw("EXEC spGetAllMenusWithCategories")
                    .AsNoTracking()
                    .ToList();

                // For each menu, manually load its category and menu items
                foreach (var menu in menus)
                {
                    // Load the category if not already loaded
                    if (menu.Category == null && menu.CategoryID > 0)
                    {
                        menu.Category = _context.Categories.Find(menu.CategoryID);
                    }

                    // Load the menu items with their related preparate
                    var menuItems = _context.MenuPreparate
                        .Where(mp => mp.MenuID == menu.MenuID)
                        .Include(mp => mp.Preparat)
                        .ToList();

                    menu.MenuPreparate = menuItems;
                }

                return menus;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetAllWithCategories: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                throw; // Re-throw the exception for handling at a higher level
            }
        }

        public IEnumerable<Menu> GetByCategory(int categoryId)
        {
            var menus = _context.Menus
                .FromSqlRaw("EXEC spGetMenusByCategory @p0", categoryId)
                .AsNoTracking()
                .ToList();

            // Load menu items for each menu
            foreach (var menu in menus)
            {
                menu.MenuPreparate = _context.MenuPreparate
                    .Include(mp => mp.Preparat)
                    .Where(mp => mp.MenuID == menu.MenuID)
                    .ToList();
            }

            return menus;
        }

        public Menu GetById(int menuId)
        {
            if (menuId <= 0)
                throw new ArgumentException("Menu ID must be positive", nameof(menuId));

            try
            {
                System.Diagnostics.Debug.WriteLine($"MenuRepository: GetById called with menuId={menuId}");

                // Obține doar informațiile de bază despre meniu, fără a adăuga operatori LINQ
                var menu = _context.Menus
                    .FromSqlRaw("EXEC spGetMenuById @MenuID", new Microsoft.Data.SqlClient.SqlParameter("@MenuID", menuId))
                    .AsEnumerable() // Adaugă AsEnumerable() înainte de a folosi FirstOrDefault()
                    .FirstOrDefault();

                if (menu == null)
                {
                    System.Diagnostics.Debug.WriteLine("MenuRepository: No menu found with this ID");
                    return null;
                }

                System.Diagnostics.Debug.WriteLine($"MenuRepository: Menu found: {menu.Name}, CategoryID: {menu.CategoryID}");

                // Încarcă categoria separat
                menu.Category = _context.Categories.FirstOrDefault(c => c.CategoryId == menu.CategoryID);
                if (menu.Category != null)
                    System.Diagnostics.Debug.WriteLine($"MenuRepository: Category loaded: {menu.Category.Name}");
                else
                    System.Diagnostics.Debug.WriteLine("MenuRepository: Failed to load category");

                // Încarcă preparatele meniului separat
                menu.MenuPreparate = _context.MenuPreparate
                    .Where(mp => mp.MenuID == menuId)
                    .Include(mp => mp.Preparat)
                    .ToList();

                System.Diagnostics.Debug.WriteLine($"MenuRepository: Loaded {menu.MenuPreparate?.Count ?? 0} menu items");

                return menu;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MenuRepository: Error in GetById - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"MenuRepository: {ex.StackTrace}");
                throw;
            }
        }
        public int Insert(string name, int categoryId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Menu name is required", nameof(name));
            if (categoryId <= 0)
                throw new ArgumentException("Category ID must be positive", nameof(categoryId));

            // Create a new menu entity
            var menu = new Menu
            {
                Name = name,
                CategoryID = categoryId
            };

            // Add to context and save to get the ID
            _context.Menus.Add(menu);
            _context.SaveChanges();

            return menu.MenuID;
        }

        public void Update(int menuId, string name, int categoryId)
        {
            if (menuId <= 0)
                throw new ArgumentException("Menu ID must be positive", nameof(menuId));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Menu name is required", nameof(name));
            if (categoryId <= 0)
                throw new ArgumentException("Category ID must be positive", nameof(categoryId));

            _context.Database.ExecuteSqlRaw(
                "EXEC spUpdateMenu @MenuID = {0}, @Name = {1}, @CategoryID = {2}",
                menuId, name, categoryId);
        }

        public void Delete(int menuId)
        {
            if (menuId <= 0)
                throw new ArgumentException("Menu ID must be positive", nameof(menuId));

            // First, remove all menu items
            RemoveAllPreparate(menuId);

            // Then delete the menu
            _context.Database.ExecuteSqlRaw(
                "EXEC spDeleteMenu @MenuID = {0}", menuId);
        }

        public void AddPreparat(int menuId, int preparatId, int quantityMenuPortie)
        {
            if (menuId <= 0)
                throw new ArgumentException("Menu ID must be positive", nameof(menuId));
            if (preparatId <= 0)
                throw new ArgumentException("Preparat ID must be positive", nameof(preparatId));
            if (quantityMenuPortie <= 0)
                throw new ArgumentException("Quantity must be positive", nameof(quantityMenuPortie));

            _context.Database.ExecuteSqlRaw(
                "EXEC spInsertMenuPreparat @MenuID = {0}, @PreparatID = {1}, @QuantityMenuPortie = {2}",
                menuId, preparatId, quantityMenuPortie);
        }

        public void RemovePreparat(int menuId, int preparatId)
        {
            if (menuId <= 0)
                throw new ArgumentException("Menu ID must be positive", nameof(menuId));
            if (preparatId <= 0)
                throw new ArgumentException("Preparat ID must be positive", nameof(preparatId));

            _context.Database.ExecuteSqlRaw(
                "EXEC spDeleteMenuPreparat @MenuID = {0}, @PreparatID = {1}",
                menuId, preparatId);
        }

        public void RemoveAllPreparate(int menuId)
        {
            if (menuId <= 0)
                throw new ArgumentException("Menu ID must be positive", nameof(menuId));

            _context.Database.ExecuteSqlRaw(
                "EXEC spDeleteAllMenuPreparate @MenuID = {0}", menuId);
        }

        public IEnumerable<MenuPreparat> GetMenuPreparate(int menuId)
        {
            if (menuId <= 0)
                throw new ArgumentException("Menu ID must be positive", nameof(menuId));

            return _context.MenuPreparate
                .FromSqlRaw("EXEC spGetMenuPreparate @MenuID = {0}", menuId)
                .Include(mp => mp.Preparat)
                .AsNoTracking()
                .ToList();
        }
    }
}